using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlRoot("photos")]
    public class PhotoList
    {
        [XmlElement("photo")]
        public List<Photo> Photos;

        public PhotoList()
        {}

        public PhotoList(List<Photo> photos)
        {
            this.Photos = photos;
        }
    }
}