using System;
using System.Collections.Generic;

namespace IndoorLocalization_API.Models;

public partial class Asset
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? X { get; set; }

    public int? Y { get; set; }

    public TimeOnly? LastSync { get; set; }

    public bool? Active { get; set; }

    public int? FloorMapId { get; set; }

    public virtual ICollection<AssetPositionHistory> AssetPositionHistories { get; set; } = new List<AssetPositionHistory>();

    public virtual ICollection<AssetZoneHistory> AssetZoneHistories { get; set; } = new List<AssetZoneHistory>();

    public virtual FloorMap? FloorMap { get; set; }
}
