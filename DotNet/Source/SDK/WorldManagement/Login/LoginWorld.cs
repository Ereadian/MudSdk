namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using System;
    using System.Security.Cryptography;
    using System.Text;

    public class LoginWorld : IWorld
    {
        private static readonly MD5 md5 = MD5.Create();

        public string Name { get; private set; }

        public Room EntryRoom { get; }
        public Room RespawnRoom { get; }

        public void Init(string name, Game game)
        {
            this.Name = name;
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
                    var name = player.GetInput();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        return;
                    }

                    name = name.Trim();
                    if (!VerifyInput(name))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName));
                        return;
                    }

                    runtime.UserName = name;
                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterPassword));
                    runtime.Status = LoginStatus.Password;
                    break;
                case LoginStatus.Password:
                    var password = player.GetInput();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return;
                    }

                    password = password.Trim();
                    if (!VerifyInput(password))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName));
                        return;
                    }

                    runtime.PasswordHash = GetHash(password);
                    runtime.Status = LoginStatus.Password;
                    break;
            }
        }

        private static bool VerifyInput(string input)
        {
            for (var i = 0; i < input.Length; i++)
            {
                var c = input[i];
                if (!char.IsLetterOrDigit(c) && !(c == '.'))
                {
                    return false;
                }
            }

            return true;
        }

        private static string GetHash(string input)
        {

            byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));

            var sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }

        private static bool VerifyHash(string input, string hash)
        {
            var hashOfInput = GetHash(input);

            // Create a StringComparer an compare the hashes.
            var comparer = StringComparer.OrdinalIgnoreCase;

            return 0 == comparer.Compare(hashOfInput, hash);
        }
    }
}
