//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="ResourceCollection.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk.ContentManagement
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.IO;

    /// <summary>
    /// Resource data
    /// </summary>
    public static class ResourceCollection
    {
        public const string ResourceCollectionNameAttributeName = "name";
        public const string ResourceElementName = "resource";
        public const string ResourceNameAttributeName = "name";
        public const string ContentElementName = "content";

        private static Dictionary<string, Dictionary<string, Resource>> resourceCollection;

        public static void LoadResources(string resourceFolder, IGameContext context)
        {
            var storage = context.ContentStorage;
            var locales = context.LocaleManager;
            var colors = context.ColorManager;
            resourceCollection = new Dictionary<string, Dictionary<string, Resource>>(StringComparer.OrdinalIgnoreCase);
            var files = storage.GetFiles(resourceFolder);
            if (files != null)
            {
                var list = new List<Text>();
                for (var i = 0; i < files.Count; i++)
                {
                    var path = storage.CombinePath(resourceFolder, files[i]);
                    XmlElement rootElement;
                    using (var stream = storage.OpenForRead(path))
                    {
                        var document = new XmlDocument();
                        document.Load(stream);
                        rootElement = document.DocumentElement;
                    }

                    var collectionName = rootElement.GetAttribute(ResourceCollectionNameAttributeName);
                    if (string.IsNullOrWhiteSpace(collectionName))
                    {
                        // TODO: log error. resource collection name is required
                        continue;
                    }

                    Dictionary<string, Resource> resources = null;
                    foreach (XmlElement resourceElement in rootElement.SelectNodes(ResourceElementName))
                    {
                        var resourceName = resourceElement.GetAttribute(ResourceNameAttributeName);
                        if (string.IsNullOrWhiteSpace(resourceName))
                        {
                            // TODO: log error. resource name is required
                            continue;
                        }

                        list.Clear();
                        foreach (XmlElement contentElement in resourceElement.SelectNodes(ContentElementName))
                        {
                            var content = new Text(contentElement, locales, colors);
                            list.Add(content);
                        }

                        if (list.Count > 0)
                        {
                            if (!resourceCollection.TryGetValue(collectionName, out resources))
                            {
                                resources = new Dictionary<string, Resource>(StringComparer.OrdinalIgnoreCase);
                                resourceCollection.Add(collectionName, resources);
                            }
                            else
                            {
                                if (resources.ContainsKey(resourceName))
                                {
                                    // TODO: duplicate resource found
                                    continue;
                                }
                            }

                            resources.Add(resourceName, new Resource(list.ToArray()));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Get resource collection by type
        /// </summary>
        /// <param name="type">resource enumerator type</param>
        /// <returns>resource collection for the type</returns>
        public static IReadOnlyDictionary<string, Resource> GetResources(Type type)
        {
            Dictionary<string, Resource> collection;
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
    }
}