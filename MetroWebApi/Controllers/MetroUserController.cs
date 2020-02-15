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
using MetroWebApi.Services.Services;
using MetroWebApi.Services.Interfaces.IServices;

namespace MetroWebApi.Controllers
{
    [Authorize] //[Authorize(Roles = "User, Admin")]
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
            IEnumerable<TicketArchive> myArchive;

            try
            {
                myArchive = await _metroUserService.GetMyTicketArchiveAsync();
            }
            catch (ArgumentException ex)
            {
                return Ok("error: " + ex.Message);
            }

            return Ok(myArchive);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Railway>>> GetAllRailways()
        {
            IEnumerable<Railway> railwaysList;
            try
            {
                railwaysList = await _metroUserService.GetAllRailwaysAsync();
            }
            catch (ArgumentException ex)
            {
                return Ok("error: " + ex.Message);
            }

            return Ok(railwaysList);
        }

        [HttpGet("railwayId")]
        public async Task<ActionResult<TicketArchive>> BuyTicket(int railwayId)
        {
            TicketArchive ticket;
            try
            {
                ticket = await _metroUserService.BuyTicketAsync(railwayId);
            }
            catch (ArgumentException ex)
            {
                return Ok("error: " + ex.Message);
            }

            return Ok(ticket);
        }


        [HttpGet("{startPoint}, {endPoint}")]
        public ActionResult<IEnumerable<Railway>> GetAllRailways(string startPoint, string endPoint)
        {
            IEnumerable<Railway> railwaysList;

            try
            {
                railwaysList = _metroUserService.GetAllRailwaysAsync(startPoint, endPoint);
            }
            catch (ArgumentException ex)
            {
                return Ok("error: " + ex.Message);
            }

            return Ok(railwaysList);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketArchive>> GetTicket(int id)
        {
            TicketArchive ticket;

            try
            {
                ticket = await _metroUserService.GetTicketAsync(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound("error: " + ex.Message);
            }

            return Ok(ticket);
        }
  
    }
}