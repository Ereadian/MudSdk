namespace Ereadian.MudSdk.Sdk.ItemManagement
{
    using System.Xml.Serialization;
    using Ereadian.MudSdk.Sdk.Globalization;

    [XmlRoot("type")]
    public class ItemTypeData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("descriptions")]
        public ContentData[] Descriptions { get; set; }

        [XmlElement("actions")]
        public string[] SupportedActions { get; set; }
    }
}
