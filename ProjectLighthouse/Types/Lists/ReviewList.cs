using System.Collections.Generic;
using System.Xml.Serialization;
using LBPUnion.ProjectLighthouse.Types.Reviews;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlType("reviews")]
    public class ReviewList
    {
        public ReviewList()
        {}

        public ReviewList(int hintStart, int hint, List<Review> reviews)
        {
            this.HintStart = hintStart;
            this.Hint = hint;
            this.Reviews = reviews;
        }

        [XmlAttribute("hint")]
        public int Hint;

        [XmlAttribute("hint_start")]
        public int HintStart;

        [XmlElement("review")]
        public List<Review> Reviews;

    }
}