using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlRoot("resources")]
    public class ResourcesList
    {
        [XmlElement("resource")]
        public List<string> Resources;

        public ResourcesList()
        {}

        public ResourcesList(List<string> resources)
        {
            this.Resources = resources;
        }
    }
}