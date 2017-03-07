//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Area.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.Globalization;
    using System;
    using System.Collections.Generic;

    public class Area
	{
        private Dictionary<string, Room> roomCollection;

        public Area(string name)
        {
            this.Name = name;
            this.roomCollection = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
        }

        public void Load(AreaData area, LocaleIndex locales, ColorIndex colors)
        {
            var rooms = this.roomCollection;
            for (var i = 0; i < area.Rooms.Length; i++)
            {
                var roomData = area.Rooms[i];
                var room = new Room(this, roomData, locales, colors);
                if (!rooms.ContainsKey(room.Name))
                {
                    rooms.Add(room.Name, room);
                }
            }
        }

        public string Name { get; set; }

        public IReadOnlyDictionary<string, Room> Rooms
        {
            get
            {
                return this.roomCollection;
            }
        }
	}
}

