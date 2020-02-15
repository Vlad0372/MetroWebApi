using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Models;

namespace MetroWebApi.Services.Interfaces.IServices
{
    public interface IMetroUserService
    {
        Task<IEnumerable<TicketArchive>> GetMyTicketArchiveAsync();
        Task<IEnumerable<Railway>> GetAllRailwaysAsync();
        IEnumerable<Railway> GetAllRailwaysAsync(string startPoint, string endPoint);
        Task<TicketArchive> GetTicketAsync(int ticketId);
        Task<TicketArchive> BuyTicketAsync(int railwayId);
    }
}
