using System.Threading.Tasks;
using MetroWebApi.Models.Dto;

namespace MetroWebApi.Services.Interfaces
{
    public interface IAccountService
    {
        Task<string> RegisterAsync(RegisterDto request);
        Task<string> LoginAsync(LoginDto request);
    }
}
