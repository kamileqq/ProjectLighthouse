using System.Threading.Tasks;
using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Types;
using Microsoft.AspNetCore.Mvc;

namespace LBPUnion.ProjectLighthouse.Controllers
{
    [ApiController]
    [Route("LITTLEBIGPLANETPS3_XML/")]
    [Produces("text/plain")]
    public class StatisticsController : ControllerBase
    {
        [HttpGet("playersInPodCount")]
        [HttpGet("totalPlayerCount")]
        public async Task<IActionResult> TotalPlayerCount() => this.Ok((await StatisticsHelper.RecentMatches()).ToString());

        [HttpGet("planetStats")]
        public async Task<IActionResult> PlanetStats() => this.Ok(new PlanetStats(await StatisticsHelper.SlotCount(), await StatisticsHelper.TeamPickCount()));

        [HttpGet("planetStats/totalLevelCount")]
        public async Task<IActionResult> TotalLevelCount() => this.Ok((await StatisticsHelper.SlotCount()).ToString());
    }
}