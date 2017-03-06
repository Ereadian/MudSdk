namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public class Profile
    {
        public Profile(string name, string passwordHash, Room room)
        {
            this.File = Guid.NewGuid().ToString("N") + ".xml";
            this.Name = name;
            this.PasswordHash = passwordHash;
            this.Room = room;
        }

        public Profile(string file, ProfileData data)
        {
            this.File = file;
            this.Name = data.Name;
            this.PasswordHash = data.PasswordHash;
        }

        public string File { get; private set; }
        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public Room Room { get; set; }

    }
}
