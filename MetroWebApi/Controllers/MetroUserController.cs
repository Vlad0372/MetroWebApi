using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MetroWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MetroWebApi.Services.Interfaces;

namespace MetroWebApi.Controllers
{
    [Authorize(Roles = "User, Admin")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class MetroUserController : ControllerBase
    {
        private readonly IMetroUserService _metroUserService;
        
        public MetroUserController(IMetroUserService metroUserService)
        {
            _metroUserService = metroUserService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TicketArchive>>> GetMyTicketArchive()
        {
            try
            {
                return Ok(await _metroUserService.GetMyTicketArchiveAsync());
            }
            catch (ArgumentException ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Railway>>> GetAllRailways()
        {
            try
            {
                return Ok(await _metroUserService.GetAllRailwaysAsync());
            }
            catch (ArgumentException ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet("railwayId")]
        public async Task<ActionResult<TicketArchive>> BuyTicket(int railwayId)
        {
            try
            {
                return Ok(await _metroUserService.BuyTicketAsync(railwayId));
            }
            catch (ArgumentException ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet("{startPoint}, {endPoint}")]
        public ActionResult<IEnumerable<Railway>> GetAllRailways(string startPoint, string endPoint)
        {
            try
            {
                return Ok (_metroUserService.GetAllRailways(startPoint, endPoint));
            }
            catch (ArgumentException ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet("{ticketId}")]
        public async Task<ActionResult<TicketArchive>> GetTicket(int ticketId)
        {
            try
            {
                return Ok(await _metroUserService.GetTicketAsync(ticketId));
            }
            catch (ArgumentException ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }
    }
}