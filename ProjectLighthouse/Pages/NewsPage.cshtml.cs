using System.Threading.Tasks;
using JetBrains.Annotations;
using LBPUnion.ProjectLighthouse.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types.News;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Pages;

public class NewsPage : BaseLayout
{
    public NewsPage([NotNull] Database database) : base(database)
    {}

    public NewsEntry NewsEntry;

    public async Task<IActionResult> OnGet([FromRoute] int id)
    {
        this.NewsEntry = await this.Database.NewsEntries.FirstOrDefaultAsync(n => n.NewsId == id);
        if (this.NewsEntry == null) return this.NotFound();

        return this.Page();
    }
}