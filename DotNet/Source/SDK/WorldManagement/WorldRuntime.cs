using Ereadian.MudSdk.Sdk.CreatureManagement;

namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using System.Xml;

    public class WorldRuntime : IWorldRuntime
    {
        public WorldRuntime(IWorld world)
        {
            this.World = world;
        }

        public IWorld World { get; private set; }

        public virtual void Init(Player player)
        {
        }

        public virtual void Load(Player player)
        {
        }

        public virtual void Save(Player player)
        {
        }

        public virtual void Descrialize(Player player, XmlElement runtimeXml)
        {
        }

        public virtual void Serialize(Player player, XmlElement runtimeXml)
        {
        }
    }
}
