using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MetroWebApi.Models;

namespace MetroWebApi.Services.Interfaces.IServices
{
    public interface IMetroUser
    {
        Task<IEnumerable<TicketArchive>> GetMyArchiveTicketAsync();
        Task<IEnumerable<Railway>> GetAllRailwaysAsync();
        Task<IEnumerable<Railway>> GetAllRailwaysAsync(string startPoint, string endPoint);
        Task<TicketArchive> GetTicketAsync(int ticketId);
        Task<TicketArchive> BuyTicketAsync(int railwayId);
    }
}
