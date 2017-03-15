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
        /// <summary>
        /// Area separator character
        /// </summary>
        public const char AreaSeparatorChar = '.';

        private readonly IReadOnlyDictionary<string, Area> areas;
        private readonly IReadOnlyList<Room> rooms;

        public RoomManager(string folder, LocaleManager locales, ColorManager colors)
        {
            var areaCollection = new Dictionary<string, Area>(StringComparer.OrdinalIgnoreCase);
            var roomList = new List<Room>();

            if (Directory.Exists(folder))
            {

                var files = Directory.GetFiles(folder);
                if (files != null)
                {
                    for (var i = 0; i < files.Length; i++)
                    {
                        var file = files[i];

                        var areaData = Singleton<Serializer<AreaData>>.Instance.Deserialize(file);
                        Area area;
                        if (!areaCollection.TryGetValue(areaData.Name, out area))
                        {
                            //area = new Area(areaData.Name);
                            areaCollection.Add(areaData.Name, area);
                        }

                        //area.Load(areaData, roomList, locales, colors);
                    }
                }
            }

            this.areas = areaCollection;
            this.rooms = roomList.ToArray();
        }

        public Room FindRoom(string fullName)
        {
            return FindRoom(this.areas, this.rooms, fullName);
        }

        public Room FindRoom(string area, string room)
        {
            return FindRoom(this.areas, this.rooms, area, room);
        }

        private static Room FindRoom(IReadOnlyDictionary<string, Area> areas, IReadOnlyList<Room> rooms, string fullName)
        {
            var names = fullName.Split(AreaSeparatorChar);
            if (names.Length < 2)
            {
                return null;
            }

            return FindRoom(areas, rooms, names[0], names[1]);
        }

        private static Room FindRoom(IReadOnlyDictionary<string, Area> areas, IReadOnlyList<Room> rooms, string areaName, string roomName)
        {
            Area area;
            if (!areas.TryGetValue(areaName, out area))
            {
                return null;
            }

            int roomId;
            //return area.Rooms.TryGetValue(roomName, out roomId) ? rooms[roomId]: null;
            return null;
        }
	}
}

