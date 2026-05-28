using System;
using System.Collections.Generic;
using TerraLimit.Api.Interfaces;

namespace TerraLimit.Api.Models;

public partial class WaterStation : IEntity<string>
{
    public string Id { get; set; } = null!;

    public string? CountryName { get; set; }

    public string? WaterType { get; set; }

    public string? StationIdentifier { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public virtual ICollection<WaterRecord> WaterRecords { get; set; } = new List<WaterRecord>();
}
