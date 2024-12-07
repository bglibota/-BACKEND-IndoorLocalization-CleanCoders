using IndoorLocalization_API.Database;
using IndoorLocalization_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IndoorLocalization_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeatmapReportController : APIDatabaseContext<AssetPositionHistory>
    {
        public HeatmapReportController(IndoorLocalizationContext context) : base(context){}

        [HttpGet]
        [Route("GetAssetPositionHistory/{assetId}")]
        public async Task<List<AssetPositionHistory>> GetAssetPositionHistoriesAsync(int assetId)
        {
            
            var assetPositionHistory = await _context.AssetPositionHistories.Where(p=>p.AssetId==assetId).ToListAsync();
            if (assetPositionHistory == null)
            { 
                return new List<AssetPositionHistory>();
            }
            return assetPositionHistory;
        }
    }
}
