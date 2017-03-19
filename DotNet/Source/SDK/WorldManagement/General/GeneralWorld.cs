namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System;
    using System.IO;
    using System.Threading;
    using ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.IO;

    public class GeneralWorld : World
    {
        public const string EntryRoomNameElementName = "entry";
        public const string RespawnRoomNameElementName = "respawn";

        public string RuntimeDataFolder { get; private set; }

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

        public override void Init(string name, IGameContext context)
        {
            base.Init(name, context);
            var contentStorage = context.ContentStorage;
            var configurationFile = contentStorage.CombinePath("worlds", name + ".xml");
            if (!File.Exists(configurationFile))
            {
                // TODO: write error
                return;
            }

            var worldXml = contentStorage.LoadXml(configurationFile);
            var roomName = worldXml.GetChildElementText(EntryRoomNameElementName);
            if (!string.IsNullOrEmpty(roomName))
            {
                this.EntryRoom = context.RoomManager.FindRoom(roomName);
            }

            roomName = worldXml.GetChildElementText(RespawnRoomNameElementName);
            if (!string.IsNullOrEmpty(roomName))
            {
                this.RespawnRoom = context.RoomManager.FindRoom(roomName);
            }

            this.RuntimeDataFolder = contentStorage.CombinePath(
                Constants.UserFolderName, 
                Constants.UserWorldRuntimeFolderName, 
                name);
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
            var runtime = new GeneralWorldRuntime(this);
            runtime.Init(player);
            return runtime;
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
            player.CurrentGame.Context.ProfileStorage.Save(player.Profile);


            if (runtime.Room == null)
            {
                runtime.Room = this.EntryRoom;
            }

            runtime.Status = WorldStatus.Enter;
        }
    }
}
