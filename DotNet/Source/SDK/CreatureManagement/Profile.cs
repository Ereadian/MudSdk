//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Profile.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using System.IO;
    using System.Globalization;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.WorldManagement;

    public class Profile
    {
        private const string RootElementName = "profile";
        private const string NameElementName = "name";
        private const string PasswordHashElementName = "password";
        private const string WorldElemenName = "world";
        private const string LastActiveElementName = "active";

        public Profile(string name, string passwordHash)
        {
            this.Filename = Guid.NewGuid().ToString("N") + ".xml";
            this.Name = name;
            this.PasswordHash = passwordHash;
        }

        public Profile(string profileFolder, string file, WorldManager worldManager)
        {
            this.Load(profileFolder, file, worldManager);
        }

        public string Filename { get; private set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public IWorld World { get; set; }

        public DateTime LastActive { get; set; }

        public int LocaleId { get; set; }

        public void Save(string profileFolder)
        {
            lock (this)
            {
                var document = new XmlDocument();
                var rootElement = document.CreateElement(RootElementName);
                document.AppendChild(rootElement);
                rootElement.AddTextElement(NameElementName, this.Name);
                rootElement.AddTextElement(PasswordHashElementName, this.PasswordHash);
                rootElement.AddTextElement(WorldElemenName, this.World.Name);
                rootElement.AddTextElement(LastActiveElementName, this.LastActive.ToString(CultureInfo.InvariantCulture));
                var path = Path.Combine(profileFolder, this.Filename);
                document.Save(path);
            }
        }

        private void Load(string profileFolder, string file, WorldManager worldManager)
        {
            lock (this)
            {
                this.Filename = file;
                var path = Path.Combine(profileFolder, file);
                var rootElement = GameUtility.LoadXml(path);
                this.Name = rootElement.GetChildElementText(NameElementName) ?? Path.GetFileNameWithoutExtension(file);
                this.PasswordHash = rootElement.GetChildElementText(PasswordHashElementName);

                var worldName = rootElement.GetChildElementText(WorldElemenName);
                this.World = worldManager.GetWorld(worldName) ?? worldManager.StartWorld;

                var activeData = rootElement.GetChildElementText(LastActiveElementName);
                DateTime lastActive;
                if (!string.IsNullOrEmpty(activeData) && DateTime.TryParse(activeData, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out lastActive))
                {
                    this.LastActive = lastActive;
                }
                else
                {
                    this.LastActive = DateTime.MinValue;
                }
            }
        }
    }
}
