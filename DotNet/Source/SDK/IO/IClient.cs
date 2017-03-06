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
        /// <param name="content">incoming message</param>
        /// <param name="colorIndex">color index</param>
        /// <param name="parameters">message parameters</param>
        void RenderMessage(IReadOnlyList<IContent> content, ColorIndex colorIndex, IReadOnlyList<object> parameters);

        /// <summary>
        /// Disconnect from game
        /// </summary>
        void Disconnect();
    }
}
