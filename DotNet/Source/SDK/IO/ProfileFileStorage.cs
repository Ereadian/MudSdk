//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ProfileFileStorage.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.IO
{
    using System;
    using System.Collections.Concurrent;
    using System.Globalization;
    using System.Threading.Tasks;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public class ProfileFileStorage : IProfileStorage
    {
        private const string RootElementName = "profile";
        private const string NameElementName = "name";
        private const string PasswordHashElementName = "password";
        private const string WorldElemenName = "world";
        private const string LastActiveElementName = "active";
        private const string DefaultProfileFolder = "users/profile";

        private ConcurrentDictionary<string, Guid> NameIdMapping;
        private readonly IContentStorage storage;
        private readonly string profileFolder;

        public ProfileFileStorage(IContentStorage storage) : this(storage, DefaultProfileFolder)
        {
        }

        public ProfileFileStorage(IContentStorage storage, string folder)
        {
            this.storage = storage;
            this.profileFolder = folder;
            var mappping = new ConcurrentDictionary<string, Guid>(StringComparer.OrdinalIgnoreCase);
            this.NameIdMapping = mappping;
            var files = storage.GetFiles(profileFolder);
            if ((files != null) && (files.Count > 0))
            {
                Parallel.For(
                    0, 
                    files.Count,
                    index =>
                    {
                        var file = files[index];
                        var profile = Load(storage, folder, file);
                        if (!mappping.TryAdd(profile.Name, profile.Id))
                        {
                            // TODO: log error
                        }
                    });
            }
        }

        public bool TryRegisterProfile(string name, Guid id)
        {
            return this.NameIdMapping.TryAdd(name, id);
        }

        public Profile Load(string name)
        {
            Guid id;
            if (!this.NameIdMapping.TryGetValue(name, out id))
            {
                return null;
            }

            return Load(this.storage, this.profileFolder, id, GameUtility.GetFilenameById(id));
        }

        public void Save(Profile profile)
        {
            var document = new XmlDocument();
            var rootElement = document.CreateElement(RootElementName);
            document.AppendChild(rootElement);
            rootElement.AddTextElement(NameElementName, profile.Name);
            rootElement.AddTextElement(PasswordHashElementName, profile.PasswordHash);
            rootElement.AddTextElement(WorldElemenName, profile.WorldName);
            rootElement.AddTextElement(LastActiveElementName, profile.LastActive.ToString(CultureInfo.InvariantCulture));

            using (var stream = this.storage.OpenForWrite(GameUtility.GetFilenameById(profile.Id)))
            {
                document.Save(stream);
            }
        }

        private static Profile Load(IContentStorage storage, string profileFolder, string file)
        {
            var id = Guid.Parse(file.Substring(0, file.LastIndexOf('.')));
            return Load(storage, profileFolder, id, file);
        }

        private static Profile Load(IContentStorage storage, string profileFolder, Guid id, string file)
        {
            Profile profile = null;
            var path = storage.CombinePath(profileFolder, file);
            if (storage.IsFileExist(path))
            {
                var document = new XmlDocument();
                using (var stream = storage.OpenForRead(path))
                {
                    document.Load(stream);
                }

                var rootElement = document.DocumentElement;
                var name = rootElement.GetChildElementText(NameElementName) ?? file;
                var passwordHash = rootElement.GetChildElementText(PasswordHashElementName);
                profile = new Profile(id, name, passwordHash);
                profile.WorldName = rootElement.GetChildElementText(WorldElemenName);

                var activeData = rootElement.GetChildElementText(LastActiveElementName);
                DateTime lastActive;
                if (!string.IsNullOrEmpty(activeData) 
                    && DateTime.TryParse(activeData, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out lastActive))
                {
                    profile.LastActive = lastActive;
                }
                else
                {
                    profile.LastActive = DateTime.MinValue;
                }
            }

            return profile;
        }
    }
}
