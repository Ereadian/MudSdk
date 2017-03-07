namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System.Xml.Serialization;

    [XmlRoot("world")]
    public class GeneralWorldData
    {
        [XmlElement("entry")]
        public string EntryRoomName { get; set; }

        [XmlElement("respawn")]
        public string RespawnRoomName { get; set; }
    }
}
