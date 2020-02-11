using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MetroWebApi.Models;

namespace MetroWebApi.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private IConfiguration _config;
        private MetroContext _context;

        public TokenController(IConfiguration config, MetroContext context)
        {
            _context = context;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("/token")]
        public IActionResult Token(Login login)
        {
            var identity = GetIdentity(login);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            // создаем JWT-токен
            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims: identity.Claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            var encodedToken = new JwtSecurityTokenHandler().WriteToken(token);

            var response = new
            {
                access_token = "Bearer " + encodedToken,
                username = identity.Name
            };

            return Json(response);
        }
        private ClaimsIdentity GetIdentity(Login login)
        {
            User localUser = _context.Users.FirstOrDefault(x => x.Email == login.Email && x.Password == login.Password);

            if (localUser != null)
            {
                var claims = new[]
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, localUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, localUser.Role),
                };

                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token",
                                   ClaimsIdentity.DefaultNameClaimType,
                                   ClaimsIdentity.DefaultRoleClaimType);

                return claimsIdentity;
            }

            return null;
        }

    }
}
