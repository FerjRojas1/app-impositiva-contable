using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Estado
{
    public int IdEstado { get; set; }

    public string Descripcion { get; set; } = null!;

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual ICollection<Persona> Personas { get; set; } = new List<Persona>();

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
