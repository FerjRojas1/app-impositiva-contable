using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public abstract partial class Persona
{
    public int IdPersona { get; set; }

    public string? Nombre { get; set; }

    public string? Apellido { get; set; }

    public string? Dni { get; set; }

    public string? Email { get; set; }

    public string? Telefono { get; set; }

    public DateTime FechaAlta { get; set; }

    public int EstadoId { get; set; }

    public int RolId { get; set; }

    public virtual ICollection<Auditoria> Auditoria { get; set; } = new List<Auditoria>();

    public virtual ICollection<Compra> Compras { get; set; } = new List<Compra>();

    public virtual Estado Estado { get; set; } = null!;

    public virtual Role Rol { get; set; } = null!;

    public virtual ICollection<Venta> Venta { get; set; } = new List<Venta>();

    public virtual ICollection<Ingresosbrutos> Ingresosbrutos { get; set; } = new List<Ingresosbrutos>();

    public virtual ICollection<LibroIva> LibrosIva { get; set; } = new List<LibroIva>();

}
