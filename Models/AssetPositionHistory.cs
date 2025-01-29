using System;
using System.Collections.Generic;

namespace IndoorLocalization_API.Models;

public partial class AssetPositionHistory
{
    public int Id { get; set; }

    public double? X { get; set; }

    public double? Y { get; set; }

    public DateTime? DateTime { get; set; }

    public int? AssetId { get; set; }

    public int? FloorMapId { get; set; }

    public virtual Asset? Asset { get; set; }

    public virtual FloorMap? FloorMap { get; set; }
}
