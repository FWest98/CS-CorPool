using System;
using System.Threading.Tasks;
using Corpool.AspNetCoreTenant;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.Mongo.DatabaseModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    public class AuthController : AbstractApiController {
        public AuthController(Lazy<DatabaseContext> database, Lazy<JwtUserManager> userManager, Lazy<IDistributedCache> cache) : base(database, userManager, cache) { }

        [Authorize]
        public async Task<User> Get() {
            // Get current user
            return await User;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.LoginModel login) {
            if (string.IsNullOrWhiteSpace(login.Username) || string.IsNullOrWhiteSpace(login.Password))
                return BadRequest();

            // Obtain user
            var user = await UserManager.FindByNameAsync(login.Username);
            user ??= await UserManager.FindByEmailAsync(login.Username);
            if (user == null)
                return Unauthorized();

            // Check password
            if (!await UserManager.CheckPasswordAsync(user, login.Password))
                return Unauthorized();

            // Authorized, return JWT
            var (token, expiry) = await UserManager.GenerateJwtToken(user);
            return Ok(new Models.TokenModel {
                Key = token,
                Expiry = expiry
            });
        }

        public class Models {
            public class LoginModel {
                public string Username { get; set; }
                public string Password { get; set; }
            }

            public class TokenModel {
                public string Key { get; set; }
                public DateTime Expiry { get; set; }
            }
        }
    }
}
