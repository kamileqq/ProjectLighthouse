#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBPUnion.ProjectLighthouse.Helpers;
using LBPUnion.ProjectLighthouse.Types;
using LBPUnion.ProjectLighthouse.Types.Levels;
using LBPUnion.ProjectLighthouse.Types.Lists;
using LBPUnion.ProjectLighthouse.Types.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Controllers
{
    [ApiController]
    [Route("LITTLEBIGPLANETPS3_XML/")]
    [Produces("text/xml")]
    public class SlotsController : ControllerBase
    {
        private readonly Database database;
        public SlotsController(Database database)
        {
            this.database = database;
        }

        [HttpGet("slots/by")]
        public async Task<IActionResult> SlotsBy([FromQuery] string u, [FromQuery] int pageStart, [FromQuery] int pageSize)
        {
            GameToken? token = await this.database.GameTokenFromRequest(this.Request);
            if (token == null) return this.StatusCode(403, "");

            GameVersion gameVersion = token.GameVersion;
//            GameVersion gameVersion = GameVersion.LittleBigPlanet2;

            User? user = await this.database.Users.FirstOrDefaultAsync(dbUser => dbUser.Username == u);
            if (user == null) return this.NotFound();

            List<Slot> slots = await this.database.Slots.Where(s => s.GameVersion <= gameVersion)
                .Include(s => s.Creator)
                .Include(s => s.Location)
                .Where(s => s.Creator!.Username == user.Username)
                .Skip(pageStart - 1)
                .Take(Math.Min(pageSize, ServerSettings.Instance.EntitledSlots))
                .ToListAsync();

            return this.Ok(new SlotsList(slots, pageStart + Math.Min(pageSize, ServerSettings.Instance.EntitledSlots), user.UsedSlots));
        }

        [HttpGet("s/user/{id:int}")]
        public async Task<IActionResult> SUser(int id)
        {
            User? user = await this.database.UserFromGameRequest(this.Request);
            if (user == null) return this.StatusCode(403, "");

            GameToken? token = await this.database.GameTokenFromRequest(this.Request);
            if (token == null) return this.StatusCode(403, "");

            GameVersion gameVersion = token.GameVersion;

            Slot? slot = await this.database.Slots.Where(s => s.GameVersion <= gameVersion)
                .Include(s => s.Creator)
                .Include(s => s.Location)
                .FirstOrDefaultAsync(s => s.SlotId == id);

            if (slot == null) return this.NotFound();

            //TODO: re-implement
//            RatedLevel? ratedLevel = await this.database.RatedLevels.FirstOrDefaultAsync(r => r.SlotId == id && r.UserId == user.UserId);
//            VisitedLevel? visitedLevel = await this.database.VisitedLevels.FirstOrDefaultAsync(r => r.SlotId == id && r.UserId == user.UserId);
            return this.Ok(slot);
        }

        [HttpGet("slots/lbp2cool")]
        [HttpGet("slots/cool")]
        public async Task<IActionResult> CoolSlots([FromQuery] int page) => await this.LuckyDipSlots(30 * page, 30, 69);

        [HttpGet("slots")]
        public async Task<IActionResult> NewestSlots([FromQuery] int pageStart, [FromQuery] int pageSize)
        {
            GameToken? token = await this.database.GameTokenFromRequest(this.Request);
            if (token == null) return this.StatusCode(403, "");

            GameVersion gameVersion = token.GameVersion;

            List<Slot> slots = await this.database.Slots.Where(s => s.GameVersion <= gameVersion)
                .Include(s => s.Creator)
                .Include(s => s.Location)
                .OrderByDescending(s => s.FirstUploaded)
                .Skip(pageStart - 1)
                .Take(Math.Min(pageSize, 30))
                .ToListAsync();

            return this.Ok(new SlotsList(slots, pageStart + Math.Min(pageSize, ServerSettings.Instance.EntitledSlots), await StatisticsHelper.SlotCount()));
        }

        [HttpGet("slots/mmpicks")]
        public async Task<IActionResult> TeamPickedSlots([FromQuery] int pageStart, [FromQuery] int pageSize)
        {
            GameToken? token = await this.database.GameTokenFromRequest(this.Request);
            if (token == null) return this.StatusCode(403, "");

            GameVersion gameVersion = token.GameVersion;

            List<Slot> slots = await this.database.Slots.Where(s => s.GameVersion <= gameVersion)
                .Where(s => s.TeamPick)
                .Include(s => s.Creator)
                .Include(s => s.Location)
                .OrderByDescending(s => s.LastUpdated)
                .Skip(pageStart - 1)
                .Take(Math.Min(pageSize, 30))
                .ToListAsync();

            return this.Ok(new SlotsList(slots, pageStart + Math.Min(pageSize, ServerSettings.Instance.EntitledSlots), await StatisticsHelper.TeamPickCount()));
        }

        [HttpGet("slots/lbp2luckydip")]
        public async Task<IActionResult> LuckyDipSlots([FromQuery] int pageStart, [FromQuery] int pageSize, [FromQuery] int seed)
        {
            GameToken? token = await this.database.GameTokenFromRequest(this.Request);
            if (token == null) return this.StatusCode(403, "");

            GameVersion gameVersion = token.GameVersion;

            List<Slot> slots = await this.database.Slots.Where(s => s.GameVersion <= gameVersion)
                .Include(s => s.Creator)
                .Include(s => s.Location)
                .OrderBy(_ => EF.Functions.Random())
                .Take(Math.Min(pageSize, 30))
                .ToListAsync();

            return this.Ok(new SlotsList(slots, pageStart + Math.Min(pageSize, ServerSettings.Instance.EntitledSlots), await StatisticsHelper.SlotCount()));
        }
    }
}