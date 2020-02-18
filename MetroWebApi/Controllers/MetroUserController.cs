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
                var result = await _metroUserService.GetMyTicketArchiveAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Railway>>> GetAllRailways()
        {
            try
            {
                var result = await _metroUserService.GetAllRailwaysAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet("railwayId")]
        public async Task<ActionResult<TicketArchive>> BuyTicket(int railwayId)
        {
            try
            {
                var result = await _metroUserService.BuyTicketAsync(railwayId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet("{startPoint}, {endPoint}")]
        public ActionResult<IEnumerable<Railway>> GetAllRailways(string startPoint, string endPoint)
        {
            try
            {
                var result = _metroUserService.GetAllRailways(startPoint, endPoint);
                return Ok (result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }

        [HttpGet("{ticketId}")]
        public async Task<ActionResult<TicketArchive>> GetTicket(int ticketId)
        {
            try
            {
                var result = await _metroUserService.GetTicketAsync(ticketId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound("Error: " + ex.Message);
            }
        }
    }
}