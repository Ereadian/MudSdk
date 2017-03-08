namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System;
    using System.IO;
    using System.Threading;
    using ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public class GeneralWorld : World
    {
        public override void Run(Player player)
        {
            var runtime = this.GetRuntime<GeneralWorldRuntime>(player.WorldRuntime);
            if (runtime == null)
            {
                return;
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
        }

        public override void Add(Player player)
        {
            player.AddOuput(ContentUtility.CreateMessage(SystemResources.LoadingUserWorldData));
            ThreadPool.QueueUserWorkItem(AddPlayerTask, Tuple.Create(this, player));
        }

        public override void Remove(Player player)
        {
            base.Remove(player);
        }

        protected override IWorldRuntime CreateRuntime()
        {
            return new GeneralWorldRuntime(this);
        }

        private static void AddPlayerTask(object state)
        {
            var data = state as Tuple<GeneralWorld, Player>;
            data.Item1.AddPlayer(data.Item2);
        }

        private void AddPlayer(Player player)
        {
            player.Profile.World = this;
            player.Profile.LastActive = DateTime.UtcNow;
            player.CurrentGame.PlayerManager.SaveProfile(player.Profile);
            base.Add(player);
        }
    }
}
