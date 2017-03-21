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
    using Ereadian.MudSdk.Sdk.Diagnostics;
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
        public const string FolderRootElementName = "folders";
        public const string FolderElementName = "folder";
        public const string FolderNameAttributeName = "name";
        public const string WorldRootElementName = "worlds";
        public const string WorldElementName = "world";
        public const string WorldNameAttributeName = "name";
        public const string WorldTypeNamAttributeName = "type";

        public const string DefaultPlayerProfileFolder = "users/profile";
        public const string DefaultPlayerDataFolder = "users/data";
        public const string DefaultMapDataFolder = "maps";
        public const string DefaultMapDesignFolder = "design";
        public const string DefaultWorldDataFolder = "worlds";

        /// <summary>
        /// Initializes a new instance of the <see cref="GameSettingsData" /> class.
        /// </summary>
        public GameSettings(IContentStorage storage, ILog log, string filename = DefaultSettingsFilename)
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

            this.PlayerProfileFolder = DefaultPlayerProfileFolder ;
            this.PlayerDataFolder = DefaultPlayerDataFolder ;
            this.MapDataFolder = DefaultMapDataFolder;
            this.MapDesignFolder = DefaultMapDesignFolder;
            this.WorldDataFolder = DefaultWorldDataFolder;
            this.SetFolders(rootElement.SelectSingleNode(FolderRootElementName) as XmlElement, log);
            this.LoadWorldConfigurations(rootElement.SelectSingleNode(WorldRootElementName) as XmlElement, log);
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

        public string PlayerProfileFolder { get; private set; }
        public string PlayerDataFolder { get; private set; }
        public string MapDataFolder { get; private set; }
        public string MapDesignFolder { get; private set; }
        public string WorldDataFolder { get; private set; }

        public IReadOnlyDictionary<string, Type> WorldTypes { get; private set; }

        private void LoadWorldConfigurations(XmlElement worldRootElement, ILog log)
        {
            var worldTypes = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            if (WorldRootElementName != null)
            {
                foreach (XmlElement worldElement in worldRootElement.SelectNodes(WorldElementName))
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
                }
            }

            this.WorldTypes = worldTypes;
        }

        private void SetFolders(XmlElement foldersElement, ILog log)
        {
            if (foldersElement != null)
            {
                foreach (XmlElement folderElement in foldersElement.SelectNodes(FolderElementName))
                {
                    var name = folderElement.GetAttribute(FolderNameAttributeName);
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        // TODO: write error. Name is required
                        continue;
                    }

                    var folder = folderElement.InnerText;
                    if (string.IsNullOrWhiteSpace(folder))
                    {
                        continue;
                    }

                    folder = folder.Trim();
                    switch (name)
                    {
                        case "profile":
                            this.PlayerProfileFolder = folder;
                            break;
                        case "data":
                            this.PlayerDataFolder = folder;
                            break;
                        case "map":
                            this.MapDataFolder = folder;
                            break;
                        case "world":
                            this.WorldDataFolder = folder;
                            break;
                        case "design":
                            this.MapDesignFolder = folder;
                            break;
                        default:
                            // TODO: log error: name is not supported
                            break;
                    }
                }
            }
        }
    }
}
