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

namespace MetroWebApi.Controllers
{
    [Route("api/trains")]
    [ApiController]
    public class TrainsController : ControllerBase
    {
        private readonly TrainContext _context;

        public TrainsController(TrainContext context)
        {
            _context = context;
        }

        // GET: api/trains
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Train>>> GetTrainItems()
        {
            
             return await _context.Trains.ToListAsync();
                               
        }

        // GET: api/trains/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Train>> GetTrain(int id)
        {
            
            var train = await _context.Trains.FindAsync(id);

            if (train == null)
            {
                return NotFound();
            }

            return train;
        }

       /* [HttpGet("{startPoint}")]
        public async void GetTrainByStartPoint()
        {  
            var train = await _context.Trains.CountAsync();
            return train;
        }*/

        // PUT: api/trains/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTrain(int id, Train train)
        {
            if (id != train.Id)
            {
                return BadRequest();
            }

            _context.Entry(train).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TrainExists(id))
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

        // POST: api/trains
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Train>> PostTrain(Train train)
        {
            _context.Trains.Add(train);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetTrain", new { id = train.Id }, train);
            return CreatedAtAction(nameof(GetTrain), new { id = train.Id }, train);
        }

        // DELETE: api/trains/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Train>> DeleteTrain(int id)
        {
            var train = await _context.Trains.FindAsync(id);
            if (train == null)
            {
                return NotFound();
            }

            _context.Trains.Remove(train);
            await _context.SaveChangesAsync();

            return train;
        }

        private bool TrainExists(int id)
        {
            return _context.Trains.Any(e => e.Id == id);
        }
    }
}
