//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="RoomExtensions.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.RoomManagement
{
    public static class RoomExtensions
	{
        public static string GeFullName(this Room room)
        {
            return RoomManager.GetRoomFullName(room.Area.Name, room.Name);
        }
    }
}

