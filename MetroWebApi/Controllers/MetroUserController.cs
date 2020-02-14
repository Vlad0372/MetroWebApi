using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetroWebApi.Entities;
using MetroWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MetroWebApi.Controllers
{
    [Authorize] //[Authorize(Roles = "User, Admin")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class MetroUserController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public MetroUserController(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketArchive>>> GetMyArchiveTickets()
        {
            var currentId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
            var currentUser = await _userManager.FindByIdAsync(currentId);

            var query = from tickets in _context.TicketArchives
                        where tickets.OwnerId == currentUser.Id
                        select tickets;

            return query.ToList();         
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Railway>>> GetAllRailways()
        {         
            return await _context.Railways.ToListAsync();
        }
        [HttpGet("{startPoint}, {endPoint}")]
        public Task<ActionResult<IEnumerable<Railway>>> GetAllRailways(string startPoint, string endPoint)
        {
            var query = from ways in _context.Railways
                        where ways.StartPoint == startPoint && ways.EndPoint == endPoint
                        select ways;

            return query.ToList();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketArchive>> GetTicket(int id)
        {
            var ticketArchive = await _context.TicketArchives.FindAsync(id);

            if (ticketArchive == null)
            {
                return NotFound();
            }

            return ticketArchive;
        }

        [HttpGet("railwayId")]
        public async Task<IActionResult<TicketArchive>> BuyTicket(int railwayId)
        {
            // var railway = await _context.Railways.FindAsync(railwayId);
            var railway =  _context.Railways.Where(c => c.Id == railwayId).FirstOrDefault();


            if (railway == null)
            {
                return NotFound("Error: railway with this ID does not exist!");
            }
            if (railway.FreePlacesAmount > 0)
            {
                railway.FreePlacesAmount = railway.FreePlacesAmount - 1;
                _context.Entry(railway).State = EntityState.Modified;

                var currentId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;
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
                return Ok(newTicketArchive);
            }
            
                return NotFound("Error: no more tickets!");
                                          
        }
   
    }
}