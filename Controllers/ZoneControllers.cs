using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IndoorLocalization_API.Models;

namespace IndoorLocalization_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZoneController : ControllerBase
    {
        private readonly IndoorLocalizationContext _context;

        public ZoneController(IndoorLocalizationContext context)
        {
            _context = context;
        }

        // Dohvati sve zone s nazivima
        [HttpGet]
        [Route("GetAllZones")]
        public async Task<List<Zone>> GetAllZones(int? floormapId)
        {
            // Fetch all zones from the database
            var zonesQuery = _context.Zones.AsQueryable();

            // If a floormapId is provided, filter the zones by FloormapId
            if (floormapId.HasValue)
            {
                zonesQuery = zonesQuery.Where(zone => zone.FloormapId == floormapId.Value);
            }

            // Execute the query and return the list of zones
            var zones = await zonesQuery.ToListAsync();

            return zones ?? new List<Zone>();
        }


        // Dohvati jednu zonu prema ID-u
        [HttpGet]
        [Route("GetZoneById/{id}")]
        public async Task<Zone?> GetZoneById(int id)
        {
            return await _context.Zones.FirstOrDefaultAsync(z => z.Id == id);
        }
    }

}
