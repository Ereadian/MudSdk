namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot("profile")]
    public class ProfileData
    {
        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("password")]
        public string PasswordHash { get; set; }

        [XmlAttribute("world")]
        public string WorldName { get; set; }

        [XmlAttribute("active")]
        public DateTime LastActive { get; set; }
    }
}
