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

        [HttpGet]
        [Route("{id}/GetPositionHistory")]
        public async Task<ActionResult<List<AssetPositionHistory>>> GetAssetPositionHistory(int id)
        {
            var lastSync = await _context.AssetPositionHistories
            .Where(h => h.AssetId == id)
            .OrderByDescending(h => h.DateTime)  
            .FirstOrDefaultAsync();  

            if (lastSync == null)
            {
                return NotFound($"No position history found for asset with ID {id}.");
            }

            return Ok(lastSync);
        }

        [HttpPost]
        [Route("AddAsset")]
        public async Task<IActionResult> AddAsset([FromBody] Asset asset)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Assets.Add(asset);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAsset), new { id = asset.Id }, asset);
        }

        [HttpPut]
        [Route("UpdateAsset/{id}")]
        public async Task<IActionResult> UpdateAsset(int id, [FromBody] Asset updatedAsset)
        {
            if (id != updatedAsset.Id)
            {
                return BadRequest("Asset ID mismatch.");
            }

            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            asset.Name = updatedAsset.Name;
            asset.X = updatedAsset.X;
            asset.Y = updatedAsset.Y;
            asset.LastSync = updatedAsset.LastSync;
            asset.Active = updatedAsset.Active;
            asset.FloorMapId = updatedAsset.FloorMapId;

            _context.Entry(asset).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteAsset/{id}")]
        public async Task<IActionResult> DeleteAsset(int id)
        {
            var asset = await _context.Assets.FindAsync(id);
            if (asset == null)
            {
                return NotFound();
            }

            _context.Assets.Remove(asset);
            await _context.SaveChangesAsync();

            return NoContent();
        }



    }

}
