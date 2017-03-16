//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Profile.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using Ereadian.MudSdk.Sdk.WorldManagement;

    public class Profile
    {
        public Profile(Guid id, string name, string passwordHash)
        {
            this.Id = id;
            this.Name = name;
            this.PasswordHash = passwordHash;
        }

        public Guid Id { get; private set; }

        public string Name { get; set; }

        public string PasswordHash { get; set; }

        public string WorldName { get; set; }

        public DateTime LastActive { get; set; }

        public int LocaleId { get; set; }
    }
}
