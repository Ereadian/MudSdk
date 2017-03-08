namespace Ereadian.MudSdk.Sdk.CreatureManagement
{
    using System;
    using System.Xml.Serialization;

    [XmlRoot("profile")]
    public class ProfileData
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("password")]
        public string PasswordHash { get; set; }

        [XmlElement("locale")]
        public string LocaleName { get; set; }

        [XmlElement("world")]
        public string WorldName { get; set; }

        [XmlElement("active")]
        public DateTime LastActive { get; set; }
    }
}
