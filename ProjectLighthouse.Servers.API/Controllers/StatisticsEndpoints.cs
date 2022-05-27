using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Servers.API.Responses;
using LBPUnion.ProjectLighthouse.Types;
using Microsoft.AspNetCore.Mvc;

namespace LBPUnion.ProjectLighthouse.Servers.API.Controllers;

/// <summary>
/// A collection of endpoints relating to statistics.
/// </summary>
public class StatisticsEndpoints : ApiEndpointController
{
    /// <summary>
    /// Gets everything that StatisticsHelper provides.
    /// </summary>
    /// <returns>An instance of StatisticsResponse</returns>
    [HttpGet("statistics")]
    [ProducesResponseType(typeof(StatisticsResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStatistics()
        => this.Ok
        (
            new StatisticsResponse
            {
                Photos = await StatisticsHelper.PhotoCount(),
                Slots = await StatisticsHelper.SlotCount(),
                Users = await StatisticsHelper.UserCount(),
                RecentMatches = await StatisticsHelper.RecentMatches(),
                TeamPicks = await StatisticsHelper.TeamPickCount(),
            }
        );
}