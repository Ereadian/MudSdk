namespace Ereadian.MudSdk.Sdk.Runtime
{
    using System;
    using System.Xml;
    using Ereadian.MudSdk.Sdk.Diagnostics;

    public static class RuntimeUtility
    {
        /// <summary>
        /// name of reference type attribute name
        /// </summary>
        public const string TypeNameAttributeName = "reference";

        public static T CreateInstance<T>(XmlElement typeElement, TypeManager typeManager, ILog log) where T : class
        {
            T instance;
            return TryCreateInstance<T>(typeElement, typeManager, log, out instance) ? instance : null;
        }

        public static bool TryCreateInstance<T>(XmlElement typeElement, TypeManager typeManager, ILog log, out T instance)
        {
            var type = GetTypeFromXml(typeElement, typeManager, log);
            instance = default(T);
            bool success = false;
            if (type != null)
            {
                try
                {
                    var rawInstance = Activator.CreateInstance(type);
                    if (rawInstance is T)
                    {
                        instance = (T)rawInstance;
                        success = true;
                    }
                }
                catch
                {
                    // TODO: write error. Creating room got exception
                }
            }

            return success;
        }

        public static Type GetTypeFromXml(XmlElement typeElement, TypeManager typeManager, ILog log)
        {
            Type type = null;
            var referenceName = typeElement.GetAttribute(TypeNameAttributeName);
            if (!string.IsNullOrWhiteSpace(referenceName))
            {
                type = typeManager.GetRegisteredType(referenceName.Trim());
                if (type == null)
                {
                    // TODO: write error. could not find registered type
                }
            }
            else
            {
                var typeName = typeElement.InnerText;
                if (string.IsNullOrEmpty(typeName))
                {
                    // TODO: write error. type name is required
                }
                else
                {
                    type = Type.GetType(typeName.Trim());
                    if (type == null)
                    {
                        // TODO: write type does not exist
                    }
                }
            }

            return type;
        }
    }
}
