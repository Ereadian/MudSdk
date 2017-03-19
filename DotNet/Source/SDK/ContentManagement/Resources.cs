//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="Resources.cs" company="Ereadian"> 
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
    /// <typeparam name="T">resource enumerator type</typeparam>
    public class Resources<T> where T : struct
    {
        /// <summary>
        /// resources for give resource enumerator type
        /// </summary>
        private readonly IReadOnlyList<Resource> resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="Resources{T}" /> class.
        /// </summary>
        public Resources()
        {
            Resource[] collection = null;
            if (typeof(T).IsEnum)
            {
                var resources = ResourceCollection.GetResources(typeof(T));
                if (resources != null)
                {
                    var names = Enum.GetNames(typeof(T));
                    var ids = new int[names.Length];
                    int maxId = 0;
                    for (var i = 0; i < names.Length; i++)
                    {
                        var id = (int)Enum.Parse(typeof(T), names[i]);
                        ids[i] = id;
                        if (maxId < id)
                        {
                            maxId = id;
                        }
                    }

                    collection = new Resource[names.Length];
                    for (var i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        Resource data;
                        if (resources.TryGetValue(name, out data))
                        {
                            collection[ids[i]] = data;
                        }
                    }
                }
            }

            this.resources = collection;
        }

        /// <summary>
        /// Gets resource by resource id
        /// </summary>
        /// <param name="id">resource id</param>
        /// <returns>resource instance</returns>
        public Resource this[T id]
        {
            get
            {
                return this.resources[(int)(object)id];
            }
        }
    }
}