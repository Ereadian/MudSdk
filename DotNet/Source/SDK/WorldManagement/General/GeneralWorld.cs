namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System;
    using System.IO;
    using System.Threading;
    using ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public class GeneralWorld : World
    {
        public  string RuntimeDataFolder { get; set; }

        public override void Run(Player player)
        {
            var runtime = this.GetRuntime<GeneralWorldRuntime>(player.WorldRuntime);
            if (runtime == null)
            {
                return;
            }

            switch (runtime.Status)
            {
                case WorldStatus.Enter:
                    runtime.Room.ShowRoom(player);
                    runtime.Status = WorldStatus.Run;
                    break;
            }
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

            if (!string.IsNullOrEmpty(data.RespawnRoomName))
            {
                this.RespawnRoom = game.RoomManager.FindRoom(data.RespawnRoomName);
            }

            this.RuntimeDataFolder = Path.GetFullPath(Path.Combine(
                game.Settings.GameFolder, 
                Constants.UserFolderName, 
                Constants.UserWorldRuntimeFolderName, 
                name));
            if (!Directory.Exists(this.RuntimeDataFolder))
            {
                Directory.CreateDirectory(this.RuntimeDataFolder);
            }
        }

        public override void Add(Player player)
        {
            player.AddOuput(ContentUtility.CreateMessage(SystemResources.LoadingUserWorldData, player.Profile.LocaleId));
            ThreadPool.QueueUserWorkItem(AddPlayerTask, Tuple.Create(this, player));
        }

        public override void Remove(Player player)
        {
            var runtime = this.GetRuntime(player);
            runtime.Save(player);
            base.Remove(player);
        }

        protected override IWorldRuntime CreateRuntime(Player player)
        {
            return new GeneralWorldRuntime(player, this);
        }

        private GeneralWorldRuntime GetRuntime(Player player)
        {
            return this.GetRuntime<GeneralWorldRuntime>(player.WorldRuntime); ;
        }

        private static void AddPlayerTask(object state)
        {
            var data = state as Tuple<GeneralWorld, Player>;
            data.Item1.AddPlayer(data.Item2);
        }

        private void AddPlayer(Player player)
        {
            base.Add(player);

            var runtime = GetRuntime(player);
            var profile = player.Profile;
            player.Profile.LastActive = DateTime.UtcNow;
            player.CurrentGame.PlayerManager.SaveProfile(player);


            if (runtime.Room == null)
            {
                runtime.Room = this.EntryRoom;
            }

            runtime.Status = WorldStatus.Enter;
        }
    }
}
