using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MetroWebApi.Models.Dto;
using MetroWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace MetroWebApi.Controllers
{
    [Route("[controller]/[action]")]
    public class AccountController : ControllerBase
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
                var result = await _accountService.RegisterAsync(request);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginDto request)
        {
            try
            {
                var result = await _accountService.LoginAsync(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

    }
}