﻿namespace ET
{
    [FriendClass(typeof(RoleInfosComponent))]
    public class RoleInfoCompomnentDestroySystem: DestroySystem<RoleInfosComponent>
    {
        public override void Destroy(RoleInfosComponent self)
        {
            foreach (var roleInfo in self.RoleInfos)
            {
                roleInfo?.Dispose();
            }
            self.RoleInfos.Clear();
            self.CurrentRoleId = 0;
        }
    }
    
    public static class RoleInfoComponentSystem
    {
        
    }
}