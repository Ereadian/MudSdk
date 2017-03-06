//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Room.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Ereadian.MudSdk.Sdk.Globalization;

    /// <summary>
    /// Room data
    /// </summary>
    public class RoomData
    {
        /// <summary>
        /// Gets or sets room description in different languages
        /// </summary>
        public ContentData[] Descriptions { get; set; }
    }
}
