using System.Collections.Generic;
using System.Xml.Serialization;
using LBPUnion.ProjectLighthouse.Types.Profiles;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlRoot("comments")]
    public class CommentList
    {
        [XmlElement("comment")]
        public List<Comment> Comments;

        public CommentList()
        {}

        public CommentList(List<Comment> comments)
        {
            this.Comments = comments;
        }
    }
}