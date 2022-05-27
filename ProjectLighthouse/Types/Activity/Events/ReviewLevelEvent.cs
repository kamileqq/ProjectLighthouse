using LBPUnion.ProjectLighthouse.Serialization;
using LBPUnion.ProjectLighthouse.Types.Levels;
using LBPUnion.ProjectLighthouse.Types.Reviews;

namespace LBPUnion.ProjectLighthouse.Types.Activity.Events;

public class ReviewLevelEvent : IEvent
{
    public long Timestamp { get; set; }
    public User User { get; init; }
    public Slot Slot { get; init; }
    public Review Review { get; init; }
    public string Serialize()
    {
        string @event = LbpSerializer.StringElement("timestamp", this.Timestamp) +
                        LbpSerializer.StringElement("actor", this.User.Username) +
                        LbpSerializer.TaggedStringElement("object_slot_id", this.Slot.SlotId, "type", "user") +
                        LbpSerializer.StringElement("review_id", this.Slot.SlotId) +
                        LbpSerializer.StringElement("review_modified", this.Review.Timestamp);

        return LbpSerializer.TaggedStringElement("event", @event, "type", "review_level");
    }
}