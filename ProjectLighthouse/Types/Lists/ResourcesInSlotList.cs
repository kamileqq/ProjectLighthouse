using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlType("slot")]
    [XmlRoot("slot")]
    public class ResourcesInSlotList
    {
        public ResourcesInSlotList()
        {}

        public ResourcesInSlotList(List<string> resources, string type = "user")
        {
            this.Resources = resources;
            this.Type = type;
        }

        [XmlElement("resource")]
        public List<string> Resources;

        [XmlAttribute("type")]
        public string Type;
    }
}