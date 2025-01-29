using IndoorLocalization_API.Database;
using IndoorLocalization_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloormapController : APIDatabaseContext<FloorMap>
    {
        public FloormapController(IndoorLocalizationContext context) : base(context) { }

        [HttpGet]
        [Route("GetAllFloormaps")]
        public async Task<ActionResult<List<FloorMap>>> GetAllFloormaps()
        {
            var floorMaps = await _context.FloorMaps.ToListAsync();
            if (floorMaps == null)
            {
                return NotFound();
            }
            return floorMaps;
        }

        [HttpGet]
        [Route("GetFloormapById/{id}")]
        public async Task<ActionResult<FloorMap>> GetFloormapById(int id)
        {
            var floorMap = await _context.FloorMaps.FindAsync(id);
            if (floorMap == null)
            {
                return NotFound();
            }
            return floorMap;
        }

        [HttpPost]
        [Route("AddFloormap")]
        public async Task<ActionResult<FloorMap>> AddFloormap([FromBody] FloorMap floorMap)
        {
            if (floorMap == null)
            {
                return BadRequest();
            }
            var floormapEntity = await _context.FloorMaps.AddAsync(floorMap);
            await _context.SaveChangesAsync();
            return floormapEntity.Entity;
        }
    }
}
