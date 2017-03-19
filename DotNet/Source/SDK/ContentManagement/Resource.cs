//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Resource.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Resource data
    /// </summary>
    public struct Resource
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Resource" /> struct.
        /// </summary>
        /// <param name="data">resource data</param>
        public Resource(IReadOnlyList<Text> data)
        {
            this.Data = data;
        }

        /// <summary>
        /// Gets resource data
        /// </summary>
        public IReadOnlyList<Text> Data { get; private set; }

        /// <summary>
        /// Gets content from resource by locale id
        /// </summary>
        /// <param name="localeId">locale id</param>
        /// <returns>content of the locale</returns>
        public Content this[int localeId]
        {
            get
            {
                if ((this.Data == null) || (this.Data.Count < 1))
                {
                    return default(Content);
                }

                for (var i = 0; i < this.Data.Count; i++)
                {
                    var content = this.Data[i];
                    if (content.LocaleId == localeId)
                    {
                        return content.Content;
                    }
                }

                if (localeId == LocaleManager.DefaultLocaleId)
                {
                    return this.Data[0].Content;
                }

                return this[LocaleManager.DefaultLocaleId];
            }
        }
    }
}
