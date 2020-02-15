using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Models;

namespace MetroWebApi.Services.Interfaces
{
    public interface IRailwayService
    {
        Task<IEnumerable<Railway>> GetAllRailwaysAsync();
        Task<Railway> GetRailwayAsync(int railwayId);
        Task<Railway> PutRailwayAsync(int railwayId, Railway railway);
        Task PostRailwayAsync(Railway railway);
        Task<Railway> DeleteRailwayAsync(int railwayId);
    }
}
