using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using MetroWebApi.Models;
using MetroWebApi.Services.Interfaces;
using MetroWebApi.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace MetroWebApi.Services
{
    public class MetroUserService : IMetroUserService
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public MetroUserService(ApplicationContext context, UserManager<IdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TicketArchive> BuyTicketAsync(int railwayId)
        {
            var railway = _context.Railways.Where(c => c.Id == railwayId).FirstOrDefault();

            if (railway == null)
            {
                throw new ArgumentException("railway with this ID does not exist.");
            }
            if (railway.FreePlacesAmount > 0)
            {
                railway.FreePlacesAmount = railway.FreePlacesAmount - 1;
                _context.Entry(railway).State = EntityState.Modified;

                var currentId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
                var currentUser = await _userManager.FindByIdAsync(currentId);

                TicketArchive newTicketArchive = new TicketArchive
                {
                    OwnerId = currentUser.Id,
                    StartPoint = railway.StartPoint,
                    EndPoint = railway.EndPoint,
                    DepartureDate = railway.DepartureDate
                };
                await _context.TicketArchives.AddAsync(newTicketArchive);
                await _context.SaveChangesAsync();
                return newTicketArchive;
            }

            throw new ArgumentException("no more tickets.");
        }

        public async Task<IEnumerable<Railway>> GetAllRailwaysAsync()
        {
            var railwayList = await _context.Railways.ToListAsync();

            if (railwayList.Count() == 0)
            {
                throw new ArgumentException("no railways yet.");
            }
            return railwayList;
        }

        public IEnumerable<Railway> GetAllRailways(string startPoint, string endPoint)
        {
            var query = from ways in _context.Railways
                        where ways.StartPoint == startPoint && ways.EndPoint == endPoint
                        select ways;

            if (query.Count() == 0)
            {
                throw new ArgumentException("no railways on this route.");
            }

            return query.ToList();
        }

        public async Task<IEnumerable<TicketArchive>> GetMyTicketArchiveAsync()
        {
            var currentUserId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var currentUser = await _userManager.FindByIdAsync(currentUserId);

            var query = from tickets in _context.TicketArchives
                        where tickets.OwnerId == currentUser.Id
                        select tickets;

            if (query.Count() == 0)
            {
                throw new ArgumentException("ticket archive is empty.");
            }

            return query.ToList();

        }

        public async Task<TicketArchive> GetTicketAsync(int ticketId)
        {
            var ticketArchive = await _context.TicketArchives.FindAsync(ticketId);

            if (ticketArchive == null)
            {
                throw new ArgumentException("the ticket with this ID does not exist.");
            }

            return ticketArchive;
        }
    }
}
