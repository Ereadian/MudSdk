namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using WorldManagement;

    public class Profile
    {
        public Profile(string name, string passwordHash)
        {
            this.File = Guid.NewGuid().ToString("N") + ".xml";
            this.Name = name;
            this.PasswordHash = passwordHash;
        }

        public Profile(string file, ProfileData data, WorldManager worldManager)
        {
            this.File = file;
            this.Name = data.Name;
            this.PasswordHash = data.PasswordHash;
            this.World = worldManager.GetWorld(data.WorldName) ?? worldManager.StartWorld;
        }

        public string File { get; private set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public IWorld World { get; set; }

        public DateTime LastActive { get; set; }
    }
}
