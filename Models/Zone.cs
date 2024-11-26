using System;
using System.Collections.Generic;

namespace IndoorLocalization_API.Models;

public partial class Zone
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Points { get; set; }

    public virtual ICollection<AssetZoneHistory> AssetZoneHistories { get; set; } = new List<AssetZoneHistory>();
}
