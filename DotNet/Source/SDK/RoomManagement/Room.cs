//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System.Xml;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;
    using System;

    /// <summary>
    /// Room on map
    /// </summary>
    public class Room : IRoom
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Room" /> class.
        /// </summary>
        /// <param name="phaseId">phase id</param>
        /// <param name="name">room name</param>
        /// <param name="area">area the room belongs to</param>
        /// <param name="roomData">room data</param>
        /// <param name="context">game context</param>
        /// <param name="getRoom">get room method</param>
        public virtual bool Init(int phaseId, string name, Area area, XmlElement roomData, IGameContext context, Func<string, IRoom> getRoom)
        {
            this.Name = name;
            this.Area = area;
            return true;
        }

        /// <summary>
        /// Gets room name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets room area
        /// </summary>
        public Area Area { get; private set; }

        /// <summary>
        /// Gets room title
        /// </summary>
        public Resource Title { get; private set; }

        /// <summary>
        /// Gets room description
        /// </summary>
        public Resource Description { get; private set; }

        /// <summary>
        /// Gets room outlets
        /// </summary>
        public IReadOnlyDictionary<string, int> Outlets { get; private set; }

        /// <summary>
        /// Show current room information to player
        /// </summary>
        /// <param name="player">player to show</param>
        public virtual void ShowRoom(Player player)
        {
            this.ShowRoomDescription(player);
        }

        /// <summary>
        /// Show room description to player
        /// </summary>
        /// <param name="player">player to show</param>
        public virtual void ShowRoomDescription(Player player)
        {
            player.AddOuput(Message.NewLineMessage);
            player.AddOuput(Message.Create(this.Title));
            player.AddOuput(Message.Create(this.Description));
        }
    }
}