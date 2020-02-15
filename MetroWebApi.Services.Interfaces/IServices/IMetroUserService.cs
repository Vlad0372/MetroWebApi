using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Models;

namespace MetroWebApi.Services.Interfaces
{
    public interface IMetroUserService
    {
        Task<IEnumerable<TicketArchive>> GetMyTicketArchiveAsync();
        Task<IEnumerable<Railway>> GetAllRailwaysAsync();
        IEnumerable<Railway> GetAllRailways(string startPoint, string endPoint);
        Task<TicketArchive> GetTicketAsync(int ticketId);
        Task<TicketArchive> BuyTicketAsync(int railwayId);
    }
}
