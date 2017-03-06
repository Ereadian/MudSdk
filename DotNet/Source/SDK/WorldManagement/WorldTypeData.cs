namespace Ereadian.MudSdk.Sdk.WorldManagement
{
    using System.Xml.Serialization;

    [XmlRoot("world")]
    public class WorldTypeData
    {
        [XmlAttribute("name")]
        public string WorldName { get; set; }

        [XmlAttribute("type")]
        public string TypeName { get; set; }
    }
}
