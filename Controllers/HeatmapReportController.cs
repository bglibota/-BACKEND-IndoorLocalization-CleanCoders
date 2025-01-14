﻿using IndoorLocalization_API.Database;
using IndoorLocalization_API.Models;
using IndoorLocalization_API.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace IndoorLocalization_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HeatmapReportController : APIDatabaseContext<AssetPositionHistory>
    {
        public HeatmapReportController(IndoorLocalizationContext context) : base(context) { }

        [HttpGet]
        [Route("GetAssetPositionHistory/{assetId}")]
        public async Task<ActionResult<List<AssetPositionHistory>>> GetAssetPositionHistoriesAsync(int assetId)
        {
            var assetPositionHistory = await _context.AssetPositionHistories
                .Where(p => p.AssetId == assetId)
                .ToListAsync();

            return assetPositionHistory ?? new List<AssetPositionHistory>();
        }

        [HttpGet]
        [Route("GetAssetPositionHistoryByDate/{assetId}/{date}")]
        public async Task<ActionResult<List<AssetPositionHistory>>> GetAssetPositionHistoriesByDateAsync(int assetId, DateTime date)
        {
            var assetPositionHistory = await _context.AssetPositionHistories
                .Where(p => p.AssetId == assetId && p.DateTime.Value.Date == date.Date)
                .ToListAsync();

            return assetPositionHistory ?? new List<AssetPositionHistory>();
        }

        [HttpGet]
        [Route("GetAssetPositionHistoryByDateAndTimeRange/{assetId}/{date}/{startTime}/{endTime}")]
        public async Task<ActionResult<List<AssetPositionHistory>>> GetAssetPositionHistoriesByDateRangeAsync(int assetId, DateTime date, TimeSpan startTime, TimeSpan endTime)
        {
            var assetPositionHistory = await _context.AssetPositionHistories
                .Where(p => p.AssetId == assetId
                            && p.DateTime.Value.Date == date.Date
                            && p.DateTime.Value.TimeOfDay >= startTime
                            && p.DateTime.Value.TimeOfDay <= endTime)
                .ToListAsync();

            return assetPositionHistory ?? new List<AssetPositionHistory>();
        }

        [HttpGet]
        [Route("GetAllByDateAndTimeRange/{date}/{startTime}/{endTime}")]
        public async Task<ActionResult<List<AssetPositionHistoryDTO>>> GetAllByDateAndTimeRange(DateTime date, TimeSpan startTime, TimeSpan endTime)
        {

            var assetPositionHistory = await _context.AssetPositionHistories
                .Where(p => p.DateTime.Value.Date == date.Date
                            && p.DateTime.Value.TimeOfDay >= startTime
                            && p.DateTime.Value.TimeOfDay <= endTime)
                .Include(p=>p.Asset)
                .Include(p=>p.FloorMap)
                .Select(p => new AssetPositionHistoryDTO
                {
                    Id=p.Id,
                    AssetId = p.AssetId,
                    DateTime = p.DateTime,
                    X = p.X,
                    Y = p.Y,
                    AssetName = p.Asset.Name,
                    FloorMapName = p.FloorMap.Name
                })
                .ToListAsync();

            return assetPositionHistory ?? new List<AssetPositionHistoryDTO>();
        }
        [HttpGet]
        [Route("GetAssetPositionHistoryByDateRangeAndTimeRange/{startDate}/{endDate}/{startTime}/{endTime}")]
        public async Task<ActionResult<List<AssetPositionHistoryDTO>>> GetAssetPositionHistoriesByDateRangeAndTimeRangeAsync(DateTime startDate, DateTime endDate, TimeSpan startTime, TimeSpan endTime)
        {
            var assetPositionHistory = await _context.AssetPositionHistories
                .Where(p => p.DateTime.Value.Date >= startDate.Date
                            && p.DateTime.Value.Date <= endDate.Date
                            && p.DateTime.Value.TimeOfDay >= startTime
                            && p.DateTime.Value.TimeOfDay <= endTime)
                .Include(p => p.Asset)
                .Include(p => p.FloorMap)
                .Select(p => new AssetPositionHistoryDTO
                {
                    Id = p.Id,
                    AssetId = p.AssetId,
                    FloorMapId=p.FloorMapId,
                    DateTime = p.DateTime,
                    X = p.X,
                    Y = p.Y,
                    AssetName = p.Asset.Name,
                    FloorMapName = p.FloorMap.Name
                })
                .ToListAsync();
            return assetPositionHistory ?? new List<AssetPositionHistoryDTO>();
        }

        [HttpGet]
        [Route("GetAssetPositionHistoryByTimeRange/{assetId}/{startTime}/{endTime}")]
        public async Task<ActionResult<List<AssetPositionHistory>>> GetAssetPositionHistoriesByTimeRangeAsync(int assetId, TimeSpan startTime, TimeSpan endTime)
        {
            var assetPositionHistory = await _context.AssetPositionHistories
                .Where(p => p.AssetId == assetId
                            && p.DateTime.Value.TimeOfDay >= startTime
                            && p.DateTime.Value.TimeOfDay <= endTime)
                .ToListAsync();

            return assetPositionHistory ?? new List<AssetPositionHistory>();
        }

        [HttpGet]
        [Route("GetAllFloorMapsFromHistory")]
        public async Task<ActionResult<List<FloorMap>>> GetAllFloorMaps()
        {
            var floorMaps = await _context.AssetPositionHistories.Where(p=>p.FloorMapId!=null)
                                                                 .Include(p=>p.FloorMap)
                                                                 .GroupBy(p=>p.FloorMapId)
                                                                 .Select(p=>p.First().FloorMap)
                                                                 .ToListAsync();
            return floorMaps ?? new List<FloorMap>();
        }

        [HttpPost]
        [Route("AddAssetPositionHistory")]
        public async Task<HttpStatusCode> AddPositionHistory([FromBody] AssetPositionHistory positionHistory)
        {
            await _context.AssetPositionHistories.AddAsync(positionHistory);
            var result = await _context.SaveChangesAsync();
            return (result > 0) ? HttpStatusCode.OK : HttpStatusCode.NotModified;
        }

    }
}
