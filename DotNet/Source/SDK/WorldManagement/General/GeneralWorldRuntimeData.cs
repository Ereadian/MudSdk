namespace Ereadian.MudSdk.Sdk.WorldManagement.General
{
    using System.Xml.Serialization;

    [XmlRoot("data")]
    public class GeneralWorldRuntimeData
    {
        public GeneralWorldRuntimeData()
        {
        }

        public GeneralWorldRuntimeData(GeneralWorldRuntime runtime)
        {
            this.RoomFullName = runtime.Room.FullName;
        }

        [XmlElement("room")]
        public string RoomFullName { get; set; }
    }
}
