using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MetroWebApi.Services.Interfaces
{
    public interface IRolesService
    {
        Task<IEnumerable<IdentityRole>> GetAllRolesAsync();
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<IdentityRole> PostRoleAsync(string roleName);
        Task<object> PostUserRoleAsync(string roleId, string userId);
        Task<IdentityRole> DeleteRoleAsync(string roleId);
        Task<object> DeleteUserRoleAsync(string roleId, string userId);
    }
}
