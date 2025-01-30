using IndoorLocalization_API.Database;
using IndoorLocalization_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorMapController : APIDatabaseContext<FloorMap>
    {
        public FloorMapController(IndoorLocalizationContext context) : base(context) { }
        [HttpGet]
        [Route("GetAllFloorMaps")]
        public async Task<List<FloorMap>> GetAllFloorMaps()
        {
            var floorMaps = await _context.FloorMaps.Include(p => p.AssetPositionHistories).ToListAsync();
            if (floorMaps == null)
            {
                return new List<FloorMap>();
            }
            return floorMaps;
        }
        [HttpGet]
        [Route("GetFloorMap/{id}")]
        public async Task<ActionResult<FloorMap>> GetFloorMap(int id)
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
