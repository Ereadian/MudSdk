//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameSettings.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.IO;

    /// <summary>
    /// Game settings data
    /// </summary>
    public class GameSettings
    {
        public const string DefaultSettingsFilename = "game.xml";
        public const string LocaleElementName = "locale";
        public const string HeartBeatElementName = "tick";
        public const string LineSpaceElementName = "lines";
        public const string LoginWorldElementName = "login";
        public const string StartWorldElementName = "start";
        public const string WorldDefinitionXmlPath = "worlds/world";
        public const string WorldNameAttributeName = "name";
        public const string WorldTypeNamAttributeName = "type";

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettingsData" /> class.
        /// </summary>
        public GameSettings(IContentStorage storage, string filename = DefaultSettingsFilename)
        {
            if (!storage.IsFileExist(filename))
            {
                // TODO: Write error. Settings file does not exist
                return;
            }

            var rootElement = storage.LoadXml(filename);

            this.DefaultLocale = rootElement.GetChildElementText(LocaleElementName);
            this.HeartBeat = rootElement.GetChildElementValue<int>(HeartBeatElementName, 500);
            this.LineSpace = Math.Max(1, rootElement.GetChildElementValue<int>(LineSpaceElementName, 1));
            this.LoginWorldName = rootElement.GetChildElementText(LoginWorldElementName);
            this.StartWorldName = rootElement.GetChildElementText(StartWorldElementName);

            var worldTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            foreach (XmlElement worldElement in rootElement.SelectNodes(WorldDefinitionXmlPath))
            {
                var worldName = worldElement.GetAttribute(WorldNameAttributeName);
                if (string.IsNullOrWhiteSpace(worldName))
                {
                    // TODO: write error. world name is required
                    continue;
                }

                worldName = worldName.Trim();
                Type worldType;
                if (worldTypes.TryGetValue(worldName, out worldType))
                {
                    // TODO: write error, duplicated world defined
                    continue;
                }

                var worldTypeName = worldElement.InnerText;
                if (string.IsNullOrWhiteSpace(worldTypeName))
                {
                    // TODO: write error. world type name is required
                    continue;
                }

                worldType = Type.GetType(worldTypeName.Trim());
                if (worldType == null)
                {
                    // TODO: write error. world type does not exist
                    continue;
                }

                worldTypes.Add(worldName, worldType);
                this.WorldTypes = worldTypes;
            }
        }

        /// <summary>
        /// Gets or sets default locale name
        /// </summary>
        public string DefaultLocale { get; private set; }

        /// <summary>
        /// Gets or sets game heartbeat
        /// </summary>
        public int HeartBeat { get; private set; }

        /// <summary>
        /// Gets or sets line spaces between paragraph
        /// </summary>
        public int LineSpace { get; private set; }

        public string LoginWorldName { get; private set; }

        public string StartWorldName { get; private set; }

        public IReadOnlyDictionary<string, Type> WorldTypes { get; private set; }
    }
}
