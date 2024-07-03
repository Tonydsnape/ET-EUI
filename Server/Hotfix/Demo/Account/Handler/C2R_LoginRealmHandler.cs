using System;
using System.Threading.Tasks;

namespace ET
{
    public class C2R_LoginRealmHandler: AMRpcHandler<C2R_LoginRealm, R2C_LoginRealm>
    {
        protected override async ETTask Run(Session session, C2R_LoginRealm request, R2C_LoginRealm response, Action reply)
        {
            if (SceneType.Realm != session.DomainScene().SceneType)
            {
                Log.Error($"请求场景类型错误,当前场景类型:{session.DomainScene().SceneType}");
                session.Dispose();
                return;
            }

            Scene domainScene = session.DomainScene();

            if (session.GetComponent<SessionLockingComponent>() != null)
            {
                response.Error = ErrorCode.ERR_RequestRepeatedly;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            string token = session.DomainScene().GetComponent<TokenComponent>().Get(request.AccountId);

            if (token == null || token != request.RealmTokenKey)
            {
                response.Error = ErrorCode.ERR_NetWorkError;
                reply();
                session.Disconnect().Coroutine();
                return;
            }

            domainScene.GetComponent<TokenComponent>().Remove(request.AccountId);

            using (session.AddComponent<SessionLockingComponent>())
            using (await CoroutineLockComponent.Instance.Wait((int)CoroutineLockType.LoginRealm, request.AccountId))
            {
                StartSceneConfig config = RealmGateAddressHelper.GetGate(domainScene.Zone, request.AccountId);
                
                G2R_GetLoginGateKey g2RGetLoginKey = (G2R_GetLoginGateKey) await MessageHelper.CallActor(config.InstanceId, new R2G_GetLoginGateKey() { AccountId = request.AccountId });
                
                if(g2RGetLoginKey.Error != ErrorCode.ERR_Success)
                {
                    response.Error = g2RGetLoginKey.Error;
                    reply();
                    return;
                }
                
                response.GateSessionKey = g2RGetLoginKey.GateSessionKey;
                response.GateAddress = config.OuterIPPort.ToString();
                reply();
                
                session?.Disconnect().Coroutine();
            }

            await Task.CompletedTask;
        }
    }
}