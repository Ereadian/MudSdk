//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="GameContext.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using Ereadian.MudSdk.Sdk.Diagnostics;

    public class GameContext
    {
        public GameContext(ILog log)
        {
            this.Log = log;
        }

        public ILog Log { get; private set; }
    }
}
