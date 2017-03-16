namespace Ereadian.MudSdk.Sdk.Runtime
{
    using System.Configuration;
    using System.Xml;

    public class XmlConfigurationSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, XmlNode section)
        {
            return section as XmlElement;
        }
    }
}
