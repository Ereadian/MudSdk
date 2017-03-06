namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    public interface IWorld
    {
        /// <summary>
        /// Create world runtime for player
        /// </summary>
        /// <returns>world runtime</returns>
        IWorldRuntime CreateRuntime();
    }
}
