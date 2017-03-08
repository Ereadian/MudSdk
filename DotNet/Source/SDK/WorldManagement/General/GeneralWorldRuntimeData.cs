namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System.Xml.Serialization;

    [XmlRoot("data")]
    public class GeneralWorldRuntimeData
    {
        [XmlElement("room")]
        public string RoomFullName { get; set; }
    }
}
