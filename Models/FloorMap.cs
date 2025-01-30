using System;
using System.Collections.Generic;

namespace IndoorLocalization_API.Models;

public partial class FloorMap
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<AssetPositionHistory> AssetPositionHistories { get; set; } = new List<AssetPositionHistory>();

    public virtual ICollection<Asset> Assets { get; set; } = new List<Asset>();

    public virtual ICollection<Zone> Zones { get; set; } = new List<Zone>();
}
