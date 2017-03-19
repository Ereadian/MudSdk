//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement; 

    public class Room
	{
        public Room(string name, Area area, Resource title, Resource description)
        {
            this.Name = name;
            this.Area = area;
            this.Title = title;
            this.Description = description;
        }

        public string Name { get; private set; }
        public Area Area { get; private set; }
        public Resource Title { get; private set; }
        public Resource Description { get; private set; }
        public IReadOnlyDictionary<string, int> Outlets { get; private set; }

        public virtual void ShowRoom(Player player)
        {
            this.ShowRoomDescription(player);
        }

        public virtual void ShowRoomDescription(Player player)
        {
            player.AddOuput(Message.NewLineMessage);
            player.AddOuput(new Message(this.Title, player.Profile.LocaleId, null));
            player.AddOuput(new Message(this.Description, player.Profile.LocaleId, null));
        }
    }
}

