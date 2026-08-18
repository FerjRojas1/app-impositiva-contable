using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Cliente : Persona
{

    public string Cuit { get; set; } = null!;

    public string? DomFiscal { get; set; }

    public string? RazonSocial { get; set; }
}
