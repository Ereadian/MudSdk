//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="IContent.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.Contents
{
    /// <summary>
    /// Content interface
    /// </summary>
    public interface IContent
    {
        /// <summary>
        /// Gets content type
        /// </summary>
        ContentType Type { get; }
    }
}
