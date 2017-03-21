//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
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
    /// Room on map
    /// </summary>
    public class Room : IRoom
    {
        /// <summary>
        /// title element name
        /// </summary>
        public const string TitleElementName = "title";

        /// <summary>
        /// description element name
        /// </summary>
        public const string DescriptionElementName = "description";

        public const string OutletRootElementName = "outlets";

        public const string OutletElementName = "outlet";

        public const string CommandAttributeName = "command";

        public const string RoomNameAttributeName = "room";

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
        /// Gets a value indicating whether the room is in design
        /// </summary>
        public bool InDesign { get; private set; }

        /// <summary>
        /// Gets room outlets
        /// </summary>
        public IDictionary<string, IRoom> Outlets { get; set; }

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
        /// <returns>false: need another around initialization. true: finished</returns>
        public virtual bool Init(
            int phaseId, 
            string name, 
            Area area, 
            bool inDesign, 
            XmlElement roomData, 
            IGameContext context, 
            Func<string, IRoom> getRoom)
        {
            this.Name = name;
            this.Area = area;
            var titleElement = roomData.SelectSingleNode(TitleElementName) as XmlElement;
            if (titleElement == null)
            {
                this.Title = new Resource(name);
            }
            else
            {
                this.Title = new Resource(titleElement, context.LocaleManager, context.ColorManager);
            }

            var descriptionElement = roomData.SelectSingleNode(DescriptionElementName) as XmlElement;
            if (descriptionElement != null)
            {
                this.Description = new Resource(descriptionElement, context.LocaleManager, context.ColorManager);
            }

            this.InDesign = inDesign;

            Dictionary<string, IRoom> outlets = null;
            var outletRootElement = roomData.SelectSingleNode(OutletRootElementName) as XmlElement;
            if (outletRootElement != null)
            {
                foreach (XmlElement outletElement in outletRootElement.SelectNodes(OutletElementName))
                {
                    var command = outletElement.GetAttribute(CommandAttributeName);
                    if (string.IsNullOrWhiteSpace(command))
                    {
                        // TODO: write error. command already defined
                        continue;
                    }

                    var neighborName = outletElement.GetAttribute(RoomNameAttributeName);
                    if (string.IsNullOrWhiteSpace(neighborName))
                    {
                        // TODO: write error
                        continue;
                    }

                    neighborName = this.ParseRoomName(neighborName.Trim());
                    IRoom neighbor = getRoom(neighborName);
                    if (neighbor == null)
                    {
                        // TODO: log error
                        continue;
                    }

                    if (outlets == null)
                    {
                        outlets = new Dictionary<string, IRoom>(StringComparer.OrdinalIgnoreCase);
                    }

                    outlets.Add(command.Trim(), neighbor);
                }

                this.Outlets = outlets;
            }

            return true;
        }

        /// <summary>
        /// Show current room information to player
        /// </summary>
        /// <param name="player">player to show</param>
        public virtual void ShowRoom(Player player)
        {
            this.ShowRoomDescription(player);
            this.ShowOutlets(player);
        }

        public string ParseRoomName(string name)
        {
            for (var i = 0; i < name.Length; i++)
            {
                if (name[i] == RoomManager.AreaSeparatorChar)
                {
                    return name;
                }
            }

            return RoomManager.GetRoomFullName(this.Area.Name, name);
        }

        /// <summary>
        /// Show room description to player
        /// </summary>
        /// <param name="player">player to show</param>
        protected virtual void ShowRoomDescription(Player player)
        {
            player.AddOuput(Message.NewLineMessage);
            player.AddOuput(new Message(this.Title));
            player.AddOuput(Message.NewLineMessage);
            player.AddOuput(new Message(this.Description));
            player.AddOuput(Message.NewLineMessage);
        }

        protected virtual void ShowOutlets(Player player)
        {
            player.AddOuput(Message.Create(SystemResources.TheRoomOutlets));
            if (!this.ShowGenearlOutlets(player) && !this.ShowSpecialOutlets(player))
            {
            }
        }

        protected virtual bool ShowGenearlOutlets(Player player)
        {
            if (this.Outlets == null)
            {
                return false;
            }

            foreach (var pair in this.Outlets)
            {
                player.AddOuput(new Message(pair.Value.Title));
                player.AddOuput(Message.Create(SystemResources.RoomOutletTemplate, pair.Key));
            }

            player.AddOuput(Message.NewLineMessage);
            return true;
        }

        protected virtual bool ShowSpecialOutlets(Player player)
        {
            return false;
        }
    }
}