using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MetroWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Models.Dto;

namespace MetroWebApi.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }
        public async Task<IdentityUser> PostUserAsync(RegisterDto user)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser != null)
            {
                throw new Exception("user with this Email already exist.");
            }

            var newUser = new IdentityUser
            {
                Email = user.Email,
                UserName = user.Email
            };

            await _userManager.CreateAsync(newUser, user.Password);

            return newUser;
        }

        public async Task<IdentityUser> DeleteUserAsync(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                throw new Exception("user with this Id does not exist.");
            }
           
            await _userManager.DeleteAsync(existingUser);

            return existingUser;
        }

        public async Task<IdentityUser> PutUserAsync(string userId, RegisterDto newData)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if(existingUser == null)
            {
                throw new Exception("user with this Id does not exist.");
            }

            if(newData.Email != "")
            {
                existingUser.Email = newData.Email;
            }
            if(newData.Password != "")
            {
                var newPassword = _userManager.PasswordHasher.HashPassword(existingUser, newData.Password);
                existingUser.PasswordHash = newPassword;
            }
            
            var result = await _userManager.UpdateAsync(existingUser);

            if(!result.Succeeded)
            {
                throw new Exception("user data does not changed, check if data are correct.");
            }

            return existingUser;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsersAsync()
        {
            if(await _userManager.Users.CountAsync() == 0)
            {
                throw new Exception("usersList is empty.");
            }

            return await _userManager.Users.ToListAsync();
        }

        public async Task<IdentityUser> GetUserAsync(string userId)
        {
            var existingUser = await _userManager.FindByIdAsync(userId);

            if (existingUser == null)
            {
                throw new Exception("user with this Id does not exist.");
            }
            return existingUser;
        }
    }
}
