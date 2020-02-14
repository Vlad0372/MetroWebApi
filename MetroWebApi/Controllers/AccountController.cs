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
using MetroWebApi.Controllers;

namespace MetroWebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            JwtSettings jwtSettings,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            
            if(existingUser != null)
            {
                return BadRequest("Error: user with this email already exist!");
            }
            
            var newUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };
            
            var createdUser = await _userManager.CreateAsync(newUser, request.Password);
               

            if (!createdUser.Succeeded)
            {
                return BadRequest("Unknown error!");
            }

            string token = await GenerateJwtToken(newUser);

            return Ok(token); 
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                return BadRequest("user does not exist!");               
            }
           

            var userHasValidPassword = await _userManager.CheckPasswordAsync(existingUser, request.Password);

            if (!userHasValidPassword)
            {
                return BadRequest("Error: wrong login or(and) password!");
            }
          
            string token = await GenerateJwtToken(existingUser);

            return Ok(token);
        }
        private async Task <string> GenerateJwtToken(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            var userClaims = await _userManager.GetClaimsAsync(user);

            claims.AddRange(userClaims);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                                                                SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return  tokenHandler.WriteToken(token);
        }
        [HttpPost]
        public async Task<IList<string>> CheckUserRole(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return await _userManager.GetRolesAsync(user);       
        }
    }
}