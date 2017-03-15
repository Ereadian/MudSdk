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
    using Ereadian.MudSdk.Sdk.Globalization;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement; 

    public class Room
	{
        public Room(
            string areaName,
            RoomData data, 
            LocaleManager locales, 
            ColorManager colors,
            Func<string, string, int> getRoomIdFunction)
        {
            this.AreaName = areaName;
            this.Name = data.Name;
            this.Title = ContentUtility.CreateText(data.Title, locales, colors);
            this.Description = ContentUtility.CreateText(data.Description, locales, colors);
        }

        public string Name { get; private set; }
        public string AreaName { get; private set; }
        public IReadOnlyList<Text> Title { get; private set; }
        public IReadOnlyList<Text> Description { get; private set; }
        public IReadOnlyDictionary<string, int> Outlets { get; private set; }

        /// <summary>
        /// Gets room full name
        /// </summary>
        public string FullName
        {
            get
            {
                return string.Format(
                    CultureInfo.InvariantCulture,
                    "{0}{1}{2}",
                    this.AreaName,
                    RoomManager.AreaSeparatorChar,
                    this.Name);
            }
        }

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

