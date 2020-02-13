using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MetroWebApi.Models;
using MetroWebApi.Options;

namespace MetroWebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            JwtSettings jwtSettings
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
        }

        [HttpPost]
        public async Task<string> Register([FromBody]RegisterDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if(existingUser != null)
            {
                return "Error: user with this email already exist!";
            }
            
            var newUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };
            
            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

        
            await _userManager.AddToRoleAsync(newUser, "Admin");

            if (!createdUser.Succeeded)
            {
                return "Unknown error!";
            }

            string token = GenerateJwtToken(newUser);

            return token; 
        }

        [HttpPost]
        public async Task<string> Login([FromBody]LoginDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                return "user does not exist!";               
            }
           

            var userHasValidPassword = await _userManager.CheckPasswordAsync(existingUser, request.Password);

            if (!userHasValidPassword)
            {
                return "Error: wrong login or(and) password!";
            }

            string token = GenerateJwtToken(existingUser);

            return token;
        }
        private string GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)

                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return  tokenHandler.WriteToken(token);
        }      
    }
}