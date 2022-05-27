using System.Collections.Generic;
using System.Xml.Serialization;
using LBPUnion.ProjectLighthouse.Types.Levels;

namespace LBPUnion.ProjectLighthouse.Types.Lists
{
    [XmlType("slots")]
    public class SlotsList
    {

        [XmlAttribute("hint_start")]
        public int HintStart;

        [XmlElement("slot")]
        public List<Slot> Slots;

        [XmlAttribute("total")]
        public int Total;

        public SlotsList(List<Slot> slots, int hintStart, int total)
        {
            this.Slots = slots;
            this.HintStart = hintStart;
            this.Total = total;
        }

        public SlotsList()
        {}
    }
}