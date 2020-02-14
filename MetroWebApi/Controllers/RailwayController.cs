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
    [Route("[controller]/[action]")]
    [ApiController]
    public class RailwayController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RailwayController(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
       
        //[Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Railway>>> GetAllRailways()
        {
            return await _context.Railways.ToListAsync();

        }

        //[Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Railway>> GetRailway(int id)
        {
            var railway = await _context.Railways.FindAsync(id);

            if (railway == null)
            {
                return NotFound();
            }

            return railway;
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutRailway(int id, Railway railway)
        {
            if (id != railway.Id)
            {
                return BadRequest();
            }

            _context.Entry(railway).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RailwayExists(id))
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
        public async Task<ActionResult<Railway>> PostRailway(Railway railway)
        {
            _context.Railways.Add(railway);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRailway), new { id = railway.Id }, railway);
        }
      
        //[Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<Railway>> DeleteRailway(int id)
        {
            var railway = await _context.Railways.FindAsync(id);
            if (railway == null)
            {
                return NotFound();
            }

            _context.Railways.Remove(railway);
            await _context.SaveChangesAsync();

            return railway;
        }
        private bool RailwayExists(int id)
        {
            return _context.Railways.Any(e => e.Id == id);
        }
    }
}

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using MetroWebApi.Models;
//using MetroWebApi.Controllers;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Authorization;
//using MetroWebApi.Entities;

//namespace MetroWebApi.Controllers
//{
//    [Route("[controller]/[action]")]
//    [ApiController]
//    public class TrainsController : ControllerBase
//    {
//        private readonly ApplicationContext _context;

//        public TrainsController(ApplicationContext context)
//        {
//            _context = context;
//        }

//        GET: api/trains

//       [Authorize]
//       [HttpGet]
//        public async Task<ActionResult<IEnumerable<Train>>> GetAllTrains()
//        {
//            return await _context.Trains.ToListAsync();

//        }

//        GET: api/trains/5
//        [Authorize(Roles = "Admin")]
//        [HttpGet("{id}")]
//        public async Task<ActionResult<Train>> GetTrain(int id)
//        {

//            var train = await _context.Trains.FindAsync(id);

//            if (train == null)
//            {
//                return NotFound();
//            }

//            return train;
//        }


//        [HttpPut("{id}")]
//        public async Task<IActionResult> PutTrain(int id, Train train)
//        {
//            if (id != train.Id)
//            {
//                return BadRequest();
//            }

//            _context.Entry(train).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!TrainExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }


//        [HttpPost]
//        public async Task<ActionResult<Train>> PostTrain(Train train)
//        {
//            _context.Trains.Add(train);
//            await _context.SaveChangesAsync();

//            return CreatedAtAction(nameof(GetTrain), new { id = train.Id }, train);
//        }

//        DELETE: api/trains/5
//        [Authorize(Roles = "Admin")]
//        [HttpDelete("{id}")]
//        public async Task<ActionResult<Train>> DeleteTrain(int id)
//        {
//            var train = await _context.Trains.FindAsync(id);
//            if (train == null)
//            {
//                return NotFound();
//            }

//            _context.Trains.Remove(train);
//            await _context.SaveChangesAsync();

//            return train;
//        }
//        private bool TrainExists(int id)
//        {
//            return _context.Trains.Any(e => e.Id == id);
//        }
//    }
//}
