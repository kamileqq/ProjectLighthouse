using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlRoot("myFriends")]
    public class FriendsList
    {
        [XmlElement("user")]
        public List<User> Friends;

        public FriendsList()
        {}

        public FriendsList(List<User> friends)
        {
            this.Friends = friends;
        }
    }
}