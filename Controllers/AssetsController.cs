using IndoorLocalization_API.Database;
using IndoorLocalization_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssetsController :APIDatabaseContext<Asset>
    {
        public AssetsController(IndoorLocalizationContext context) : base(context){}

        [HttpGet]
        [Route("GetAllAssets")]
        public async Task<List<Asset>> GetAllAssets()
        {
            var assets = await _context.Assets.ToListAsync();

            if (assets == null)
            {
                return new List<Asset>();
            }

            return assets;
        }

        [HttpGet]
        [Route("GetAsset/{id}")]
        public async Task<ActionResult<Asset>> GetAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);

            if (asset == null)
            {
                return NotFound();
            }

            return asset;
        }
    }
    
}
