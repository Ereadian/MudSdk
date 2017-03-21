//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="RoomManager.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.IO;
    using Ereadian.MudSdk.Sdk.Runtime;

    /// <summary>
    /// Room manager
    /// </summary>
    public class RoomManager
	{
        /// <summary>
        /// Area separator character
        /// </summary>
        public const char AreaSeparatorChar = '.';

        /// <summary>
        /// name of area name attribute
        /// </summary>
        public const string AreaNameAttributeName = "name";

        /// <summary>
        /// name of room element
        /// </summary>
        public const string RoomElementName = "room";

        /// <summary>
        /// name of room name attribute
        /// </summary>
        public const string RoomNameAttributeName = "name";

        /// <summary>
        /// room type element name
        /// </summary>
        public const string TypeElementName = "type";

        private IDictionary<string, Area> areas;
        private IDictionary<string, IRoom> rooms;

        public RoomManager(IGameContext context)
        {
            var storage = context.ContentStorage;
            var typeManager = context.TypeManager;
            var localeManager = context.LocaleManager;
            var colorManager = context.ColorManager;


            var syncObject = new object();
            this.areas = new Dictionary<string, Area>(StringComparer.OrdinalIgnoreCase);
            this.rooms = new Dictionary<string, IRoom>(StringComparer.OrdinalIgnoreCase);
            var activeConfigurations = new List<RoomData>();
            var inactiveConfigurations = new List<RoomData>();

            LoadRoomsFromMapFolder(
                storage,
                context.Settings.MapDataFolder,
                context,
                false,
                syncObject,
                activeConfigurations);

            LoadRoomsFromMapFolder(
                storage,
                context.Settings.MapDesignFolder,
                context,
                true,
                syncObject,
                activeConfigurations);

            int phaseId = 0;
            Func<string, IRoom> getRoom = name =>
            {
                lock (syncObject)
                {
                    IRoom room;
                    return rooms.TryGetValue(name, out room) ? room : null;
                }
            };

            while (activeConfigurations.Count > 0)
            {
                inactiveConfigurations.Clear();
                Parallel.For(
                    0,
                    activeConfigurations.Count,
                    index =>
                    {
                        var roomData = activeConfigurations[index];
                        var room = roomData.Area.Rooms[roomData.Name];
                        if (!room.Init(phaseId, roomData.Name, roomData.Area, roomData.InDesign, roomData.Data, context, getRoom))
                        {
                            lock (syncObject)
                            {
                                inactiveConfigurations.Add(roomData);
                            }
                        }
                    });

                var temp = activeConfigurations;
                activeConfigurations = inactiveConfigurations;
                inactiveConfigurations = temp;
            }
        }

        public static string GetRoomFullName(string areaName, string roomName)
        {
            return string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}{2}",
                areaName,
                AreaSeparatorChar,
                roomName);
        }

        public IRoom FindRoom(string fullName)
        {
            IRoom room;
            return this.rooms.TryGetValue(fullName, out room) ? room : null;
        }

        public IRoom FindRoom(string areaName, string roomName)
        {
            return FindRoom(GetRoomFullName(areaName, roomName));
        }

        private void LoadRoomsFromMapFolder(
            IContentStorage storage, 
            string folder, 
            IGameContext context, 
            bool inDesign, 
            object syncObject, 
            IList<RoomData> dataToProcess)
        {
            var files = storage.GetFiles(folder);
            if ((files != null) && (files.Count > 0))
            {
                Parallel.For(
                    0,
                    files.Count,
                    index =>
                    {
                        var path = storage.CombinePath(folder, files[index]);
                        LoadRoomsFromMapFile(storage, path, syncObject, context, inDesign, dataToProcess);
                    });
            }
        }

        private void LoadRoomsFromMapFile(
            IContentStorage storage, 
            string path, 
            object syncObject,
            IGameContext context, 
            bool inDesign,
            IList<RoomData> dataToProcess)
        {
            XmlElement rootElement = storage.LoadXml(path);
            string areaName = rootElement.GetAttribute(AreaNameAttributeName);
            if (string.IsNullOrWhiteSpace(areaName))
            {
                // TODO: write log. area name is required
                return;
            }

            Area area;
            lock (syncObject)
            {
                if (!this.areas.TryGetValue(areaName, out area))
                {
                    area = new Area(areaName);
                    this.areas.Add(areaName, area);
                }
            }

            foreach (XmlElement roomElement in rootElement.SelectNodes(RoomElementName))
            {
                string roomName = roomElement.GetAttribute(RoomNameAttributeName);
                if (string.IsNullOrWhiteSpace(roomName))
                {
                    // TODO: write error. room name is required
                    continue;
                }

                roomName = roomName.Trim();

                IRoom room = null;
                var typeElement = roomElement.SelectSingleNode(TypeElementName) as XmlElement;
                if (typeElement != null)
                {
                    room = RuntimeUtility.CreateInstance<IRoom>(typeElement, context.TypeManager, context.Log);
                }

                if (room == null)
                {
                    room = new Room();
                }

                var fullName = GetRoomFullName(areaName, roomName);
                var roomData = new RoomData
                {
                    Name = roomName,
                    Area = area,
                    InDesign = inDesign,
                    Data = roomElement
                };

                lock (syncObject)
                {
                    if (area.Rooms.ContainsKey(roomName))
                    {
                        // TODO: write error
                    }
                    else
                    {
                        area.Rooms.Add(roomName, room);
                        rooms.Add(fullName, room);
                        dataToProcess.Add(roomData);
                    }
                }
            }
        }

        private static Resource LoadContentFromXml(
            XmlElement contentsElement, 
            LocaleManager localeManager, 
            ColorManager colorManager)
        {
            var list = new List<Text>();
            foreach (XmlElement contentElement in contentsElement.SelectNodes(Resource.ContentElementName))
            {
                var content = new Text(contentElement, localeManager, colorManager);
                list.Add(content);
            }

            return new Resource(list.ToArray());
        }

        private class RoomData
        {
            internal string Name { get; set; }
            internal Area Area { get; set; }
            internal bool InDesign { get; set; }
            internal XmlElement Data { get; set; }
        }
    }
}

