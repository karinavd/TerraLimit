using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TerraLimit.Api.Interfaces;

namespace TerraLimit.Api.Models;

public partial class WaterParameter : IEntity<string>
{
    [Column("Code")]
    public string Id { get; set; } = null!;

    public string? Name { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<WaterRecord> WaterRecords { get; set; } = new List<WaterRecord>();
}
