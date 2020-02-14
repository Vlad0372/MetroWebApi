using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MetroWebApi.Models;
using MetroWebApi.Controllers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Authorization;
using MetroWebApi.Entities;



namespace MetroWebApi.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class TicketArchiveController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TicketArchiveController(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketArchive>>> GetAllTickets()
        {
            return await _context.TicketArchives.ToListAsync();

        }

        [HttpGet("{userEmail}")]
        public async Task<ActionResult<IEnumerable<TicketArchive>>> GetAllTickets(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if(user == null)
            {
                return BadRequest("Error: this user does not exist!");
            } 

            var query = from tckt in _context.TicketArchives
                        where tckt.OwnerId == user.Id
                        select tckt;

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


        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicket(int id, TicketArchive ticketArchive)
        {
            if (id != ticketArchive.Id)
            {
                return BadRequest();
            }

            _context.Entry(ticketArchive).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TicketArchives.Any(e => e.Id == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<ActionResult<TicketArchive>> PostTicket(TicketArchive ticketArchive)
        {
            _context.TicketArchives.Add(ticketArchive);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTicket), new { id = ticketArchive.Id }, ticketArchive);
        }

       
        [HttpDelete("{id}")]
        public async Task<ActionResult<TicketArchive>> DeleteTicket(int id)
        {
            var ticketArchive = await _context.TicketArchives.FindAsync(id);
            if (ticketArchive == null)
            {
                return NotFound();
            }

            _context.TicketArchives.Remove(ticketArchive);
            await _context.SaveChangesAsync();

            return Ok("Successfully deleted!");
        }

        [HttpDelete("{userEmail}")]
        public async Task<ActionResult<TicketArchive>> DeleteAllTickets(string userEmail)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return BadRequest("Error: this user does not exist!");
            }

            var all = _context.TicketArchives.Where(t => t.OwnerId == user.Id); 
            _context.TicketArchives.RemoveRange(all);
            _context.SaveChanges();

            if (all == null)
            {
                return NotFound("Ticket archive is empty!");
            }       

            return Ok("Successfully deleted!");
        }
        private bool TicketExists(int id)
        {
            return _context.TicketArchives.Any(e => e.Id == id);
        }
    }
}