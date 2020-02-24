using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MetroWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace MetroWebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<IdentityRole>>> GetAllRoles()
        {
            try
            {
                var result = await _rolesService.GetAllRolesAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IList<string>>> GetUserRoles(string userId)
        {
            try
            {
                var result = await _rolesService.GetUserRolesAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpPost("{roleName}")]
        public async Task<ActionResult<IdentityRole>> PostRole(string roleName)
        {
            try
            {
                var result = await _rolesService.PostRoleAsync(roleName);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost("{roleId}, {userId}")]
        public async Task<ActionResult<object>> PostUserRole(string roleId, string userId)
        {
            try
            {
                var result = await _rolesService.PostUserRoleAsync(roleId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpDelete("{roleId}")]
        public async Task<ActionResult<IdentityRole>> DeleteRole(string roleId)
        {
            try
            {
                var result = await _rolesService.DeleteRoleAsync(roleId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpDelete("{roleId}, {userId}")]
        public async Task<ActionResult<object>> DeleteUserRole(string roleId, string userId)
        {
            try
            {
                var result = await _rolesService.DeleteUserRoleAsync(roleId, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }
    }
}