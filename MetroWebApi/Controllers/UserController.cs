using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetroWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using MetroWebApi.Models.Dto;

namespace MetroWebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("{user}")]
        public async Task<ActionResult<IdentityUser>> PostUser(RegisterDto user)
        {
            try
            {
                var result = await _userService.CreateUserAsync(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<IdentityUser>> DeleteUser(string userId)
        {
            try
            {
                var result = await _userService.DeleteUserAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpPut("{userId}, {newData}")]
        public async Task<ActionResult<IdentityUser>> PutUser(string userId, RegisterDto newData)
        {
            try
            {
                var result = await _userService.EditUserAsync(userId, newData);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityUser>>> GetAllUsers()
        {
            try
            {
                var result = await _userService.GetAllUsersAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IdentityUser>> GetUser(string userId)
        {
            try
            {
                var result = await _userService.GetUserAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

    }
}
