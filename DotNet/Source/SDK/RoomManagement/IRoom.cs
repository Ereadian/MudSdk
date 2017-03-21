//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="IRoom.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.ContentManagement;
    using Ereadian.MudSdk.Sdk.CreatureManagement;

    /// <summary>
    /// Room interface
    /// </summary>
    public interface IRoom
    {
        /// <summary>
        /// Gets room name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets room area
        /// </summary>
        Area Area { get; }

        /// <summary>
        /// Gets room title
        /// </summary>
        Resource Title { get; }

        /// <summary>
        /// Gets a value indicating whether the room is in design
        /// </summary>
        bool InDesign { get; }

        /// <summary>
        /// Gets room description
        /// </summary>
        Resource Description { get; }

        /// <summary>
        /// Gets room outlets
        /// </summary>
        IDictionary<string, IRoom> Outlets { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Room" /> class.
        /// </summary>
        /// <param name="phaseId">phase id</param>
        /// <param name="name">room name</param>
        /// <param name="area">area the room belongs to</param>
        /// <param name="inDesign">whether the room is in design</param>
        /// <param name="roomData">room data</param>
        /// <param name="context">game context</param>
        /// <param name="getRoom">get room method</param>
        /// <returns>true: done. false: need another around initialization</returns>
        bool Init(
            int phaseId, 
            string name, 
            Area area, 
            bool inDesign, 
            XmlElement roomData, 
            IGameContext context, 
            Func<string, IRoom> getRoom);

        /// <summary>
        /// Show current room information to player
        /// </summary>
        /// <param name="player">player to show</param>
        void ShowRoom(Player player);
    }
}