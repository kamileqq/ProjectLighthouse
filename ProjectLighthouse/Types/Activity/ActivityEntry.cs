using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LBPUnion.ProjectLighthouse.Types.Activity;

public class ActivityEntry
{
    [Key]
    public int EntryId { get; set; }

    public long Timestamp { get; set; }

    public EventType Type { get; set; }

    [ForeignKey(nameof(UserId))]
    public User User { get; set; }

    public int UserId { get; set; }

    public int RelatedId { get; set; }
}