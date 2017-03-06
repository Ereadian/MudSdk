namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using System;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public class WorldManager
    {
        private Dictionary<string, IWorld> worlds = new Dictionary<string, IWorld>(StringComparer.OrdinalIgnoreCase);

        public IWorld LoginWorld { get; private set; }

        public void RegisterWorld(string name, IWorld world)
        {
            if (this.LoginWorld == null)
            {
                this.LoginWorld = world;
            }

            if (!this.worlds.ContainsKey(name))
            {
                this.worlds.Add(name, world);
            }
        }
    }
}
