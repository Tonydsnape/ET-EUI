using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class ServerInfoComponent : Entity, IAwake, IDestroy
    {
        public List<ServerInfo> ServerInfosList = new List<ServerInfo>();
    }
}