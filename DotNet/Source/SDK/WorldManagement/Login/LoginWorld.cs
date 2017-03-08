﻿namespace Ereadian.MudSdk.Sdk.WorldManagement.Login
{
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using Ereadian.MudSdk.Sdk.RoomManagement;
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading;

    public class LoginWorld : World
    {
        private static readonly MD5 md5 = MD5.Create();


        public override void Run(Player player)
        {
            var runtime = this.GetRuntime<LoginWorldRuntime>(player.WorldRuntime);
            if (runtime == null)
            {
                return;
            }

            string userName = null;
            string password = null;

            switch (runtime.Status)
            {
                case LoginStatus.Enter:
                    player.AddOuput(ContentUtility.CreateMessage(GameTitle.Title));
                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterUserName));
                    runtime.Status = LoginStatus.EnterUserName;
                    break;
                case LoginStatus.EnterUserName:
                    userName = player.GetInput();
                    if (string.IsNullOrWhiteSpace(userName))
                    {
                        return;
                    }

                    userName = userName.Trim();
                    if (!VerifyInput(userName))
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.InvalidName));
                        return;
                    }

                    runtime.UserName = userName;
                    runtime.UserProfile = player.CurrentGame.PlayerManager.GetProfile(userName);
                    if (runtime.UserProfile == null)
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.NewUser));
                        runtime.Status = LoginStatus.CreateProfile;
                    }
                    else
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.EnterPassword));
                        runtime.Status = LoginStatus.VerifyPassword;
                    }

                    break;
                case LoginStatus.VerifyPassword:
                    password = player.GetInput();
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

                    runtime.Password = password;
                    player.Profile = runtime.UserProfile;
                    runtime.Status = LoginStatus.EnterWorld;
                    break;
                case LoginStatus.CreateProfile:
                    password = player.GetInput();
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

                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.ConfirmPassword));
                    runtime.Password = password;
                    runtime.Status = LoginStatus.ConfirmPassword;
                    break;

                case LoginStatus.ConfirmPassword:
                    password = player.GetInput();
                    if (string.IsNullOrWhiteSpace(password))
                    {
                        return;
                    }

                    if (password != runtime.Password)
                    {
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.PasswordNotMatch));
                        player.AddOuput(ContentUtility.CreateMessage(SystemResources.NewUser));
                        runtime.Status = LoginStatus.CreateProfile;
                        return;
                    }

                    player.AddOuput(ContentUtility.CreateMessage(SystemResources.CreatingAccount));
                    player.Profile = new Profile(runtime.UserName, GetHash(runtime.Password));
                    player.Profile.World = player.CurrentGame.WorldManager.StartWorld;
                    runtime.Status = LoginStatus.EnterWorld;
                    break;
                case LoginStatus.EnterWorld:
                    runtime.Status = LoginStatus.Transferring;
                    player.Profile.World.Add(player);
                    break;
            }
        }

        protected override IWorldRuntime CreateRuntime()
        {
            return new LoginWorldRuntime(this);
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
