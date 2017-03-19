//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="TypeManager.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Xml;
    using System.Collections.Generic;
    using Ereadian.MudSdk.Sdk.IO;

    /// <summary>
    /// Type manager
    /// </summary>
    /// <example>
    /// File example:
    /// <![CDATA[
    /// <types>
    ///     ...
    ///     <type name="SpecialRoom">SmallTown.Rooms.SpecialRoom, SmallTown</type>
    ///     ...
    /// </types>
    /// ]]>
    /// </example>
    public class TypeManager
    {
        public const string TypeElementName = "type";
        public const string TypeNameAttributeName = "name";
        private const string DefaultTypeFilename = "types.xml";

        private readonly IReadOnlyDictionary<string, Type> types;

        public TypeManager(IContentStorage storage, string typeFilename = DefaultTypeFilename)
        {
            var types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            var rootElement = storage.LoadXml(typeFilename);
            foreach (XmlElement typeElement in rootElement.SelectNodes(TypeElementName))
            {
                var name = typeElement.GetAttribute(TypeNameAttributeName);
                if (string.IsNullOrWhiteSpace(name))
                {
                    // TODO: write error. name is required
                    continue;
                }

                name = name.Trim();
                if (types.ContainsKey(name))
                {
                    // TODO: write error. duplicate name found
                }

                var typeName = typeElement.InnerText;
                if (string.IsNullOrWhiteSpace(typeName))
                {
                    // TODO: write type. type name is required
                    continue;
                }

                var type = Type.GetType(typeName.Trim());
                if (type == null)
                {
                    // TODO: write error. Type does not exist
                    continue;
                }

                types.Add(name, type);
            }

            this.types = types;
        }

        public Type GetRegisteredType(string name)
        {
            Type type;
            return string.IsNullOrEmpty(name) || !this.types.TryGetValue(name, out type) ? null : type;
        }
    }
}