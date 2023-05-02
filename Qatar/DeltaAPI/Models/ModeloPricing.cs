using System;
using System.Collections.Generic;

namespace DeltaAPI.Models;
/// <summary>
/// Tabla de BD Sql Server
/// </summary>
public partial class ModeloPricing
{
    public int Id { get; set; }

    public int Score { get; set; }

    public decimal TasaEase { get; set; }

    public decimal TasaEace { get; set; }
}
