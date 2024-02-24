using System;

namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public class C2A_GetRolesHandler: AMRpcHandler<C2A_GetRoles, A2C_GetRoles>
    {
        protected override async ETTask Run(Session session, C2A_GetRoles request, A2C_GetRoles response, Action reply)
        {
            if (session.DomainScene().SceneType != SceneType.Account)
            {
                Log.Error($"请求场景类型错误,当前场景类型:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            if(session.GetComponent<SessionLockingComponent>() != null)
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
                    var roleInfos = await DBManagerComponent.Instance.GetZoneDB(session.DomainScene().DomainZone())
                            .Query<RoleInfo>(d => d.AccountId == request.AccountId && d.ServerId == request.ServerId);

                    if (roleInfos != null && roleInfos.Count > 0)
                    {
                        reply();
                        return;
                    }

                    foreach (var roleInfo in roleInfos)
                    {
                        response.RoleInfo.Add(roleInfo.ToMessage());
                        roleInfo?.Dispose();
                    }
                    roleInfos.Clear();

                    reply();
                }
            }

            await ETTask.CompletedTask;
        }
    }
}