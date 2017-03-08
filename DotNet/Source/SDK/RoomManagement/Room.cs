//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.Globalization;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    public class Room
	{
        public Room(Area area, RoomData data, LocaleIndex locales, ColorIndex colors)
        {
            this.Area = area;
            this.Name = data.Name;
            this.Title = ContentUtility.CreateText(data.Title, locales, colors);
            this.Description = ContentUtility.CreateText(data.Description, locales, colors);
        }

        public string Name { get; private set; }
        public Area Area { get; private set; }
        public IReadOnlyList<Text> Title { get; private set; }
        public IReadOnlyList<Text> Description { get; private set; }

        public void ShowRoom(Player player)
        {
        }
	}
}

