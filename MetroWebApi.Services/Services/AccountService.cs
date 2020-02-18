using System;
using MetroWebApi.Services.Interfaces;
using MetroWebApi.Options;
using System.Threading.Tasks;
using MetroWebApi.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;

namespace MetroWebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        

        public AccountService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;            
        }       
        public async Task<string> RegisterAsync(RegisterDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new Exception("user with this email already exist.");
            }

            var newUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);

            await _userManager.AddToRoleAsync(newUser, "User");

            if (!createdUser.Succeeded)
            {
                throw new Exception("user did not created, unknown error.");
            }

            string token = await GenerateJwtTokenAsync(newUser);

            return token;
        }

        public async Task<string> LoginAsync(LoginDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                throw new Exception("user does not exist.");
            }


            var userHasValidPassword = await _userManager.CheckPasswordAsync(existingUser, request.Password);

            if (!userHasValidPassword)
            {
                throw new Exception("wrong login or(and) password.");
            }

            string token = await GenerateJwtTokenAsync(existingUser);

            return token;
        }

        private async Task<string> GenerateJwtTokenAsync(IdentityUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Key);

            var userRoles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("id", user.Id)
            };

            //for multiply user roles
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            //for multiply user roles
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

            return tokenHandler.WriteToken(token);
        }
    }
}
