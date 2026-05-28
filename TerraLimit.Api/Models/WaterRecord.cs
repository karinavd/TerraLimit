using System;
using System.Collections.Generic;
using TerraLimit.Api.Interfaces;

namespace TerraLimit.Api.Models;

public partial class WaterRecord : IEntity<int>
{
    public int Id { get; set; }

    public string? StationId { get; set; }

    public DateOnly SampleDate { get; set; }

    public double Depth { get; set; }

    public string? ParameterCode { get; set; }

    public decimal? Value { get; set; }

    public string? Unit { get; set; }

    public virtual WaterParameter? ParameterCodeNavigation { get; set; }

    public virtual WaterStation? Station { get; set; }
}
