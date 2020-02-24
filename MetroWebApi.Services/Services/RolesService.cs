using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MetroWebApi.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public RolesService(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            if(await _roleManager.Roles.CountAsync() == 0)
            {
                throw new Exception("no roles yet.");
            }

            var result = await _roleManager.Roles.ToListAsync();

            return result;
        }

        public async Task<IList<string>> GetUserRolesAsync(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if(existingUser == null)
            {
                throw new Exception("user with this Id does not exist.");
            }

            var userRoles = await _userManager.GetRolesAsync(existingUser);

            return userRoles;
        }

        public async Task<IdentityRole> PostRoleAsync(string roleName)
        {
            if(await _roleManager.FindByNameAsync(roleName) != null)
            {
                throw new Exception("this role already exist.");
            }

            var newRole = new IdentityRole(roleName);
            await _roleManager.CreateAsync(newRole);

            return newRole;    
        }

        public async Task<object> PostUserRoleAsync(string roleId, string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                throw new Exception("role with this Id does not exist.");
            }
            if (existingUser == null)
            {
                throw new Exception("user with this Id does not exist.");
            }

            await _userManager.AddToRoleAsync(existingUser, role.Name);

            var response = new
            {              
                user_email = existingUser.Email,
                added_role = role.Name
            };
            return response;
        }
     
        public async Task<IdentityRole> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if(role == null)
            {
                throw new Exception("role with this Id does not exist.");
            }

            await _roleManager.DeleteAsync(role);

            return role;
        }

        public async Task<object> DeleteUserRoleAsync(string roleId, string userId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            var existingUser = await _userManager.FindByIdAsync(userId);

            if (role == null)
            {
                throw new Exception("role with this Id does not exist.");
            }
            if (existingUser == null)
            {
                throw new Exception("user with this Id does not exist.");
            }

            await _userManager.RemoveFromRoleAsync(existingUser, role.Name);

            var response = new
            {
                user_email = existingUser.Email,
                removed_role = role.Name
            };
            return response;
        }        
    }
}
