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
using MetroWebApi.Models.Dto;
using MetroWebApi.Controllers;
using Microsoft.EntityFrameworkCore.Internal;
using System.ComponentModel.DataAnnotations;

namespace MetroWebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Acco

        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto request)
        {
            

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
        private async Task<string> GenerateJwtToken(IdentityUser user)
        {
            
        }       
    }
}