using System;

namespace ET
{
    public class G2M_RequestExitGameHandler: AMActorLocationRpcHandler<Unit, G2M_RequestExitGame, M2G_RequestExitGame>
    {
         protected override async ETTask Run(Unit unit, G2M_RequestExitGame request, M2G_RequestExitGame response, Action reply)
         {
             //TODO：保存玩家是数据到数据库
             
             reply();
            
             // 删除Unit
             await unit.RemoveLocation();
             UnitComponent unitComponent = unit.DomainScene().GetComponent<UnitComponent>(); 
             unitComponent.Remove(unit.Id);
             
             await ETTask.CompletedTask;
         }
    }
}