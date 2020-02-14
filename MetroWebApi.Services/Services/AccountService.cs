using System;
using MetroWebApi.Services.Interfaces.IServices;
using Microsoft.AspNetCore.Identity;
using MetroWebApi.Options;
using System.Threading.Tasks;
using MetroWebApi.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace MetroWebApi.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        

        public AccountService(UserManager<IdentityUser> userManager,                         
                              JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;            
        }       
        public async Task<string> RegisterAsync(RegisterDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ArgumentException("user with this email already exist.", "400");
            }

            var newUser = new IdentityUser
            {
                Email = request.Email,
                UserName = request.Email
            };

            var createdUser = await _userManager.CreateAsync(newUser, request.Password);


            if (!createdUser.Succeeded)
            {
                throw new ArgumentException("user did not created, unknown error.");
            }

            string token = await GenerateJwtTokenAsync(newUser);

            return token;
        }

        public async Task<string> LoginAsync(LoginDto request)
        {
            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser == null)
            {
                throw new ArgumentException("user does not exist.", "400");
            }


            var userHasValidPassword = await _userManager.CheckPasswordAsync(existingUser, request.Password);

            if (!userHasValidPassword)
            {
                throw new ArgumentException("wrong login or(and) password.", "400");
            }

            string token = await GenerateJwtTokenAsync(existingUser);

            return token;
        }

        public async Task<string> GenerateJwtTokenAsync(IdentityUser user)
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
            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

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
