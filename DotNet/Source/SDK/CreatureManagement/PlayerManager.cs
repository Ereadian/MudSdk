//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="PlayerManager.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

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
                    var profile = new Profile(Path.GetFileName(file), this.profileFolder, game.WorldManager);
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

            profile.Save(this.profileFolder);
            return true;
        }

        public void SaveProfile(Player player)
        {
            player.Profile.Save(this.profileFolder);
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
