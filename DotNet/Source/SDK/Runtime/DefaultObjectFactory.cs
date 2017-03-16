namespace Ereadian.MudSdk.Sdk.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Xml;

    public class DefaultObjectFactory : IObjectFactory
    {
        public const string RootElementName = "factory";
        public const string FactoryElementName = "add";
        public const string SourceTypeAttributeName = "source";
        public const string TargetTypeAttributeName = "target";
        public const string ConstructorElementName = "constructor";
        public const string ParameterElementName = "parameter";
        public const string ParameterTypeAttributeName = "type";
        public const string ConvertAttributeName = "convert";
        public const string FromConfigurationAttributeName = "configuration";
        public const string FactoryNameAttributeName = "name";

        private IDictionary<Type, IDictionary<string, Creator>> storage;

        public DefaultObjectFactory() 
            : this(ConfigurationManager.GetSection(RootElementName) as XmlElement, ConfigurationManager.AppSettings)
        {
        }

        public DefaultObjectFactory(XmlElement xml, NameValueCollection configurations)
        {
            this.storage = new Dictionary<Type, IDictionary<string, Creator>>();
            if (xml != null)
            {
                foreach (XmlElement itemElment in xml.SelectNodes(FactoryElementName))
                {
                    var sourceTypeName = itemElment.GetAttribute(SourceTypeAttributeName);
                    if (string.IsNullOrEmpty(sourceTypeName))
                    {
                        continue;
                    }

                    var type = Type.GetType(sourceTypeName);
                    if (type == null)
                    {
                        continue;
                    }

                    IDictionary<string, Creator> creaters;
                    if (!this.storage.TryGetValue(type, out creaters))
                    {
                        creaters = new Dictionary<string, Creator>(StringComparer.OrdinalIgnoreCase);
                        this.storage.Add(type, creaters);
                    }

                    var creator = new Creator(itemElment, configurations);
                    var name = itemElment.GetAttribute(FactoryNameAttributeName) ?? string.Empty;
                    creaters[name] = creator;
                }
            }
        }

        public bool TryGetInstance<T>(out T instance, string name = null)
        {
            IDictionary<string, Creator> creators;
            if (this.storage.TryGetValue(typeof(T), out creators))
            {
                Creator creator;
                if (creators.TryGetValue(name ?? string.Empty, out creator))
                {
                    instance = (T)creator.Instance;
                    return true;
                }
            }

            instance = default(T);
            return false;
        }

        private class Creator
        {
            private Lazy<object> loader;

            public Creator(XmlElement xml, NameValueCollection configurations)
            {
                this.loader = new Lazy<object>(() => Create(xml, configurations));
            }

            private static object Create(XmlElement xml, NameValueCollection configurations)
            {
                Type type;
                object instance;
                return TryCreate(
                    xml, 
                    TargetTypeAttributeName, 
                    configurations, 
                    out type, 
                    out instance) ? instance : null;
            }

            private static bool TryCreate(
                XmlElement xml, 
                string typeAttributeName, 
                NameValueCollection configurations, 
                out Type type, 
                out object instance)
            {
                type = null;
                instance = null;
                var targetTypeName = xml.GetAttribute(TargetTypeAttributeName);
                if (string.IsNullOrEmpty(targetTypeName))
                {
                    return false;
                }

                type = Type.GetType(targetTypeName);
                if (type == null)
                {
                    return false;
                }

                if (!xml.HasChildNodes)
                {
                    return true;
                }

                var convert = GetBoolean(xml, ConvertAttributeName, false);
                var fromConfig = GetBoolean(xml, FromConfigurationAttributeName, false);

                Type[] parameterTypes = Type.EmptyTypes;
                object[] parameterValues = null;
                var constructorElement = xml.SelectSingleNode(ConstructorElementName) as XmlElement;
                if (constructorElement == null)
                {
                    if (convert)
                    {
                        var data = xml.InnerText;
                        if (fromConfig)
                        {
                            data = configurations[data];
                        }

                        instance = Convert.ChangeType(data, type);
                        return true;
                    }
                }
                else
                {
                    var types = new List<Type>();
                    var values = new List<object>();
                    foreach (XmlElement parameterXml in constructorElement.SelectNodes(ParameterElementName))
                    {
                        Type parameterType;
                        object parameterValue;
                        if (!TryCreate(parameterXml, ParameterTypeAttributeName, configurations, out parameterType, out parameterValue))
                        {
                            return false;
                        }

                        types.Add(parameterType);
                        values.Add(parameterValue);
                    }

                    if (types.Count > 0)
                    {
                        parameterTypes = types.ToArray();
                        parameterValues = values.ToArray();
                    }
                }

                var constructor = type.GetConstructor(parameterTypes);
                if (constructor == null)
                {
                    return false;
                }

                bool success;
                try
                {
                    instance = constructor.Invoke(parameterTypes);
                    success = true;
                }
                catch
                {
                    success = false;
                }

                return success;
            }

            internal object Instance
            {
                get
                {
                    return this.loader.Value;
                }
            }

            private static bool GetBoolean(XmlElement xml, string attributeName, bool defaultValue)
            {
                if (xml != null)
                {
                    var rawData = xml.GetAttribute(attributeName);
                    if (!string.IsNullOrEmpty(rawData))
                    {
                        bool value;
                        if (bool.TryParse(rawData, out value))
                        {
                            return value;
                        }
                    }
                }

                return defaultValue;
            }
        }
    }
}
