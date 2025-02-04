#nullable enable
using LBPUnion.ProjectLighthouse.Configuration;
using LBPUnion.ProjectLighthouse.Levels;
using LBPUnion.ProjectLighthouse.PlayerData;
using LBPUnion.ProjectLighthouse.PlayerData.Profiles;
using LBPUnion.ProjectLighthouse.PlayerData.Reviews;
using LBPUnion.ProjectLighthouse.Servers.Website.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Servers.Website.Pages;

public class SlotPage : BaseLayout
{
    public List<Comment> Comments = new();
    public List<Review> Reviews = new();

    public readonly bool CommentsEnabled = ServerConfiguration.Instance.UserGeneratedContentLimits.LevelCommentsEnabled;
    public readonly bool ReviewsEnabled = ServerConfiguration.Instance.UserGeneratedContentLimits.LevelReviewsEnabled;

    public Slot? Slot;
    public SlotPage(Database database) : base(database)
    {}

    public async Task<IActionResult> OnGet([FromRoute] int id)
    {
        Slot? slot = await this.Database.Slots.Include(s => s.Creator).FirstOrDefaultAsync(s => s.SlotId == id);
        if (slot == null) return this.NotFound();

        this.Slot = slot;

        if (this.CommentsEnabled)
        {
            this.Comments = await this.Database.Comments.Include(p => p.Poster)
                .OrderByDescending(p => p.Timestamp)
                .Where(c => c.TargetId == id && c.Type == CommentType.Level)
                .Take(50)
                .ToListAsync();
        }
        else
        {
            this.Comments = new List<Comment>();
        }

        if (this.ReviewsEnabled)
        {
            this.Reviews = await this.Database.Reviews.Include(r => r.Reviewer)
                .OrderByDescending(r => r.ThumbsUp - r.ThumbsDown)
                .ThenByDescending(r => r.Timestamp)
                .Where(r => r.SlotId == id)
                .Take(50)
                .ToListAsync();
        }
        else
        {
            this.Reviews = new List<Review>();
        }

        if (this.User == null) return this.Page();

        foreach (Comment c in this.Comments)
        {
            Reaction? reaction = await this.Database.Reactions.FirstOrDefaultAsync(r => r.UserId == this.User.UserId && r.TargetId == c.CommentId);
            if (reaction != null) c.YourThumb = reaction.Rating;
        }

        return this.Page();
    }
}