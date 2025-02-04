using IndoorLocalization_API.Database;
using IndoorLocalization_API.Models;
using IndoorLocalization_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Drawing;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace IndoorLocalization_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ZoneReportController : APIDatabaseContext<AssetZoneHistory>
    {
        public ZoneReportController(IndoorLocalizationContext context) : base(context) { }
        [HttpGet]
        [Route("GetAssetZoneHistoryByDateRangeAndTimeRange/{startDate}/{endDate}/{startTime}/{endTime}")]
        public async Task<ActionResult<List<object>>> GetAssetZoneHistoryByDateRangeAndTimeRangeAsync(
     DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
        {
            Console.WriteLine($"Received params - StartDate: {startDate}, EndDate: {endDate}, StartTime: {startTime}, EndTime: {endTime}");

            var assetZoneHistory = await _context.AssetZoneHistories
                .Where(azh => azh.EnterDateTime.HasValue
                              && azh.EnterDateTime.Value.Date >= startDate.Date
                              && azh.EnterDateTime.Value.Date <= endDate.Date
                              && azh.EnterDateTime.Value.TimeOfDay >= startTime
                              && azh.EnterDateTime.Value.TimeOfDay <= endTime)
                .Include(azh => azh.Asset)
                .Include(azh => azh.Zone)
                .Select(azh => new
                {
                    azh.Id,
                    EnterDateTime = azh.EnterDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                    ExitDateTime = azh.ExitDateTime.HasValue ? azh.ExitDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffffff") : null,
                    RetentionTime = azh.RetentionTime.HasValue ? azh.RetentionTime.Value.ToString(@"hh\:mm\:ss\.ffffff") : null,
                    azh.AssetId,
                    AssetName = azh.Asset != null ? azh.Asset.Name : "Unknown",
                    azh.ZoneId,
                    ZoneName = azh.Zone != null ? azh.Zone.Name : "Unknown"
                })
                .ToListAsync();

            return Ok(assetZoneHistory);
        }


        [HttpGet]
        [Route("GetAssetZoneHistoryByFloorMapAndDateTimeRange/{floormapId}/{startDate}/{endDate}/{startTime}/{endTime}")]
        public async Task<ActionResult<List<object>>> GetAssetZoneHistoryByFloorMapAndDateTimeRangeAsync(
    int floormapId, DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
        {
            var assetZoneHistory = await _context.AssetZoneHistories
                .Where(azh => azh.Zone!.FloormapId == floormapId
                              && azh.EnterDateTime.HasValue
                              && azh.EnterDateTime.Value.Date >= startDate.Date
                              && azh.EnterDateTime.Value.Date <= endDate.Date
                              && azh.EnterDateTime.Value.TimeOfDay >= startTime
                              && azh.EnterDateTime.Value.TimeOfDay <= endTime)
                .Include(azh => azh.Asset)
                .Include(azh => azh.Zone)
                .Select(azh => new
                {
                    azh.Id,
                    EnterDateTime = azh.EnterDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                    ExitDateTime = azh.ExitDateTime.HasValue ? azh.ExitDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffffff") : null,
                    RetentionTime = azh.RetentionTime.HasValue ? azh.RetentionTime.Value.ToString(@"hh\:mm\:ss\.ffffff") : null,
                    azh.AssetId,
                    AssetName = azh.Asset != null ? azh.Asset.Name : "Unknown",
                    azh.ZoneId,
                    ZoneName = azh.Zone != null ? azh.Zone.Name : "Unknown"
                })
                .ToListAsync();

            return Ok(assetZoneHistory);
        }

        [HttpGet]
        [Route("GetAssetZoneHistoryByAssetAndDateTimeRange/{assetId}/{startDate}/{endDate}/{startTime}/{endTime}")]
        public async Task<ActionResult<List<object>>> GetAssetZoneHistoryByAssetAndDateTimeRangeAsync(
    int assetId, DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
        {
            var assetZoneHistory = await _context.AssetZoneHistories
                .Where(azh => azh.AssetId == assetId
                              && azh.EnterDateTime.HasValue
                              && azh.EnterDateTime.Value.Date >= startDate.Date
                              && azh.EnterDateTime.Value.Date <= endDate.Date
                              && azh.EnterDateTime.Value.TimeOfDay >= startTime
                              && azh.EnterDateTime.Value.TimeOfDay <= endTime)
                .Include(azh => azh.Asset)
                .Include(azh => azh.Zone)
                .Select(azh => new
                {
                    azh.Id,
                    EnterDateTime = azh.EnterDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffffff"),
                    ExitDateTime = azh.ExitDateTime.HasValue ? azh.ExitDateTime.Value.ToString("yyyy-MM-dd HH:mm:ss.ffffff") : null,
                    RetentionTime = azh.RetentionTime.HasValue ? azh.RetentionTime.Value.ToString(@"hh\:mm\:ss\.ffffff") : null,
                    azh.AssetId,
                    AssetName = azh.Asset != null ? azh.Asset.Name : "Unknown",
                    azh.ZoneId,
                    ZoneName = azh.Zone != null ? azh.Zone.Name : "Unknown"
                })
                .ToListAsync();

            return Ok(assetZoneHistory);
        }




    }

}
