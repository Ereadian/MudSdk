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
            var gameSettings = new GameSettingsData()
            {
                Locale = "zh-cn",
            };
            WriteSample("game", gameSettings);

            var resources = new ResourceData
            {
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
            };

            WriteSample("resources", resources);
        }

        private static void WriteSample<T>(string name, T data)
        {
            var serializer = new Serializer<T>();
            serializer.Serialize(name + ".xml", data);
        }
    }
}
