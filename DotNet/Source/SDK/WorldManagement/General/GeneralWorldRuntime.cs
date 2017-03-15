namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System.IO;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public class GeneralWorldRuntime : WorldRuntime
    {
        public GeneralWorldRuntime(Player player, GeneralWorld world) : base(world)
        {
            var runtimeFile = GetFilePath(player, world);
            bool fileExists = File.Exists(runtimeFile);
            if (fileExists)
            {
                var data = Singleton<Serializer<GeneralWorldRuntimeData>>.Instance.Deserialize(runtimeFile);
                this.Room = player.CurrentGame.RoomManager.FindRoom(data.RoomFullName);
                if (this.Room == null)
                {
                    this.Room = world.EntryRoom;
                }
            }
            else
            {
                this.Room = world.EntryRoom;
                this.Save(player);
            }

            this.Status = WorldStatus.Init;
        }

        public WorldStatus Status { get; set; }

        public Room Room { get; set; }

        public void Save(Player player)
        {
            var runtimeFile = GetFilePath(player, this.World as GeneralWorld);
            var data = new GeneralWorldRuntimeData(this);
            Singleton<Serializer<GeneralWorldRuntimeData>>.Instance.Serialize(runtimeFile, data);
        }

        private static string GetFilePath(Player player, GeneralWorld world)
        {
            return Path.Combine(world.RuntimeDataFolder, player.Profile.Filename);
        }
    }
}
