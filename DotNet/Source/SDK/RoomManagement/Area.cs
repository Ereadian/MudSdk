﻿//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Area.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;

    public class Area
	{
        public Area(string name)
        {
            this.Name = name;
            this.Rooms = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
        }

        public string Name { get; private set; }

        public IDictionary<string, Room> Rooms { get; private set; }
	}
}

