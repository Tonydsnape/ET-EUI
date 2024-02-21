namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public static class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self, RoleInfoProto roleInfoProto)
        {
            self.Id = roleInfoProto.Id;
            self.Name = roleInfoProto.Name;
            self.ServerId = roleInfoProto.ServerId;
            self.State = roleInfoProto.State;
            self.AccountId = roleInfoProto.AccountId; 
            self.LastLoginTime = roleInfoProto.LastLoginTime;
            self.CreateTime = roleInfoProto.CreateTime;
        }
        
        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            return new RoleInfoProto()
            {
                Id = (int)self.Id, 
                Name = self.Name, 
                ServerId = self.ServerId, 
                State = self.State, 
                AccountId = self.AccountId, 
                LastLoginTime = self.LastLoginTime, 
                CreateTime = self.CreateTime
            };
        }
    }
}