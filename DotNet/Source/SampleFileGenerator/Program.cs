//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Program.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Tools.SampleFileGenerator
{
    using System;
    using Ereadian.MudSdk.Sdk;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using Ereadian.MudSdk.Sdk.Globalization;
    using Ereadian.MudSdk.Sdk.WorldManagement;
    using Ereadian.MudSdk.Sdk.WorldManagement.General;

    class Program
    {
        static void Main(string[] args)
        {
            WriteSettings();
            WriteResource();
            WriteArea();
            WriteWorld();
        }

        private static void WriteWorld()
        {
            var data = new GeneralWorldData
            {
                EntryRoomName = "newbie:backyard",
                RespawnRoomName = "newbie:backyard",
            };

            Write("world", data);
        }

        private static void WriteArea()
        {
            var area = new AreaData
            {
                Name = "town",
                Rooms = new RoomData[]
                {
                    new RoomData
                    {
                        Name = "backyard",
                        Title = new ContentData[]
                        {
                            new ContentData
                            {
                                Locale = "en-us",
                                Data = "this is backyard"
                            },
                            new ContentData
                            {
                                Locale="zh-cn",
                                Data = "这是后院"
                            },
                        },
                        Description = new ContentData[]
                        {
                            new ContentData
                            {
                                Locale = "en-us",
                                Data = "lots of flowers"
                            },
                            new ContentData
                            {
                                Locale="zh-cn",
                                Data = "好多鲜花盛开"
                            },
                        }
                    },
                }
            };

            Write("area", area);
        }

        private static void WriteSettings()
        {
            var gameSettings = new GameSettingsData()
            {
                Locale = "zh-cn",
                HeartBeat = 100,
                LoginWorldName = "login",
                StartWorldName = "start",
                Worlds = new WorldTypeData[]
                {
                    new WorldTypeData()
                    {
                        WorldName = "login",
                        TypeName = "my_type"
                    },
                    new WorldTypeData()
                    {
                        WorldName = "normal",
                        TypeName = "normal type"
                    },
                },
            };
            Write("game", gameSettings);
        }

        private static void WriteResource()
        {
            var collection = new ResourceCollectionData
            {
                CollectionName = "collection.name",
                Resources = new ResourceData[]
                {
                    new ResourceData
                    {
                        Name = "name",
                        Resources = new ContentData[]
                        {
                            new ContentData
                            {
                                Data = "resource default"
                            },
                            new ContentData
                            {
                                Locale="zh-cn",
                                Data = "中文"
                            },
                        }
                    },
                    new ResourceData
                    {
                        Name = "name2",
                        Resources = new ContentData[]
                        {
                            new ContentData
                            {
                                Data = "another resource"
                            },
                            new ContentData
                            {
                                Locale="zh-cn",
                                Data = "中文2"
                            },
                        }
                    }
                }
            };


            Write("resources", collection);
        }

        private static void Write<T>(string name, T data)
        {
            var serializer = new Serializer<T>();
            serializer.Serialize(name + ".xml", data);
        }
    }
}
