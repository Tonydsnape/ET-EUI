﻿using System;
using System.Text.RegularExpressions;

namespace ET
{
    [FriendClass(typeof (Account))]
    public class C2A_LoginAccountHandler: AMRpcHandler<C2A_LoginAccount, A2C_LoginAccount>
    {
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error("不是Account场景");
                session.Dispose();
                return;
            }

            session.RemoveComponent<SessionAcceptTimeoutComponent>();

            if (session.GetComponent<SessionAcceptTimeoutComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            if (string.IsNullOrEmpty(request.AccountName) || string.IsNullOrEmpty(request.Password))
            {
                response.Error = ErrorCode.ERR_LoginInfoIsNull;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            if (Regex.IsMatch(request.AccountName.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*).(?=.*[a-z].*).{6,20}$")) // 正则表达式验证账号格式
            {
                response.Error = ErrorCode.ERR_AccountNameFromError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            if (Regex.IsMatch(request.Password.Trim(), @"^(?=.*[0-9].*)(?=.*[A-Z].*).(?=.*[a-z].*).{6,20}$")) // 正则表达式验证密码格式
            {
                response.Error = ErrorCode.ERR_PasswordFormError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount, request.AccountName.Trim().GetHashCode()))
                {
                    var accountInfoList = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone())
                            .Query<Account>(d => d.AccountName.Equals(request.AccountName.Trim()));
                    Account account = null;
                    if (accountInfoList != null && accountInfoList.Count > 0)
                    {
                        account = accountInfoList[0];
                        session.AddChild(account);
                        if (account.AccountType == (int)AccountType.BlackList)
                        {
                            response.Error = ErrorCode.ERR_AccountTypeIsBlackList;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }

                        if (!account.Password.Equals(request.Password))
                        {
                            response.Error = ErrorCode.ERR_LoginPasswordError;
                            reply();
                            session.Disconnect().Coroutine();
                            account?.Dispose();
                            return;
                        }
                    }
                    else
                    {
                        account = session.AddChild<Account>();
                        account.AccountName = request.AccountName.Trim();
                        account.Password = request.Password;
                        account.CreateTime = TimeHelper.ServerNow();
                        account.AccountType = (int)AccountType.General;
                        await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
                    }

                    long accountSessionInstanceId = session.DomainScene().GetComponent<AccountSessionsComponent>().Get(account.Id); // 获取sessionId
                    Session otherSession = Game.EventSystem.Get(accountSessionInstanceId) as Session; // 获取session
                    otherSession.Send(new A2C_Disconnect() { Error = 0 }); // 发送断开连接消息
                    otherSession.Disconnect().Coroutine(); // 断开连接
                    session.DomainScene().GetComponent<AccountSessionsComponent>().Add(account.Id, session.InstanceId); // 添加session
                    session.AddComponent<AccountCheckOutTimeComponent, long>(account.Id); // 10分钟后清除session

                    string Token = TimeHelper.ServerNow().ToString() + RandomHelper.RandomNumber(int.MinValue, int.MaxValue).ToString();
                    session.DomainScene().GetComponent<TokenComponent>().Remove(account.Id);
                    session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, Token);

                    response.AccountId = account.Id;
                    response.Token = Token;

                    reply();
                    account?.Dispose();
                }
            }
        }
    }
}