//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Program.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Tools.SampleFileGenerator
{
    using System;
    using Ereadian.MudSdk.Sdk;
    using Sdk.Globalization;

    class Program
    {
        static void Main(string[] args)
        {
            WriteSettings();
            WriteResource();
        }

        private static void WriteSettings()
        {
            var gameSettings = new GameSettingsData()
            {
                Locale = "zh-cn",
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
