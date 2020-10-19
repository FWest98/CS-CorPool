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
            if (string.IsNullOrWhiteSpace(login.username) || string.IsNullOrWhiteSpace(login.password))
                return BadRequest();

            // Obtain user
            var user = await UserManager.FindByNameAsync(login.username);
            user ??= await UserManager.FindByEmailAsync(login.username);
            if (user == null)
                return Unauthorized();

            // Check password
            if (!await UserManager.CheckPasswordAsync(user, login.password))
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
                public string username { get; set; }
                public string password { get; set; }
            }

            public class TokenModel {
                public string Key { get; set; }
                public DateTime Expiry { get; set; }
            }
        }
    }
}
