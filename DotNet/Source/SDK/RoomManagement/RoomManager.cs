//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Xml;
    using System.Threading.Tasks;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.IO;

    public class RoomManager
	{
        /// <summary>
        /// Area separator character
        /// </summary>
        public const char AreaSeparatorChar = '.';

        public const string AreaNameAttributeName = "name";
        public const string RoomElementName = "room";
        public const string RoomNameAttributeName = "name";
        public const string TypeElementName = "type";
        public const string TypeNameAttributeName = "reference";
        public const string TitleElementName = "title";
        public const string DescriptionElementName = "description";

        private static readonly Type[] RoomConstructorParameterTypes = new Type[]
        {
            typeof(string),
            typeof(Area),
            typeof(IReadOnlyList<Text>),
            typeof(IReadOnlyList<Text>)
        };

        private IDictionary<string, Area> areas;
        private IDictionary<string, Room> rooms;

        public RoomManager(string folder, IGameContext context)
        {
            var storage = context.ContentStorage;
            var typeManager = context.TypeManager;
            var localeManager = context.LocaleManager;
            var colorManager = context.ColorManager;

            var syncObject = new object();
            this.areas = new Dictionary<string, Area>(StringComparer.OrdinalIgnoreCase);
            this.rooms = new Dictionary<string, Room>(StringComparer.OrdinalIgnoreCase);
            var roomConfiguraions = new Dictionary<string, XmlElement>(StringComparer.OrdinalIgnoreCase);

            var files = storage.GetFiles(folder);
            if ((files != null) && (files.Count > 0))
            {
                Parallel.For(
                    0,
                    files.Count,
                    index =>
                    {
                        var path = storage.CombinePath(folder, files[index]);
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

                        var constructorParameters = new object[4];
                        foreach (XmlElement roomElement in rootElement.SelectNodes(RoomElementName))
                        {
                            string roomName = roomElement.GetAttribute(RoomNameAttributeName);
                            if (string.IsNullOrWhiteSpace(roomName))
                            {
                                // TODO: write error. room name is required
                                continue;
                            }

                            roomName = roomName.Trim();

                            Type type = null;
                            Resource title = default(Resource);
                            Resource description = default(Resource);
                            foreach (XmlNode node in rootElement.ChildNodes)
                            {
                                if (node.NodeType == XmlNodeType.Element)
                                {
                                    var childElement = node as XmlElement;
                                    switch (childElement.Name)
                                    {
                                        case TypeElementName:
                                            type = GetTypeFromXml(childElement, typeManager);
                                            break;
                                        case TitleElementName:
                                            title = LoadContentFromXml(childElement, localeManager, colorManager);
                                            break;
                                        case DescriptionElementName:
                                            description = LoadContentFromXml(childElement, localeManager, colorManager);
                                            break;
                                    }
                                }
                            }

                            Room room = null;
                            if (type != null)
                            {
                                var constructor = type.GetConstructor(RoomConstructorParameterTypes);
                                if (constructor == null)
                                {
                                    // TODO: could not find Room constructor from that type
                                    continue;
                                }

                                try
                                {
                                    constructorParameters[0] = roomName;
                                    constructorParameters[1] = area;
                                    constructorParameters[2] = title;
                                    constructorParameters[3] = description;
                                    var instance = constructor.Invoke(constructorParameters);
                                    if (instance is Room)
                                    {
                                        room = instance as Room;
                                    }
                                    else
                                    {
                                        // TODO: write type mismatch
                                    }
                                }
                                catch
                                {
                                    // TODO: room was not found
                                }
                            }

                            if (room == null)
                            {
                                room = new Room(roomName, area, title, description);
                            }

                            var fullName = GetRoomFullName(areaName, roomName);
                            lock (syncObject)
                            {
                                if (area.Rooms.ContainsKey(roomName))
                                {
                                    // TODO: write error
                                }
                                else
                                {
                                    area.Rooms.Add(roomName, room);
                                }

                                rooms.Add(fullName, room);
                                roomConfiguraions.Add(fullName, roomElement);
                            }
                        }
                    });

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

        public Room FindRoom(string fullName)
        {
            Room room;
            return this.rooms.TryGetValue(fullName, out room) ? room : null;
        }

        public Room FindRoom(string areaName, string roomName)
        {
            return FindRoom(GetRoomFullName(areaName, roomName));
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
            foreach (XmlElement contentElement in contentsElement.SelectNodes(
                ResourceCollection.ContentElementName))
            {
                var content = new Text(contentElement, localeManager, colorManager);
                list.Add(content);
            }

            return new Resource(list.ToArray());
        }
    }
}

