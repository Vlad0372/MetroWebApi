using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MetroWebApi.Models.Dto;

namespace MetroWebApi.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityUser> PostUserAsync(RegisterDto user);
        Task<IdentityUser> PutUserAsync(string userId, RegisterDto newData);
        Task<IdentityUser> GetUserAsync(string userId);
        Task<IEnumerable<IdentityUser>> GetAllUsersAsync();
        Task<IdentityUser> DeleteUserAsync(string userId);

    }
}
