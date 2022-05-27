using LBPUnion.ProjectLighthouse.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.Activity.Events;

public class NewsPostEvent : IEvent
{
    public long Timestamp { get; set; }
    public User User { get; init; }
    public int NewsPostId { get; init; }

    public string Serialize()
    {
        string @event = LbpSerializer.StringElement("timestamp", this.Timestamp) + LbpSerializer.StringElement("id", this.NewsPostId);

        return LbpSerializer.StringElement("item", @event);
    }
}