using LBPUnion.ProjectLighthouse.Serialization;
using LBPUnion.ProjectLighthouse.Types.Levels;

namespace LBPUnion.ProjectLighthouse.Types.Activity.Events;

public class DpadRateLevelEvent : IEvent
{
    public long Timestamp { get; set; }
    public User User { get; init; }
    public Slot Slot { get; init; }
    public RatedLevel RatedLevel { get; init; }
    public string Serialize()
    {
        string @event = LbpSerializer.StringElement("timestamp", this.Timestamp) +
                        LbpSerializer.StringElement("actor", this.User.Username) +
                        LbpSerializer.TaggedStringElement("object_slot_id", this.Slot.SlotId, "type", "user") +
                        LbpSerializer.StringElement("dpad_rating", RatedLevel.Rating);

        return LbpSerializer.TaggedStringElement("event", @event, "type", "dpad_rate_level");
    }
}