//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ResourceCollection.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.Globalization
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.ContentManagement;

    /// <summary>
    /// Resource data
    /// </summary>
    public static class ResourceCollection
    {
        private static IReadOnlyDictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Text>>> resourceCollection;

        public static void LoadResources(string resourceFolder, LocaleIndex locales, ColorIndex colors)
        {
            var collection = new Dictionary<string, IReadOnlyDictionary<string, IReadOnlyList<Text>>>(StringComparer.OrdinalIgnoreCase);
            if (Directory.Exists(resourceFolder))
            {
                var files = Directory.GetFiles(resourceFolder);
                if (files != null)
                {
                    for (var i=0;i<files.Length;i++)
                    {
                        var resourceCollectionData = Singleton<Serializer<ResourceCollectionData>>.Instance.Deserialize(files[i]);
                        if (!string.IsNullOrEmpty(resourceCollectionData.CollectionName) 
                            && (resourceCollectionData.Resources != null) 
                            && (resourceCollectionData.Resources.Length > 0))
                        {
                            var resources = new Dictionary<string, IReadOnlyList<Text>>(StringComparer.OrdinalIgnoreCase);
                            collection.Add(resourceCollectionData.CollectionName, resources);
                            for (var j = 0; j < resourceCollectionData.Resources.Length; j++)
                            {
                                var resource = resourceCollectionData.Resources[j];
                                resources.Add(resource.Name, ContentUtility.CreateText(resource, locales, colors));
                            }
                        }
                    }
                }
            }

            resourceCollection = collection;
        }

        /// <summary>
        /// Get resource collection by type
        /// </summary>
        /// <param name="type">resource enumerator type</param>
        /// <returns>resource collection for the type</returns>
        public static IReadOnlyDictionary<string, IReadOnlyList<Text>> GetCollection(Type type)
        {
            IReadOnlyDictionary<string, IReadOnlyList<Text>> collection;
            if (resourceCollection.TryGetValue(type.FullName, out collection))
            {
                return collection;
            }

            if (resourceCollection.TryGetValue(type.Name, out collection))
            {
                return collection;
            }

            return null;
        }

        /// <summary>
        /// Get text
        /// </summary>
        /// <typeparam name="T">resource type</typeparam>
        /// <param name="resourceId">resource id</param>
        /// <returns>text in different languages</returns>
        public static IReadOnlyList<Text> GetText<T>(T resourceId) where T: struct
        {
            return Singleton<Resource<T>>.Instance.GetResource(resourceId);
        }
    }
}
