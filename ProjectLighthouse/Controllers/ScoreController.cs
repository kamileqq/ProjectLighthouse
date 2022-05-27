#nullable enable
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using LBPUnion.ProjectLighthouse.Types;
using LBPUnion.ProjectLighthouse.Types.Levels;
using LBPUnion.ProjectLighthouse.Types.Lists;
using Microsoft.AspNetCore.Mvc;

namespace LBPUnion.ProjectLighthouse.Controllers
{
    [ApiController]
    [Route("LITTLEBIGPLANETPS3_XML/")]
    [Produces("text/xml")]
    public class ScoreController : ControllerBase
    {
        private readonly Database database;

        public ScoreController(Database database)
        {
            this.database = database;
        }

        [HttpPost("scoreboard/user/{id:int}")]
        public async Task<IActionResult> SubmitScore(int id, [FromQuery] bool lbp1 = false, [FromQuery] bool lbp2 = false, [FromQuery] bool lbp3 = false)
        {
            (User, GameToken)? userAndToken = await this.database.UserAndGameTokenFromRequest(this.Request);

            if (userAndToken == null) return this.StatusCode(403, "");

            // ReSharper disable once PossibleInvalidOperationException
            User user = userAndToken.Value.Item1;
            GameToken gameToken = userAndToken.Value.Item2;

            this.Request.Body.Position = 0;
            string bodyString = await new StreamReader(this.Request.Body).ReadToEndAsync();

            XmlSerializer serializer = new(typeof(Score));
            Score? score = (Score?)serializer.Deserialize(new StringReader(bodyString));
            if (score == null) return this.BadRequest();

            score.SlotId = id;

            Slot? slot = this.database.Slots.FirstOrDefault(s => s.SlotId == score.SlotId);
            if (slot == null) return this.BadRequest();

            switch (gameToken.GameVersion)
            {
                case GameVersion.LittleBigPlanet1:
                    slot.PlaysLBP1Complete++;
                    break;
                case GameVersion.LittleBigPlanet2:
                    slot.PlaysLBP2Complete++;
                    break;
                case GameVersion.LittleBigPlanet3:
                    slot.PlaysLBP3Complete++;
                    break;
                case GameVersion.LittleBigPlanetVita:
                    slot.PlaysLBPVitaComplete++;
                    break;
            }

            IQueryable<Score> existingScore = this.database.Scores.Where(s => s.SlotId == score.SlotId && s.PlayerIdCollection == score.PlayerIdCollection);

            if (existingScore.Any())
            {
                Score first = existingScore.First(s => s.SlotId == score.SlotId);
                score.ScoreId = first.ScoreId;
                score.Points = Math.Max(first.Points, score.Points);
                this.database.Entry(first).CurrentValues.SetValues(score);
            }
            else
            {
                this.database.Scores.Add(score);
            }

            await this.database.SaveChangesAsync();

            return this.Ok(this.GetScores(score.SlotId, score.Type, user));
        }

        [HttpGet("friendscores/user/{slotId:int}/{type:int}")]
        public IActionResult FriendScores(int slotId, int type)
            //=> await TopScores(slotId, type);
            => this.Ok(new ScoreList(new List<Score>()));

        [HttpGet("topscores/user/{slotId:int}/{type:int}")]
        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public async Task<IActionResult> TopScores(int slotId, int type, [FromQuery] int pageStart = -1, [FromQuery] int pageSize = 5)
        {
            // Get username
            User? user = await this.database.UserFromGameRequest(this.Request);

            if (user == null) return this.StatusCode(403, "");

            return this.Ok(this.GetScores(slotId, type, user, pageStart, pageSize));
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public ScoreList GetScores(int slotId, int type, User user, int pageStart = -1, int pageSize = 5)
        {
            // This is hella ugly but it technically assigns the proper rank to a score
            // var needed for Anonymous type returned from SELECT
            var rankedScores = this.database.Scores.Where(s => s.SlotId == slotId && s.Type == type)
                .OrderByDescending(s => s.Points)
                .ToList()
                .Select
                (
                    (s, rank) => new
                    {
                        Score = s,
                        Rank = rank + 1,
                    }
                );

            // Find your score, since even if you aren't in the top list your score is pinned
            var myScore = rankedScores.Where
                    (rs => rs.Score.PlayerIdCollection.Contains(user.Username))
                .OrderByDescending(rs => rs.Score.Points)
                .FirstOrDefault();

            // Paginated viewing: if not requesting pageStart, get results around user
            var pagedScores = rankedScores.Skip(pageStart != -1 || myScore == null ? pageStart - 1 : myScore.Rank - 3).Take(Math.Min(pageSize, 30));

            foreach (var rs in pagedScores) rs.Score.Rank = rs.Rank;

            ScoreList scoreList = myScore == null
                ? new ScoreList(pagedScores.Select(rs => rs.Score).ToList())
                : new ScoreListWithPlayerScore(pagedScores.Select(rs => rs.Score).ToList(), myScore.Score.Points, myScore.Rank, rankedScores.Count());

            return scoreList;
        }
    }
}