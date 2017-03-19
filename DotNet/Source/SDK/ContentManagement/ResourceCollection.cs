﻿//------------------------------------------------------------------------------------------------------------------------------------------ 
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
    /// Resource collection
    /// </summary>
    public static class ResourceCollection
    {
        /// <summary>
        /// name of collection name attribute
        /// </summary>
        public const string ResourceCollectionNameAttributeName = "name";

        /// <summary>
        /// name of resource element
        /// </summary>
        public const string ResourceElementName = "resource";

        /// <summary>
        /// name of resource name attribute
        /// </summary>
        public const string ResourceNameAttributeName = "name";

        /// <summary>
        /// name of content element
        /// </summary>
        public const string ContentElementName = "content";

        /// <summary>
        /// default resource folder name
        /// </summary>
        public const string DefaultResourceFolderName = "contents";

        /// <summary>
        /// resource collection
        /// </summary>
        private static Dictionary<string, Dictionary<string, Resource>> resourceCollection;

        /// <summary>
        /// Load resources
        /// </summary>
        /// <param name="context">game context</param>
        public static void LoadResources(IGameContext context)
        {
            LoadResources(DefaultResourceFolderName, context);
        }

        /// <summary>
        /// Load resources
        /// </summary>
        /// <param name="resourceFolder">resource folder name</param>
        /// <param name="context">game context</param>
        public static void LoadResources(string resourceFolder, IGameContext context)
        {
            var storage = context.ContentStorage;
            var localeManager = context.LocaleManager;
            var colorManager = context.ColorManager;
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
                            var content = new Text(contentElement, localeManager, colorManager);
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