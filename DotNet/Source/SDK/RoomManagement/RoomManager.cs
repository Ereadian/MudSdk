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
                    var area = new Area(areaData, locales, colors);
                    if (collection.ContainsKey(area.Name))
                    {
                        // TODO: show error
                        continue;
                    }

                    collection.Add(area.Name, area);
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
            var name = fullName.Split('.');
            return FindRoom(areas, name[0], name[1]);
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

