using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using LBPUnion.ProjectLighthouse.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types.Activity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Pages.Debug;

public class ActivityLogPage : BaseLayout
{
    public ActivityLogPage([NotNull] Database database) : base(database)
    {}

    public List<ActivityEntry> Entries;

    public async Task<IActionResult> OnGet()
    {
        #if !DEBUG
        return this.NotFound();
        #endif

        this.Entries = await this.Database.ActivityLog.OrderByDescending(e => e.Timestamp).ToListAsync();

        return this.Page();
    }
}