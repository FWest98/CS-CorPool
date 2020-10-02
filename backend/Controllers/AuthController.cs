using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CorPool.BackEnd.Attributes;
using CorPool.BackEnd.DatabaseModels;
using CorPool.BackEnd.Helpers.Jwt;
using CorPool.BackEnd.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CorPool.BackEnd.Controllers {
    [Tenanted]
    public class AuthController : AbstractApiController {
        private readonly IOptionsSnapshot<AuthenticationOptions> _authOptions;
        private readonly JwtUserManager _userManager;

        public AuthController(DatabaseContext database, IOptionsSnapshot<AuthenticationOptions> authOptions, JwtUserManager userManager) : base(database) {
            _authOptions = authOptions;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<User> Get() {
            // Get current user
            return await _userManager.GetUserAsync(User);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Models.LoginModel login) {
            if (string.IsNullOrWhiteSpace(login.username) || string.IsNullOrWhiteSpace(login.password))
                return BadRequest();

            // Obtain user
            var user = await _userManager.FindByNameAsync(login.username);
            user ??= await _userManager.FindByEmailAsync(login.username);
            if (user == null)
                return Unauthorized();

            // Check password
            if (!await _userManager.CheckPasswordAsync(user, login.password))
                return Unauthorized();

            // Authorized, return JWT
            return Ok(await _userManager.GenerateJwtToken(user));
        }

        public class Models {
            public class LoginModel {
                public string username { get; set; }
                public string password { get; set; }
            }
        }
    }
}
