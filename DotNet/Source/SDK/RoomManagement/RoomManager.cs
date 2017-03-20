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
    using IO;

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

        /// <summary>
        /// name of reference type attribute name
        /// </summary>
        public const string TypeNameAttributeName = "reference";

        /// <summary>
        /// Default folder name
        /// </summary>
        public const string DefaultFolderName = "maps";

        private IDictionary<string, Area> areas;
        private IDictionary<string, IRoom> rooms;

        public RoomManager(IGameContext context) : this(DefaultFolderName, context)
        {
        }

        public RoomManager(string folder, IGameContext context)
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

            var files = storage.GetFiles(folder);
            if ((files != null) && (files.Count > 0))
            {
                Parallel.For(
                    0,
                    files.Count,
                    index =>
                    {
                        var path = storage.CombinePath(folder, files[index]);
                        LoadRoomsFromMapFile(storage, path, syncObject, typeManager, activeConfigurations);
                    });
            }

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
                        if (room.Init(phaseId, roomData.Name, roomData.Area, roomData.Data, context, getRoom))
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

        private void LoadRoomsFromMapFile(
            IContentStorage storage, 
            string path, 
            object syncObject, 
            TypeManager typeManager, 
            IList<RoomData> activeConfigurations)
        {
            XmlElement rootElement;
            using (var stream = storage.OpenForRead(path))
            {
                var document = new XmlDocument();
                document.Load(stream);
                rootElement = document.DocumentElement;
            }

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
                    Type type = GetTypeFromXml(typeElement, typeManager);
                    if (type == null)
                    {
                        // TODO: write error. type was not found
                    }
                    else
                    {
                        try
                        {
                            var instance = Activator.CreateInstance(type);
                            room = instance as IRoom;
                            if (room == null)
                            {
                                // TODO: write error. wrong instance
                            }
                        }
                        catch
                        {
                            // TODO: write error. Creating room got exception
                        }
                    }
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
                        activeConfigurations.Add(roomData);
                    }
                }
            }
        }

        private static Type GetTypeFromXml(XmlElement typeElement, TypeManager typeManager)
        {
            Type type = null;
            var referenceName = typeElement.GetAttribute(TypeNameAttributeName);
            if (!string.IsNullOrWhiteSpace(referenceName))
            {
                type = typeManager.GetRegisteredType(referenceName.Trim());
                if (type == null)
                {
                    // TODO: write error. could not find registered type
                }
            }
            else
            {
                var typeName = typeElement.InnerText;
                if (string.IsNullOrEmpty(typeName))
                {
                    // TODO: write error. type name is required
                }
                else
                {
                    type = Type.GetType(typeName.Trim());
                    if (type == null)
                    {
                        // TODO: write type does not exist
                    }
                }
            }

            return type;
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
            internal XmlElement Data { get; set; }
        }
    }
}

