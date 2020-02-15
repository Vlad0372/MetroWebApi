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
using MetroWebApi.Services.Services;
using MetroWebApi.Services.Interfaces.IServices;


namespace MetroWebApi.Controllers
{
    [Route("[controller]/[action]")]
    //дозволити анонімам
    public class AccountController : Controller
    {
        
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterDto request)
        {
            string token;
            try
            {
                token = await _accountService.RegisterAsync(request);
            }
            catch(ArgumentException ex)
            {
                return BadRequest("Error " + ex.ParamName + ": " + ex.Message);
            }
            
            return Ok(token); 
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto request)
        {
            string token;

            try
            {
                token = await _accountService.LoginAsync(request);
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error " + ex.ParamName + ": " + ex.Message);
            }

            return Ok(token);
        }      
    }
}