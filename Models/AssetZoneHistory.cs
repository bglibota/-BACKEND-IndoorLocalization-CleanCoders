using System;
using System.Collections.Generic;

namespace IndoorLocalization_API.Models;

public partial class AssetZoneHistory
{
    public int Id { get; set; }

    public int? AssetId { get; set; }

    public int? ZoneId { get; set; }

    public DateTime? EnterDateTime { get; set; }

    public DateTime? ExitDateTime { get; set; }

    public string? RetentionTime { get; set; }

    public virtual Asset? Asset { get; set; }

    public virtual Zone? Zone { get; set; }
}
