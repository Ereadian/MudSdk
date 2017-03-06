namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class GeneralWorld : World
    {
        public override void Run(Player player)
        {
        }

        protected override IWorldRuntime CreateRuntime()
        {
            return new GeneralWorldRuntime();
        }
    }
}
