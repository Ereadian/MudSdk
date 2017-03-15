//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Creature.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using Ereadian.MudSdk.Sdk.RoomManagement;

    public abstract class Creature : ActionableObject
    {
        public Creature(ActionableObjectManager manager) : base(manager)
        {
        }

        public Room Room { get; set; }
    }
}
