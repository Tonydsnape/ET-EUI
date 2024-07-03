using System;

namespace ET
{
    public class A2R_GetRealmKeyHandler: AMActorRpcHandler<Scene, A2R_GetRealmKey, R2A_GetRealmKey>
    {
        protected override async ETTask Run(Scene scene, A2R_GetRealmKey request, R2A_GetRealmKey response, Action reply)
        {
            if (scene.SceneType != SceneType.Realm)
            {
                Log.Error($"请求场景类型错误,当前场景类型:{scene.SceneType}");
                response.Error = ErrorCode.ERR_RequestSceneTypeError;
                reply();
                return;
            }

            string key = TimeHelper.ServerNow().ToString() + RandomHelper.RandInt64().ToString();
            scene.GetComponent<TokenComponent>().Remove(request.AccountId);
            scene.GetComponent<TokenComponent>().Add(request.AccountId, key);
            response.RealmKey = key.ToString();
            reply();
            await ETTask.CompletedTask;
        }
    }
}