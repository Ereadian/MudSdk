//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ResourceData.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.Globalization
{
    using System;
    using System.Collections.Generic;
    using System.Xml.Serialization;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    /// <summary>
    /// Resource data
    /// </summary>
    public class Resource<T> where T: struct
    {
        private readonly IReadOnlyList<IReadOnlyList<Text>> resources;

        public Resource()
        {
            IReadOnlyList<Text>[] collection = null;
            if (typeof(T).IsEnum)
            {
                var resources = ResourceCollection.GetCollection(typeof(T));
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

                    collection = new IReadOnlyList<Text>[names.Length];
                    for (var i = 0; i < names.Length; i++)
                    {
                        var name = names[i];
                        IReadOnlyList<Text> data;
                        if (resources.TryGetValue(name, out data))
                        {
                            collection[ids[i]] = data;
                        }
                    }
                }
            }

            resources = collection;
        }

        public IReadOnlyList<Text> GetResource(T id)
        {
            return this.resources[(int)(object)id];
        }
    }
}
