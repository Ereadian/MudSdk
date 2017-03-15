//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Area.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System.Collections.Generic;

    public class Area
	{
        public Area(string name, IReadOnlyList<int> roomIdList)
        {
            this.Name = name;
            this.Rooms = roomIdList;
        }

        public string Name { get; }

        public IReadOnlyList<int> Rooms { get; private set; }
	}
}

