using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MetroWebApi.Entities;
using MetroWebApi.Models;
using MetroWebApi.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MetroWebApi.Services
{
    public class RailwayService : IRailwayService
    {
        private readonly ApplicationContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public RailwayService(ApplicationContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }        
        public async Task<IEnumerable<Railway>> GetAllRailwaysAsync()
        {
            var railwayList = await _context.Railways.ToListAsync();

            if (railwayList.Count() == 0)
            {
                throw new Exception("no railways yet.");
            }
            return railwayList;
        }

        public async Task<Railway> GetRailwayAsync(int railwayId)
        {
            var railway = await _context.Railways.FindAsync(railwayId);

            if (railway == null)
            {
                throw new Exception("railway with this Id not found.");
            }

            return railway;
        }

        public async Task PostRailwayAsync(Railway railway)
        {
            _context.Railways.Add(railway);
            await _context.SaveChangesAsync();
        }

        public async Task<Railway> PutRailwayAsync(int railwayId, Railway railway)
        {
            if (railwayId != railway.Id)
            {
                throw new Exception("Id is not equal to railway Id.");
            }

            _context.Entry(railway).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.Railways.Any(e => e.Id == railwayId))
                {
                    throw new Exception("this railway is not exist.");
                }
                else
                {
                    throw;
                }
            }

            return railway;
        }
        
        public async Task<Railway> DeleteRailwayAsync(int railwayId)
        {
            var railway = await _context.Railways.FindAsync(railwayId);

            if (railway == null)
            {
                throw new Exception("railway with this Id not found.");
            }

            _context.Railways.Remove(railway);
            await _context.SaveChangesAsync();

            return railway;
        }
    }
}
