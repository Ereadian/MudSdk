namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    public class LoginWorld : IWorld
    {
        /// <summary>
        /// Create world runtime for player
        /// </summary>
        /// <returns>world runtime</returns>
        public IWorldRuntime CreateRuntime()
        {
            return new LoginWorldRuntime(this);
        }
    }
}
