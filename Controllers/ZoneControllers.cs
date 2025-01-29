using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using IndoorLocalization_API.Models;
using Newtonsoft.Json;
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
        [HttpGet]
        [Route("GetAllZones")]
        public async Task<List<Zone>> GetAllZones()
        {
            var zones = await _context.Zones.ToListAsync();

            if (zones == null || zones.Count == 0)
            {
                return new List<Zone>();
            }

            // At this point, `zone.Points` is already deserialized to a List<Point>
            foreach (var zone in zones)
            {
                // You can now safely access `zone.Points` as a List<Point>
                if (zone.Points != null && zone.Points.Count > 0)
                {
                    foreach (var point in zone.Points)
                    {
                        // Do something with point.X, point.Y, point.OrdinalNumber
                    }
                }
                else
                {
                    // Handle case where Points is null or empty
                }
            }

            return zones;
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
