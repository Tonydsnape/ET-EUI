namespace ET
{
    public class ServerInfosComponentDestroySystem: DestroySystem<ServerInfoComponent>
    {
        public override void Destroy(ServerInfoComponent self)
        {
            foreach (var serverInfo in self.ServerInfosList)
            {
                serverInfo?.Dispose();
            }
        }
    }

    [FriendClass(typeof(ServerInfoComponent))]
    public static class ServerInfosComponentSystem
    {
        public static void Add(this ServerInfoComponent self, ServerInfo serverInfo)
        {
            self.ServerInfosList.Add(serverInfo);
        }
    }
}