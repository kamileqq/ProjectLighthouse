using LBPUnion.ProjectLighthouse.Serialization;
using LBPUnion.ProjectLighthouse.Types.Levels;

namespace LBPUnion.ProjectLighthouse.Types.Activity.Events;

public class PublishLevelEvent : IEvent
{
    public long Timestamp { get; set; }
    public User User { get; init; }
    public Slot Slot { get; init; }
    public string Serialize()
    {
        string @event = LbpSerializer.StringElement("timestamp", this.Timestamp) +
                        LbpSerializer.StringElement("actor", this.User.Username) +
                        LbpSerializer.StringElement("republish", 0) +
                        LbpSerializer.StringElement("count", 1) +
                        LbpSerializer.TaggedStringElement("object_slot_id", this.Slot.SlotId, "type", "user");

        return LbpSerializer.TaggedStringElement("event", @event, "type", "publish_level");
    }
}