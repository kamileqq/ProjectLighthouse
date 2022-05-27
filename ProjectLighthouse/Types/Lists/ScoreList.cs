using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlRoot("score")]
    public class ScoreList
    {
        [XmlElement("playRecord")]
        public List<Score> Scores;

        public ScoreList()
        {}

        public ScoreList(List<Score> scores)
        {
            this.Scores = scores;
        }
    }
}