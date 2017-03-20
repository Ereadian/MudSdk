namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System.IO;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using Ereadian.MudSdk.Sdk.IO;

    public class GeneralWorldRuntime : WorldRuntime
    {
        public const string RootElementName = "runtime";
        public const string RoomElementName = "room";

        public GeneralWorldRuntime(GeneralWorld world) : base(world)
        {
        }

        public WorldStatus Status { get; set; }

        public IRoom Room { get; set; }

        private GeneralWorld CurrentWorld
        {
            get
            {
                return this.World as GeneralWorld;
            }
        }

        public override void Init(Player player)
        {
            base.Init(player);
            var runtimeFile = GetFilePath(player, this.CurrentWorld);
            bool fileExists = File.Exists(runtimeFile);
            if (this.World.GameConext.ContentStorage.IsFileExist(runtimeFile))
            {
                this.Load(player);
            }
            else
            {
                this.Room = this.World.EntryRoom;
                this.Save(player);
            }

            this.Status = WorldStatus.Init;
        }

        public override void Load(Player player)
        {
            base.Load(player);
            var runtimeFile = GetFilePath(player, this.CurrentWorld);
            var runtimeXml = this.World.GameConext.ContentStorage.LoadXml(runtimeFile);
            this.Descrialize(player, runtimeXml);
        }

        public override void Descrialize(Player player, XmlElement runtimeXml)
        {
            base.Descrialize(player, runtimeXml);
            var roomName = runtimeXml.GetChildElementText(RoomElementName);
            this.Room = player.GameContext.RoomManager.FindRoom(roomName);
            if (this.Room == null)
            {
                this.Room = this.World.EntryRoom;
            }
        }

        public override void Save(Player player)
        {
            var runtimeFile = GetFilePath(player, this.CurrentWorld);
            var document = new XmlDocument();
            var rootElement = document.CreateElement(RootElementName);
            this.Serialize(player, rootElement);
            using (var stream = this.World.GameConext.ContentStorage.OpenForWrite(runtimeFile))
            {
                document.Save(stream);
            }
        }

        public override void Serialize(Player player, XmlElement runtimeXml)
        {
            base.Serialize(player, runtimeXml);
        }

        private static string GetFilePath(Player player, GeneralWorld world)
        {
            return world.GameConext.ContentStorage.CombinePath(world.RuntimeDataFolder, GameUtility.GetFilenameById(player.Profile.Id));
        }
    }
}
