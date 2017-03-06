namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System.Xml.Serialization;

    [XmlRoot("profile")]
    public class ProfileData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("password")]
        public string PasswordHash { get; set; }

        [XmlAttribute("area")]
        public string AreaName { get; set; }

        [XmlAttribute("room")]
        public string RoomName { get; set; }
    }
}
