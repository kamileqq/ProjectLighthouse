using System.Collections.Generic;
using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    public class ScoreListWithPlayerScore : ScoreList
    {
        public ScoreListWithPlayerScore()
        {}

        public ScoreListWithPlayerScore(List<Score> scores, int yourScore, int yourRank, int totalScores) : base(scores)
        {
            this.YourScore = yourScore;
            this.YourRank = yourRank;
            this.TotalScores = totalScores;
        }

        [XmlAttribute("yourScore")]
        public int YourScore { get; set; }

        [XmlAttribute("yourRank")]
        public int YourRank { get; set; }

        [XmlAttribute("totalNumScores")]
        public int TotalScores { get; set; }
    }
}