﻿//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ContentType.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    /// <summary>
    /// Content type
    /// </summary>
    public enum ContentType
    {
        /// <summary>
        /// Color segment
        /// </summary>
        Color,

        /// <summary>
        /// parameter injection
        /// </summary>
        Parameter,

        /// <summary>
        /// pure text
        /// </summary>
        Text,

        /// <summary>
        /// Blank content
        /// </summary>
        /// <remarks>
        /// For example, new line, tab, etc.
        /// </remarks>
        Blank,
    }
}
