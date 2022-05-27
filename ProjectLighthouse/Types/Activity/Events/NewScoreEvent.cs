using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Activity.Events;

public class NewScoreEvent : IEvent
{
    public long Timestamp { get; set; } = TimestampHelper.TimestampMillis;
    public User User { get; init; }
    public Score Score { get; init; }

    public string Serialize()
    {
        string @event = LbpSerializer.StringElement("timestamp", this.Timestamp) +
                        LbpSerializer.StringElement("actor", this.User.Username) +
                        LbpSerializer.TaggedStringElement("object_slot_id", this.Score.SlotId, "type", "user") +
                        LbpSerializer.StringElement("score", this.Score.Points) +
                        LbpSerializer.StringElement("user_count", this.Score.Type);

        return LbpSerializer.TaggedStringElement("event", @event, "type", "score");
    }
}