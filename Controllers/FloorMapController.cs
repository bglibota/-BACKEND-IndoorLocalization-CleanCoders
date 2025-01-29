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
    [Route("api/[controller]")]
    [ApiController]
    public class FloorMapController : ControllerBase
    {
        private readonly IndoorLocalizationContext _context;

        public FloorMapController(IndoorLocalizationContext context)
        {
            _context = context;
        }

        // GET: api/FloorMap
        [HttpGet]
        public async Task<ActionResult<List<FloorMap>>> GetAllFloorMaps()
        {
            var floorMaps = await _context.FloorMaps.ToListAsync();
            return Ok(floorMaps);
        }

        // GET: api/FloorMap/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FloorMap>> GetFloorMap(int id)
        {
            var floorMap = await _context.FloorMaps.FindAsync(id);

            if (floorMap == null)
            {
                return NotFound();
            }

            return Ok(floorMap);
        }
    }
}
