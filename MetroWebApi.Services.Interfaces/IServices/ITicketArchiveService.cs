using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Models;


namespace MetroWebApi.Services.Interfaces.IServices
{
    public interface ITicketArchiveService
    {
        Task<IEnumerable<TicketArchive>> GetAllTicketsAsync();
        Task<IEnumerable<TicketArchive>> GetAllTicketsAsync(string userEmail);
        Task<TicketArchive> GetTicketAsync(int ticketId);
        Task PutTicketAsync(int ticketId, TicketArchive ticketArchive);
        Task<TicketArchive> PostTicketAsync(TicketArchive ticketArchive);
        Task<TicketArchive> DeleteTicketAsync(int ticketId);
        Task<TicketArchive> DeleteAllTicketsAsync(string userEmail);
    }
}
