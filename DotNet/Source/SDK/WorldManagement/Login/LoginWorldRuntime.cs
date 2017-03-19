using Ereadian.MudSdk.Sdk.CreatureManagement;

namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    public class LoginWorldRuntime : WorldRuntime
    {
        public LoginWorldRuntime(LoginWorld world) : base(world)
        {
        }

        public override void Init(Player player)
        {
            this.Status = LoginStatus.Enter;
        }

        public LoginStatus Status { get; set; }

        public int LocaleId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
