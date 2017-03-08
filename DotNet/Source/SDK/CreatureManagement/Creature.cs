using Ereadian.MudSdk.Sdk.RoomManagement;

namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    public abstract class Creature : ActionableObject
    {
        public Creature(ActionableObjectManager manager) : base(manager)
        {
        }

        public Room Room { get; set; }
    }
}
