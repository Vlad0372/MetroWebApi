using System.Threading.Tasks;
using MetroWebApi.Models.Dto;
using Microsoft.AspNetCore.Identity;

namespace MetroWebApi.Services.Interfaces.IServices
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto request);
        Task<string> LoginAsync(LoginDto request);
        Task<string> GenerateJwtTokenAsync(IdentityUser user);

    }
}
