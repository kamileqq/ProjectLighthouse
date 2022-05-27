namespace LBPUnion.ProjectLighthouse.Types.Activity;

public interface IEvent
{
    public long Timestamp { get; set; }
    public User User { get; init; }

    public string Serialize();
}