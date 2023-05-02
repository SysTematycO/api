using System;
using System.Collections.Generic;

namespace DeltaAPI.Models;
/// <summary>
/// Tabla de BD Sql Server
/// </summary>
public partial class ValorXpolitica
{
    public int Id { get; set; }

    public int Tipo { get; set; }

    public string Nombre { get; set; } = null!;

    public decimal? Valor { get; set; }

    public string? ReglaNegocio { get; set; }
}
