using ServiciosEC.Models;
using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Iva
{
    public int IdIva { get; set; }

    public string Descripcion { get; set; } = null!;

    public decimal Porcentaje { get; set; }

    //public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();
}
