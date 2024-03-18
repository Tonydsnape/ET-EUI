using System;

namespace ET
{
    [FriendClass(typeof (RoleInfo))]
    public class C2A_DeleteRoleHandler: AMRpcHandler<C2A_DeleteRole, A2C_DeleteRole>
    {
        protected override async ETTask Run(Session session, C2A_DeleteRole request, A2C_DeleteRole response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求场景类型错误,当前场景类型:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);

            if (token == null || token != request.Token)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session?.Disconnect().Coroutine();
                return;
            }

            using (session.AddComponent<SessionLockingComponent>())
            {
                using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.CreateRole, request.AccountId))
                {
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(request.ServerId)
                            .Query<RoleInfo>(role => role.Id == request.RoleInfoId && role.ServerId == request.ServerId);
                    if (roleInfos == null || roleInfos.Count == 0)
                    {
                        response.Error = ErrorCode.ERR_RoleNotExist;
                        reply();
                        return;
                    }
                    
                    var roleInfo = roleInfos[0];
                    session.AddChild(roleInfo);
                    
                    roleInfo.State = (int)RoleInfoState.Freeze;
                    
                    await DBManagerComponent.Instance.GetZoneDB(request.ServerId).Save(roleInfo);
                    response.DeletedRoleInfoId = roleInfo.Id;
                    roleInfo.Dispose();
                    
                    reply();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}