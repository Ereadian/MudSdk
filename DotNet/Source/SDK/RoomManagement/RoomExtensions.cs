//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="RoomExtensions.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    /// <summary>
    /// Room extensions
    /// </summary>
    public static class RoomExtensions
    {
        /// <summary>
        /// Get room full name
        /// </summary>
        /// <param name="room">room instance</param>
        /// <returns>room full name</returns>
        public static string GeFullName(this Room room)
        {
            return RoomManager.GetRoomFullName(room.Area.Name, room.Name);
        }
    }
}
