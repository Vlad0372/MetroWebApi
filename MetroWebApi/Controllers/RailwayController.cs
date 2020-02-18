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
                var result = await _railwayService.GetAllRailwaysAsync();
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }

        [HttpGet("{railwayId}")]
        public async Task<ActionResult<Railway>> GetRailway(int railwayId)
        {
            try
            {
                var result = await _railwayService.GetRailwayAsync(railwayId);
                return Ok(result);
            }
            catch(Exception ex)
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
                var result = await _railwayService.PutRailwayAsync(railwayId, railway);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }       
      
        [HttpDelete("{railwayId}")]
        public async Task<ActionResult<Railway>> DeleteRailway(int railwayId)
        {
            try
            {
                var result = await _railwayService.DeleteRailwayAsync(railwayId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
        }     
    }
}