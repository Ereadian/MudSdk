//------------------------------------------------------------------------------------------------------------------------------------------ 
// <copyright file="TypeManager.cs" company="Ereadian"> 
//     Copyright (c) Ereadian.  All rights reserved. 
// </copyright> 
//------------------------------------------------------------------------------------------------------------------------------------------ 

namespace Ereadian.MudSdk.Sdk
{
    using System;
    using System.Collections.Generic;
    using System.Xml;
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
        /// <summary>
        /// Type element name
        /// </summary>
        public const string TypeElementName = "type";

        /// <summary>
        /// name of type name attribute
        /// </summary>
        public const string TypeNameAttributeName = "name";

        /// <summary>
        /// default type XML file name
        /// </summary>
        private const string DefaultTypeFilename = "types.xml";

        /// <summary>
        /// type collection/mapping
        /// </summary>
        private readonly IReadOnlyDictionary<string, Type> types;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeManager" /> class.
        /// </summary>
        /// <param name="storage">content storage</param>
        /// <param name="typeFilename">type XML file path</param>
        public TypeManager(IContentStorage storage, string typeFilename = DefaultTypeFilename)
            : this(storage.LoadXml(typeFilename))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeManager" /> class.
        /// </summary>
        /// <param name="rootElement">type root XML element</param>
        public TypeManager(XmlElement rootElement)
        {
            var types = new Dictionary<string, Type>(StringComparer.OrdinalIgnoreCase);
            if (rootElement != null)
            {
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