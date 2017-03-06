using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    public class LoginWorldRuntime : IWorldRuntime
    {
        public LoginWorldRuntime(IWorld world)
        {
            this.World = world;
        }

        /// <summary>
        /// Gets world
        /// </summary>
        public IWorld World { get; private set; }
    }
}
