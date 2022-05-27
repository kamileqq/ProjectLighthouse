using System.Threading.Tasks;

namespace LBPUnion.ProjectLighthouse.Maintenance.MaintenanceJobs;

public class ClearActivityLogMaintenanceJob : IMaintenanceJob
{
    private readonly Database database = new();
    public async Task Run()
    {
        this.database.ActivityLog.RemoveRange(this.database.ActivityLog);
        await this.database.SaveChangesAsync();
    }
    public string Name() => "Clear Activity Log";
    public string Description() => "Deletes ALL entries in the Activity Log. Some events may be lost if you choose to clear and repopulate.";
}