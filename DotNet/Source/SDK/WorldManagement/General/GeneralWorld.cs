namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.IO;

    public class GeneralWorld : World
    {
        public override void Run(Player player)
        {
        }

        public override void Init(string name, Game game)
        {
            base.Init(name, game);
            var configurationFile = Path.Combine(game.Settings.GameFolder, "worlds", name + ".xml");
            if (!File.Exists(configurationFile))
            {
                // TODO: write error
                return;
            }

            var data = Singleton<Serializer<GeneralWorldData>>.Instance.Deserialize(configurationFile);
            if (!string.IsNullOrEmpty(data.EntryRoomName))
            {
                this.EntryRoom = game.RoomManager.FindRoom(data.EntryRoomName);
            }

            if (!string.IsNullOrEmpty(data.EntryRoomName))
            {
                this.RespawnRoom = game.RoomManager.FindRoom(data.RespawnRoomName);
            }
        }

        protected override IWorldRuntime CreateRuntime()
        {
            return new GeneralWorldRuntime();
        }
    }
}
