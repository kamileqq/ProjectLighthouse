using System.Collections.Generic;
using System.Linq;
using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Activity;

public class UserGroup
{
    public long Timestamp { get; set; } = TimestampHelper.TimestampMillis;
    public User User { get; init; }
    public List<IEvent> Events { get; init; }

    public string Serialize()
    {
        string events = this.Events.Aggregate(string.Empty, (current, @event) => current + @event.Serialize());

        string group = LbpSerializer.StringElement("timestamp", Timestamp) +
                       LbpSerializer.StringElement("user_id", User.Username) +
                       LbpSerializer.StringElement("events", events);

        return LbpSerializer.TaggedStringElement("group", group, "type", "user");
    }
}