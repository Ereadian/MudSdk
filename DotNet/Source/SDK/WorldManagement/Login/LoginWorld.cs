namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.Globalization;

    public class LoginWorld : IWorld
    {
        public bool IsLogingWorld
        {
            get
            {
                return true;
            }
        }

        public void Add(Player player)
        {
            if (player.World != null)
            {
                if (object.ReferenceEquals(this, player.World))
                {
                    return;
                }

                player.World.Remove(player);
            }

            player.World = this;
            player.WorldRuntime = new LoginWorldRuntime();
        }

        public void Remove(Player player)
        {
            player.WorldRuntime = null;
        }

        public void Run(Player player)
        {
            var runtime = player.WorldRuntime as LoginWorldRuntime;
            switch (runtime.Status)
            {
                case LoginStatus.Enter:
                    player.AddOuput(ContentUtility.CreateMessage(GameTitle.Title));
                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterUserName));
                    runtime.Status = LoginStatus.UserName;
                    break;
                case LoginStatus.UserName:
                    break;
                case LoginStatus.Password:
                    break;
            }
        }
    }
}
