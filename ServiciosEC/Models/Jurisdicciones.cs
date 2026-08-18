using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Jurisdicciones
{
    public int IdJurisdiccion { get; set; }

    public string? Descripcion { get; set; }

    public virtual ICollection<Ingresosbrutos> Ingresosbrutos { get; set; } = new List<Ingresosbrutos>();
}
