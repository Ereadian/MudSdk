namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Concurrent;
    using System.IO;
    using System.Threading;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public class PlayerManager
    {
        private string userFolder;
        private string profileFolder;

        private ConcurrentDictionary<string, Profile> profiles;

        public PlayerManager(string root)
        {
            this.userFolder = GetFolderName(root, "users");
            this.profileFolder = GetFolderName(this.userFolder, "profile");

            this.profiles = new ConcurrentDictionary<string, Profile>(StringComparer.OrdinalIgnoreCase);
            var files = Directory.GetFiles(this.profileFolder);
            if (files != null)
            {
                for (var i = 0; i < files.Length; i++)
                {
                    var file = files[i];
                    var profileData = Singleton<Serializer<ProfileData>>.Instance.Deserialize(file);
                    var profile = new Profile(Path.GetFileName(file), profileData);
                    if (!this.profiles.TryAdd(profile.Name, profile))
                    {
                        // TODO: show error
                    }
                }
            }
        }

        public Profile GetProfile(string playerName)
        {
            Profile profile;
            return this.profiles.TryGetValue(playerName, out profile) ? profile : null;
        }

        public bool TryAddProfile(Profile profile, Room room)
        {
            if (!this.profiles.TryAdd(profile.Name, profile))
            {
                return false;
            }

            var data = new ProfileData
            {
                Name = profile.Name,
                PasswordHash = profile.PasswordHash,
                AreaName = profile.Room.Area.Name,
                RoomName = profile.Room.Name
            };

            Singleton<Serializer<ProfileData>>.Instance.Serialize(this.profileFolder, data);
            return true;
        }

        private static string GetFolderName(string root, string folder)
        {
            folder = Path.GetFullPath(Path.Combine(root, folder));
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }

            return folder;
        }
    }
}
