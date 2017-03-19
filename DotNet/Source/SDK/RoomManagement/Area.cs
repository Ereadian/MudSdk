//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Area.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Map area
    /// </summary>
    public class Area
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Area" /> class.
        /// </summary>
        /// <param name="name">area name</param>
        public Area(string name)
        {
            this.Name = name;
            this.Rooms = new Dictionary<string, IRoom>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets area name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets rooms of this area
        /// </summary>
        public IDictionary<string, IRoom> Rooms { get; private set; }
    }
}
