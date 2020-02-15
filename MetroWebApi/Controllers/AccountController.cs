using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MetroWebApi.Models.Dto;
using MetroWebApi.Services.Interfaces;


namespace MetroWebApi.Controllers
{
    [Route("[controller]/[action]")]
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
            try
            {
                return Ok(await _accountService.RegisterAsync(request));
            }
            catch(ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto request)
        {
            try
            {
                return Ok(await _accountService.LoginAsync(request));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }      
    }
}