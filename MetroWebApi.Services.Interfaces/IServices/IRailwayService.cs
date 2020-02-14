using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Models;

namespace MetroWebApi.Services.Interfaces.IServices
{
    public interface IRailwayService
    {
        Task<IEnumerable<Railway>> GetAllRailwaysAsync();
        Task<Railway> GetRailwayAsync(int railwayId);
        Task PutRailwayAsync(int railwayId, Railway railway);
        Task<Railway> PostRailwayAsync(Railway railway);
        Task<Railway> DeleteRailwayAsync(int railwayId);
    }
}
