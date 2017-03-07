//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.Globalization;

    public class RoomManager
	{
        private IReadOnlyDictionary<string, Area> areas;

        public RoomManager(string folder, LocaleIndex locales, ColorIndex colors)
        {
            var collection = new Dictionary<string, Area>(StringComparer.OrdinalIgnoreCase);
            this.areas = collection;

            if (!Directory.Exists(folder))
            {
                return;
            }

            var files = Directory.GetFiles(folder);
            if (files != null)
            {
                for (var i = 0; i < files.Length; i++)
                {
                    var file = files[i];

                    var areaData = Singleton<Serializer<AreaData>>.Instance.Deserialize(file);
                    Area area;
                    if (!collection.TryGetValue(areaData.Name, out area))
                    {
                        area = new Area(areaData.Name);
                        collection.Add(areaData.Name, area);
                    }

                    area.Load(areaData, locales, colors);
                }
            }
        }

        public Room FindRoom(string fullName)
        {
            return FindRoom(this.areas, fullName);
        }

        public Room FindRoom(string area, string room)
        {
            return FindRoom(this.areas, area, room);
        }

        private static Room FindRoom(IReadOnlyDictionary<string, Area> areas, string fullName)
        {
            var names = fullName.Split('.');
            if (names.Length < 2)
            {
                return null;
            }

            return FindRoom(areas, names[0], names[1]);
        }

        private static Room FindRoom(IReadOnlyDictionary<string, Area> areas, string areaName, string roomName)
        {
            Area area;
            if (!areas.TryGetValue(areaName, out area))
            {
                return null;
            }

            Room room;
            return area.Rooms.TryGetValue(roomName, out room) ? room : null;
        }
	}
}

