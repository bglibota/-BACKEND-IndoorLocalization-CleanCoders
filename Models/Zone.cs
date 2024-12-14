using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;  // Koristi Newtonsoft.Json za deserializaciju

namespace IndoorLocalization_API.Models
{
    public class Zone
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Ovdje je lista koja predstavlja tačke koje ćemo deserializovati iz JSONB
        public List<Point> Points { get; set; }

        public virtual ICollection<AssetZoneHistory> AssetZoneHistories { get; set; } = new List<AssetZoneHistory>();
    }

    public class Point
    {
        public int X { get; set; } // Koordinata X
        public int Y { get; set; } // Koordinata Y
        public int OrdinalNumber { get; set; } // Redni broj tačke
    }
}
