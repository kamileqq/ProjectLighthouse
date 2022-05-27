using System.Xml.Serialization;

namespace LBPUnion.ProjectLighthouse.Types
{
    public class PlanetStats
    {
        public PlanetStats(int totalSlotCount, int teamPickCount)
        {
            this.TotalSlotCount = totalSlotCount;
            this.TeamPickCount = teamPickCount;
        }
        public PlanetStats()
        {}

        [XmlElement("totalSlotCount")]
        public int TotalSlotCount { get; set; }

        [XmlElement("mmPicksCount")]
        public int TeamPickCount { get; set; }
    }
}