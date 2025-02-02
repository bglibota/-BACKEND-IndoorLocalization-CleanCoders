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

        /*[HttpPost]
        [Route("AddAssetPositionHistory")]
        public async Task<HttpStatusCode> AddPositionHistory([FromBody] JsonElement assetPositionJSON)
        {
            try
            {
                var assetPositionHistory = new AssetPositionHistory
                {
                    AssetId = assetPositionJSON.GetProperty("AssetId").GetInt32(),
                    FloorMapId = assetPositionJSON.GetProperty("FloorMapId").GetInt32(),
                    X = assetPositionJSON.GetProperty("X").GetDouble(),
                    Y = assetPositionJSON.GetProperty("Y").GetDouble(),
                    DateTime = DateTime.Now 
                };

                await _context.AssetPositionHistories.AddAsync(assetPositionHistory);




                var result = await _context.SaveChangesAsync();

                return result > 0 ? HttpStatusCode.OK : HttpStatusCode.NotModified;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Error: {jsonEx.Message}");
                return HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return HttpStatusCode.InternalServerError;
            }
        }*/
        [HttpPost]
        [Route("AddAssetPositionHistory")]
        public async Task<HttpStatusCode> AddPositionHistory([FromBody] JsonElement assetPositionJSON)
        {
            try
            {
                // Parse incoming data
                int assetId = assetPositionJSON.GetProperty("AssetId").GetInt32();
                int floorMapId = assetPositionJSON.GetProperty("FloorMapId").GetInt32();
                double x = assetPositionJSON.GetProperty("X").GetDouble();
                double y = assetPositionJSON.GetProperty("Y").GetDouble();

                Console.WriteLine($"Received request: AssetId={assetId}, FloorMapId={floorMapId}, X={x}, Y={y}");

                // Create AssetPositionHistory entry
                var assetPositionHistory = new AssetPositionHistory
                {
                    AssetId = assetId,
                    FloorMapId = floorMapId,
                    X = x,
                    Y = y,
                    DateTime = DateTime.Now
                };

                await _context.AssetPositionHistories.AddAsync(assetPositionHistory);

                // Fetch zones for the floor map
                var zones = await _context.Zones
                    .Where(z => z.FloormapId == floorMapId)
                    .ToListAsync();

                // Check if the asset is in a zone
                var currentZone = zones.FirstOrDefault(zone =>
                {
                    var points = JsonSerializer.Deserialize<List<Point>>(zone.Points!);
                    return points != null && IsPointInPolygon(x, y, points);
                });

                // Fetch the latest AssetZoneHistory entry
                var latestHistory = await _context.AssetZoneHistories
                    .Where(azh => azh.AssetId == assetId && azh.ExitDateTime == null)
                    .OrderByDescending(azh => azh.EnterDateTime)
                    .FirstOrDefaultAsync();

                if (currentZone != null)
                {
                    Console.WriteLine($"Asset {assetId} is inside Zone {currentZone.Id}");

                    if (latestHistory != null && latestHistory.ZoneId != currentZone.Id)
                    {
                        latestHistory.ExitDateTime = DateTime.Now;
                        latestHistory.RetentionTime = latestHistory.ExitDateTime.Value - latestHistory.EnterDateTime;
                        Console.WriteLine($"Asset {assetId} exited Zone {latestHistory.ZoneId} at {latestHistory.ExitDateTime}, Total Time: {latestHistory.RetentionTime}");
                    }

                    if (latestHistory == null || latestHistory.ZoneId != currentZone.Id)
                    {
                        var newHistory = new AssetZoneHistory
                        {
                            AssetId = assetId,
                            ZoneId = currentZone.Id,
                            EnterDateTime = DateTime.Now
                        };
                        await _context.AssetZoneHistories.AddAsync(newHistory);
                        Console.WriteLine($"Asset {assetId} entered Zone {currentZone.Id} at {newHistory.EnterDateTime}");
                    }
                }
                else if (latestHistory != null)
                {
                    latestHistory.ExitDateTime = DateTime.Now;
                    latestHistory.RetentionTime = latestHistory.ExitDateTime.Value - latestHistory.EnterDateTime;
                    Console.WriteLine($"Asset {assetId} exited Zone {latestHistory.ZoneId} at {latestHistory.ExitDateTime}, Total Time: {latestHistory.RetentionTime}");
                }

                // Save changes to the database
                var result = await _context.SaveChangesAsync();
                return result > 0 ? HttpStatusCode.OK : HttpStatusCode.NotModified;
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"JSON Error: {jsonEx.Message}");
                return HttpStatusCode.BadRequest;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return HttpStatusCode.InternalServerError;
            }
        }

        private bool IsPointInPolygon(double x, double y, List<Point> polygon)
        {
            bool isInside = false;
            int j = polygon.Count - 1;

           

            for (int i = 0; i < polygon.Count; i++)
            {
              

                if ((polygon[i].Y > y) != (polygon[j].Y > y))  // Check if y is between the Y-coordinates of the edge
                {
                    double intersectionX = (polygon[j].X - polygon[i].X) * (y - polygon[i].Y) / (polygon[j].Y - polygon[i].Y) + polygon[i].X;
                  

                    if (x < intersectionX) // Check if the point is to the left of the intersection
                    {
                        isInside = !isInside;
                     
                    }
                }
                j = i;
            }

            Console.WriteLine($"Final result: Point ({x}, {y}) is {(isInside ? "INSIDE" : "OUTSIDE")} the polygon.");
            return isInside;
        }


       

public class Point
    {
        [JsonPropertyName("x")]
        public double X { get; set; }

        [JsonPropertyName("y")]
        public double Y { get; set; }
    }




}
}
