using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Models;
using MetroWebApi.Models.Dto;

namespace MetroWebApi.Services.Interfaces
{
    public interface ITicketArchiveService
    {
        Task<IEnumerable<TicketArchive>> GetAllTicketsAsync();
        Task<IEnumerable<TicketArchive>> GetAllTicketsAsync(string userEmail);
        Task<TicketArchive> GetTicketAsync(int ticketId);
        Task<TicketArchive> PutTicketAsync(int ticketId, TicketArchive ticketArchive);
        Task PostTicketAsync(TicketArchive ticketArchive);
        Task<TicketArchive> DeleteTicketAsync(int ticketId);
        Task<IEnumerable<TicketArchive>> DeleteAllTicketsAsync(string userEmail);
    }
}
