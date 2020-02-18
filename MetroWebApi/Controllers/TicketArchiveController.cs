using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MetroWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using MetroWebApi.Services.Interfaces;



namespace MetroWebApi.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class TicketArchiveController : ControllerBase
    {
        private readonly ITicketArchiveService _ticketArciveService;

        public TicketArchiveController(ITicketArchiveService ticketArciveService)
        {
            _ticketArciveService = ticketArciveService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketArchive>>> GetAllTickets()
        {
            try
            {
                var result = await _ticketArciveService.GetAllTicketsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<TicketArchive>>> GetAllTickets(string userId)
        {
            try
            {
                var result = await _ticketArciveService.GetAllTicketsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
        
        [HttpGet("{ticketId}")]
        public async Task<ActionResult<TicketArchive>> GetTicket(int ticketId)
        {
            try
            {
                var result = await _ticketArciveService.GetTicketAsync(ticketId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }


        [HttpPut("{ticketId},{ticket}")]
        public async Task<IActionResult> PutTicket(int ticketId, TicketArchive ticket)
        {
            try
            {
                var result = await _ticketArciveService.PutTicketAsync(ticketId, ticket);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult<TicketArchive>> PostTicket(TicketArchive ticket)
        {
            await _ticketArciveService.PostTicketAsync(ticket);

            return CreatedAtAction(nameof(GetTicket), new { ticketId = ticket.Id }, ticket);
        }

       
        [HttpDelete("{ticketId}")]
        public async Task<ActionResult<TicketArchive>> DeleteTicket(int ticketId)
        {
            try
            {
                var result = await _ticketArciveService.DeleteTicketAsync(ticketId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }           
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<TicketArchive>> DeleteAllTickets(string userId)
        {
            try
            {
                var result = await _ticketArciveService.DeleteAllTicketsAsync(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }       
    }
}