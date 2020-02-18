using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Models;
using Microsoft.AspNetCore.Identity;
using MetroWebApi.Entities;
using MetroWebApi.Services.Interfaces;

namespace MetroWebApi.Services
{
    public class TicketArchiveService : ITicketArchiveService
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketArchiveService(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
             
        public async Task<IEnumerable<TicketArchive>> GetAllTicketsAsync()
        {
            var ticketList = await _context.TicketArchives.ToListAsync();

            if (ticketList.Count() == 0)
            {
                throw new Exception("no tickets yet.");
            }

            return ticketList;
        }

        public async Task<IEnumerable<TicketArchive>> GetAllTicketsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                throw new Exception("user with this Id does not exist.");
            }

            var query = from ticket in _context.TicketArchives
                        where ticket.OwnerId == user.Id
                        select ticket;

            return query;
        }

        public async Task<TicketArchive> GetTicketAsync(int ticketId)
        {
            var ticket = await _context.TicketArchives.FindAsync(ticketId);

            if (ticket == null)
            {
                throw new Exception("ticket with this Id does not exist.");
            }

            return ticket;
        }

        public async Task PostTicketAsync(TicketArchive ticketArchive)
        {
            _context.TicketArchives.Add(ticketArchive);
            await _context.SaveChangesAsync();
        }
   
        public async Task<TicketArchive> PutTicketAsync(int ticketId, TicketArchive ticketArchive)
        {
            if (ticketId != ticketArchive.Id)
            {
                throw new Exception("ticketId is not equal to ticketArchive Id.");
            }

            //_context.Entry(ticketArchive).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (! _context.TicketArchives.Any(e => e.Id == ticketId))
                {
                    throw new Exception("ticket with this Id does not exist.");
                }
                else
                {
                    throw;
                }
            }
            return ticketArchive;
        }
       
        public async Task<TicketArchive> DeleteTicketAsync(int ticketId)
        {
            var ticket = await _context.TicketArchives.FindAsync(ticketId);

            if (ticket == null)
            {
                throw new Exception("ticket with this Id not exist.");
            }

            _context.TicketArchives.Remove(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }
        public async Task<IEnumerable<TicketArchive>> DeleteAllTicketsAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new Exception("user with this id does not exist.");
            }

            var allTickets = _context.TicketArchives.Where(t => t.OwnerId == user.Id);
            _context.TicketArchives.RemoveRange(allTickets);
            _context.SaveChanges();

            if (allTickets == null)
            {
                throw new Exception("this user ticket archive is empty.");
            }

            return allTickets;
        }     
    }
}
