namespace Ereadian.MudSdk.Sdk.ItemManagement
{
    using System.Xml.Serialization;

    [XmlRoot("definition")]
    public class ItemTypeCollectionData
    {
        [XmlAttribute("category")]
        public int CategoryId { get; set; }

        [XmlElement("types")]
        public ItemTypeData[] Types { get; set; }
    }
}
