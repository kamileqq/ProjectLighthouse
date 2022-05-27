#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LBPUnion.ProjectLighthouse.Types;
using LBPUnion.ProjectLighthouse.Types.Lists;
using LBPUnion.ProjectLighthouse.Types.Profiles;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace LBPUnion.ProjectLighthouse.Controllers
{
    [Route("LITTLEBIGPLANETPS3_XML/")]
    [Produces("text/xml")]
    public class UserController : ControllerBase
    {
        private readonly Database database;

        public UserController(Database database)
        {
            this.database = database;
        }

        public async Task<User?> GetSerializedUser(string username)
            => await this.database.Users.Include(u => u.Location).FirstOrDefaultAsync(u => u.Username == username);

        [HttpGet("user/{username}")]
        public async Task<IActionResult> GetUser(string username)
        {
//            GameToken? token = await this.database.GameTokenFromRequest(this.Request);
//            if (token == null) return this.StatusCode(403, "");

            User? user = await this.GetSerializedUser(username);
            if (user == null) return this.NotFound();

            return this.Ok(user);
        }

        [HttpGet("users")]
        public async Task<IActionResult> GetUserAlt([FromQuery] string[] u)
        {
            GameToken? token = await this.database.GameTokenFromRequest(this.Request);
            if (token == null) return this.StatusCode(403, "");

            List<User> serializedUsers = new();
            foreach (string username in u)
            {
                User? user = await this.GetSerializedUser(username);
                if (user == null) continue;

                serializedUsers.Add(user);
            }
            return this.Ok(new UsersList(serializedUsers));
        }

        [HttpGet("user/{username}/playlists")]
        public IActionResult GetUserPlaylists(string username) => this.Ok();

        // example request for changing profile card location:
        // <updateUser>
        //     <location>
        //         <x>1234</x>
        //         <y>1234</y>
        //     </location>
        // </updateUser>
        //
        // example request for changing biography:
        // <updateUser>
        //     <biography>biography stuff</biography>
        // </updateUser>
        [HttpPost("updateUser")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUser? updateUser)
        {
            #if DEBUG
            IEnumerable<ModelError> errors = this.ModelState.Values.SelectMany(v => v.Errors);
            Console.WriteLine(JsonSerializer.Serialize(errors));
            #endif

            User? user = await this.database.UserFromGameRequest(this.Request);
            if (user == null) return this.StatusCode(403, "");

            if (updateUser == null) return this.BadRequest();

            if (updateUser.PlanetHash != null) user.PlanetHash = updateUser.PlanetHash;
            if (updateUser.Biography != null) user.Biography = updateUser.Biography;
            if (updateUser.IconHash != null) user.IconHash = updateUser.IconHash;
            if (updateUser.YayHash != null) user.YayHash = updateUser.YayHash;
            if (updateUser.BooHash != null) user.BooHash = updateUser.BooHash;
            if (updateUser.MehHash != null) user.MehHash = updateUser.MehHash;

            if (updateUser.Location != null)
            {
                Location? l = await this.database.Locations.FirstOrDefaultAsync(l => l.Id == user.LocationId);

                if (l == null)
                    throw new Exception
                        ("This should never happen." + "If it did, it's because you created a user in the database manually without creating a Location.");

                l.X = updateUser.Location.X;
                l.Y = updateUser.Location.Y;
            }

            await this.database.SaveChangesAsync();
            return this.Ok();
        }

        [HttpPost("update_my_pins")]
        public async Task<IActionResult> UpdateMyPins()
        {
            User? user = await this.database.UserFromGameRequest(this.Request);
            if (user == null) return this.StatusCode(403, "");

            string pinsString = await new StreamReader(this.Request.Body).ReadToEndAsync();
            Pins? pinJson = JsonSerializer.Deserialize<Pins>(pinsString);
            if (pinJson == null) return this.BadRequest();

            // Sometimes the update gets called periodically as pin progress updates via playing,
            // may not affect equipped profile pins however, so check before setting it.
            string currentPins = user.Pins;
            string newPins = string.Join(",", pinJson.ProfilePins);

            if (string.Equals(currentPins, newPins)) return this.Ok("[{\"StatusCode\":200}]");

            user.Pins = newPins;
            await this.database.SaveChangesAsync();

            return this.Ok("[{\"StatusCode\":200}]");
        }
    }
}