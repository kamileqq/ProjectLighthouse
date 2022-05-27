using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlRoot("users")]
    public class UsersList
    {
        [XmlElement("user")]
        public List<User> Users;

        public UsersList()
        {}

        public UsersList(List<User> users)
        {
            this.Users = users;
        }
    }
}