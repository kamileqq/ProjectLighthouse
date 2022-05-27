using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlRoot("favouriteUsers")]
    public class FavouriteUsersList
    {
        [XmlElement("user")]
        public List<User> FavouriteUsers;

        [XmlAttribute("total")]
        public int Total;

        public FavouriteUsersList()
        {}

        public FavouriteUsersList(List<User> favouriteUsers, int total)
        {
            this.FavouriteUsers = favouriteUsers;
            this.Total = total;
        }
    }
}