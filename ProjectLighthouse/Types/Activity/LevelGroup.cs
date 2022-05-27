using System.Collections.Generic;
using System.Linq;
using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Activity;

public class LevelGroup
{
    public long Timestamp { get; set; } = TimestampHelper.TimestampMillis;
    public int SlotId { get; init; }
    public List<UserGroup> UserGroups { get; set; }

    public string Serialize()
    {
        string subGroups = this.UserGroups.Aggregate(string.Empty, (current, userGroup) => current + userGroup.Serialize());

        string group = LbpSerializer.StringElement("timestamp", Timestamp) +
                       LbpSerializer.TaggedStringElement("slot_id", SlotId, "type", "user") +
                       LbpSerializer.StringElement("subgroups", subGroups);

        return LbpSerializer.TaggedStringElement("group", group, "type", "level");
    }
}