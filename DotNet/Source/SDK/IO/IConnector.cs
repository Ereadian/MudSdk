﻿//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="IConnector.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.IO
{
    using System;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    /// <summary>
    /// Connector interface
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        /// Run user command
        /// </summary>
        /// <param name="command">command text</param>
        void RunUserCommand(string command);

        /// <summary>
        /// Disconnect from game
        /// </summary>
        void Disconnect();
    }
}
