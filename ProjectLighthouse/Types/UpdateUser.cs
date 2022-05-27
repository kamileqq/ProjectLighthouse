#nullable enable
using System;
using System.Xml.Serialization;
using LBPUnion.ProjectLighthouse.Types.Profiles;

namespace LBPUnion.ProjectLighthouse.Types
{
    [XmlRoot("updateUser")]
    [XmlType("updateUser")]
    [Serializable]
    public class UpdateUser
    {
        [XmlElement("biography")]
        public string? Biography { get; set; }

        [XmlElement("location")]
        public Location? Location { get; set; }

        [XmlElement("icon")]
        public string? IconHash { get; set; }

        [XmlElement("planets")]
        public string? PlanetHash { get; set; }

        [XmlElement("yay2")]
        public string? YayHash { get; set; }

        [XmlElement("boo2")]
        public string? BooHash { get; set; }

        [XmlElement("meh2")]
        public string? MehHash { get; set; }
    }
}