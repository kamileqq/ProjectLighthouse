using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LBPUnion.ProjectLighthouse.Serialization;

namespace LBPUnion.ProjectLighthouse.Types.News;

/// <summary>
///     Used on the info moon on LBP1 and in Recent Activity on LBP2. Broken on LBP1 due to an issue (i cant say the b word jetbrains pls) in the game.
/// </summary>
public class NewsEntry
{
    [Key]
    public int NewsId { get; set; }

    public string Title { get; set; }
    public string Summary { get; set; }
    public string Text { get; set; }

    [NotMapped]
    public NewsImage Image { get; set; }

    public string Category { get; set; }
    public long Timestamp { get; set; }

    public string Serialize()
        => LbpSerializer.StringElement("id", this.NewsId) +
           LbpSerializer.StringElement("title", this.Title) +
           LbpSerializer.StringElement("summary", this.Summary) +
           LbpSerializer.StringElement("text", this.Text) +
           LbpSerializer.StringElement("date", this.Timestamp) +
//           this.Image.Serialize() +
           LbpSerializer.StringElement("category", this.Category);
}