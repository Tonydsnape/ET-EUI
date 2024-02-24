﻿using System.Collections.Generic;

namespace ET
{
    [ComponentOf(typeof(Scene))]
    [ChildType]
    public class ServerInfosComponent : Entity, IAwake, IDestroy
    {
        public List<ServerInfo> ServerInfosList = new List<ServerInfo>();
        
        public int CurrentServerId = 0;
    }
}