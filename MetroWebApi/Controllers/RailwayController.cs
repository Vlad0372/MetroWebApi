using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MetroWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using MetroWebApi.Services.Interfaces;

namespace MetroWebApi.Controllers
{
    [Authorize(Roles="Admin")]
    [Route("[controller]/[action]")]
    [ApiController]
    public class RailwayController : ControllerBase
    {
        private readonly IRailwayService _railwayService;       

        public RailwayController(IRailwayService railwayService)
        {
            _railwayService = railwayService;
            
        }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Railway>>> GetAllRailways()
        {
            try
            {
                return Ok(await _railwayService.GetAllRailwaysAsync());
            }
            catch(ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("{railwayId}")]
        public async Task<ActionResult<Railway>> GetRailway(int railwayId)
        {
            try
            {
                return Ok(await _railwayService.GetRailwayAsync(railwayId));
            }
            catch(ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Railway>> PostRailway(Railway railway)
        {
            await _railwayService.PostRailwayAsync(railway);

            return CreatedAtAction(nameof(GetRailway), new { railwayId = railway.Id }, railway);
        }

        [HttpPut("{railwayId}")]
        public async Task<IActionResult> PutRailway(int railwayId, Railway railway)
        {        
            try
            {
                return Ok(await _railwayService.PutRailwayAsync(railwayId, railway));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }       
      
        [HttpDelete("{railwayId}")]
        public async Task<ActionResult<Railway>> DeleteRailway(int railwayId)
        {
            try
            {
                return Ok(await _railwayService.DeleteRailwayAsync(railwayId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }     
    }
}