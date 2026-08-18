using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Auditoria
{
    public int Id { get; set; }

    public string Tabla { get; set; } = null!;

    public string Accion { get; set; } = null!;

    public DateTime Fecha { get; set; }

    public int? IdPersona { get; set; }

    public string? DatosAntes { get; set; }

    public string? DatosDespues { get; set; }

    public virtual Persona? Persona { get; set; }
}
