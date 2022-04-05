#nullable enable
using System.Threading.Tasks;
using LBPUnion.ProjectLighthouse.Pages.Layouts;
using LBPUnion.ProjectLighthouse.Types;
using Microsoft.AspNetCore.Mvc;

namespace LBPUnion.ProjectLighthouse.Pages
{
    public class Donation : BaseLayout
    {
            public Donation(Database database) : base(database)
        {}
    }
}
