//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Message.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Message to user
    /// </summary>
    public struct Message
    {
        /// <summary>
        /// New line message
        /// </summary>
        public static Message NewLineMessage = new Message(
            new Resource(
                new Text[]
                {
                    new Text(
                        LocaleManager.DefaultLocaleId,
                        new Content(
                            new IContent[]
                            {
                                new TextContent(Environment.NewLine)
                            }))
                }));

        /// <summary>
        /// Resource for this message
        /// </summary>
        public Resource Resource;

        /// <summary>
        /// parameters for this message
        /// </summary>
        public IReadOnlyList<object> Parameters;

        /// <summary>
        /// Initializes a new instance of the <see cref="Message" /> struct.
        /// </summary>
        /// <param name="resource">resource for the message</param>
        /// <param name="parameters">parameters for message rendering</param>
        public Message(Resource resource, params object[] parameters)
        {
            this.Resource = resource;
            this.Parameters = parameters;
        }

        /// <summary>
        /// Create message by given resource id
        /// </summary>
        /// <typeparam name="T">type of resource enumerator</typeparam>
        /// <param name="resourceId">resource id</param>
        /// <returns>message instance</returns>
        public static Message Create<T>(T resourceId) where T : struct
        {
            return Create(resourceId, null);
        }

        /// <summary>
        /// Create message by resource id
        /// </summary>
        /// <typeparam name="T">type of resource id</typeparam>
        /// <param name="resourceId">resource id</param>
        /// <param name="parameters">message parameters</param>
        /// <returns>message instance</returns>
        public static Message Create<T>(T resourceId, params object[] parameters)
            where T : struct
        {
            var resource = GetResource<T>(resourceId);
            return new Message(resource, parameters);
        }

        /// <summary>
        /// Get resource by id
        /// </summary>
        /// <typeparam name="T">type of resource enumerator</typeparam>
        /// <param name="id">resource id</param>
        /// <returns>resource instance</returns>
        private static Resource GetResource<T>(T id) where T : struct
        {
            return Singleton<Resources<T>>.Instance[id];
        }
    }
}
