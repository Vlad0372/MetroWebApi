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
using MetroWebApi.Services.Interfaces;
using MetroWebApi.Models.Dto;



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
                return Ok(await _ticketArciveService.GetAllTicketsAsync());
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<TicketArchive>>> GetAllTickets(string userId)
        {
            try
            {
                return Ok(await _ticketArciveService.GetAllTicketsAsync(userId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }
        
        [HttpGet("{ticketId}")]
        public async Task<ActionResult<TicketArchive>> GetTicket(int ticketId)
        {
            try
            {
                return Ok(await _ticketArciveService.GetTicketAsync(ticketId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }


        [HttpPut("{ticketId},{ticket}")]
        public async Task<IActionResult> PutTicket(int ticketId, TicketArchive ticket)
        {
            try
            {
                return Ok(await _ticketArciveService.PutTicketAsync(ticketId, ticket));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }


        [HttpPost]
        public async Task<ActionResult<TicketArchive>> PostTicket(TicketArchive ticket)
        {
            await _ticketArciveService.PostTicketAsync(ticket);

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

       
        [HttpDelete("{ticketId}")]
        public async Task<ActionResult<TicketArchive>> DeleteTicket(int ticketId)
        {
            try
            {
                return Ok(await _ticketArciveService.DeleteTicketAsync(ticketId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }           
        }

        [HttpDelete("{userId}")]
        public async Task<ActionResult<TicketArchive>> DeleteAllTickets(string userId)
        {
            try
            {
                return Ok(await _ticketArciveService.DeleteAllTicketsAsync(userId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }       
    }
}