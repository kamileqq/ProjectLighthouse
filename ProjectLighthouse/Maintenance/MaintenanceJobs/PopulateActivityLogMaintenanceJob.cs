#nullable enable
using System.Threading.Tasks;
using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Types;
using LBPUnion.ProjectLighthouse.Types.Activity;
using LBPUnion.ProjectLighthouse.Types.Levels;
using LBPUnion.ProjectLighthouse.Types.Reviews;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Maintenance.MaintenanceJobs;

public class PopulateActivityLogMaintenanceJob : IMaintenanceJob
{
    private readonly Database database = new();

    public async Task Run()
    {
        // Populate with scores
        foreach (Score score in await this.database.Scores.ToListAsync())
        {
            User? scoreUser = await this.database.Users.FirstOrDefaultAsync(u => u.Username == score.MainPlayer);
            if (scoreUser == null) continue;

            this.database.ActivityLog.Add
            (
                new ActivityEntry
                {
                    User = scoreUser,
                    UserId = scoreUser.UserId,
                    RelatedId = score.ScoreId,
                    Type = EventType.NewScore,
                    Timestamp = TimestampHelper.TimestampMillis,
                }
            );
        }

        // Populate with new levels
        foreach (Slot slot in await this.database.Slots.Include(s => s.Creator).ToListAsync())
        {
            this.database.ActivityLog.Add
            (
                new ActivityEntry
                {
                    User = slot.Creator,
                    UserId = slot.CreatorId,
                    RelatedId = slot.SlotId,
                    Type = EventType.PublishLevel,
                    Timestamp = slot.FirstUploaded == 0 ? TimestampHelper.TimestampMillis : slot.FirstUploaded,
                }
            );
        }

        // Populate with new photos
        foreach (Photo photo in await this.database.Photos.Include(p => p.Creator).ToListAsync())
        {
            this.database.ActivityLog.Add
            (
                new ActivityEntry
                {
                    User = photo.Creator,
                    UserId = photo.CreatorId,
                    RelatedId = photo.PhotoId,
                    Type = EventType.UploadPhoto,
                    Timestamp = photo.Timestamp * 1000,
                }
            );
        }

        // Populate with reviews
        foreach (Review review in await this.database.Reviews.Include(r => r.Reviewer).ToListAsync())
        {
            this.database.ActivityLog.Add
            (
                new ActivityEntry
                {
                    User = review.Reviewer,
                    UserId = review.ReviewerId,
                    RelatedId = review.ReviewId,
                    Type = EventType.ReviewLevel,
                    Timestamp = review.Timestamp,
                }
            );
        }

        // Populate with dpad ratings
        foreach (RatedLevel ratedLevel in await this.database.RatedLevels.Include(r => r.User).ToListAsync())
        {
            this.database.ActivityLog.Add
            (
                new ActivityEntry
                {
                    User = ratedLevel.User,
                    UserId = ratedLevel.UserId,
                    RelatedId = ratedLevel.RatedLevelId,
                    Type = EventType.DpadRateLevel,
                    Timestamp = TimestampHelper.TimestampMillis,
                }
            );
        }

        // Finally, save.
        await this.database.SaveChangesAsync();
    }
    public string Name() => "Populate Activity Log";
    public string Description()
        => "Adds entries to the activity log based on events that have happened in the past. Only do this if you have a completely cleared activity log.";
}