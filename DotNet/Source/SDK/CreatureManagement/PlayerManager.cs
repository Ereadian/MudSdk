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
        private string profileFolder;
        private string userFolder;

        private ConcurrentDictionary<string, Profile> profiles;

        public PlayerManager(string root, Game game)
        {
            this.userFolder = GetFolderName(root, Constants.UserFolderName);
            this.profileFolder = GetFolderName(this.userFolder, Constants.ProfileFolderName);

            this.profiles = new ConcurrentDictionary<string, Profile>(StringComparer.OrdinalIgnoreCase);
            var files = Directory.GetFiles(this.profileFolder);
            if (files != null)
            {
                for (var i = 0; i < files.Length; i++)
                {
                    var file = files[i];
                    var profileData = Singleton<Serializer<ProfileData>>.Instance.Deserialize(file);
                    var profile = new Profile(Path.GetFileName(file), profileData, game.WorldManager);
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
            };

            Singleton<Serializer<ProfileData>>.Instance.Serialize(this.profileFolder, data);
            return true;
        }

        public void SaveProfile(Player player)
        {
            var profile = player.Profile;
            var data = new ProfileData
            {
                Name = profile.Name,
                PasswordHash = profile.PasswordHash,
                LocaleName = player.CurrentGame.Locales.GetCulture(profile.LocaleId).Name,
            };

            Singleton<Serializer<ProfileData>>.Instance.Serialize(Path.Combine(this.profileFolder, profile.File), data);
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
