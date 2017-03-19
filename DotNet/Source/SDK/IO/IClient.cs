//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="IClient.cs" company="Ereadian"> 
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
    public interface IClient
    {
        /// <summary>
        /// Render message
        /// </summary>
        /// <param name="message">message to render</param>
        /// <param name="localeId">locale id</param>
        void RenderMessage(Message message, int localeId);

        /// <summary>
        /// Disconnect from game
        /// </summary>
        void Disconnect();
    }
}
