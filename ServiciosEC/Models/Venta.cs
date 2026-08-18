using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Venta
{
    public int IdVenta { get; set; }

    public int IdPersona { get; set; }

    public DateOnly Fecha { get; set; }

    public string TipoFact { get; set; } = null!;

    public int? PuntoVenta { get; set; }

    public int NroDesde { get; set; }

    public int? NroHasta { get; set; }

    public string? TipoDocComprador { get; set; }

    public string? NroDocComprador { get; set; }

    public string? DenomComprador { get; set; }

    public int? TipoCambio { get; set; }

    public string? Moneda { get; set; }

    public decimal? NetoGravado { get; set; }

    public decimal? NoGravado { get; set; }

    public decimal? Exento { get; set; }

    public decimal? Iva { get; set; }

    public decimal? Total { get; set; }

    public int EstadoId { get; set; }

    public decimal? Grav0 { get; set; }

    public decimal? Grav25 { get; set; }

    public decimal? Grav5 { get; set; }

    public decimal? Grav105 { get; set; }

    public decimal? Grav21 { get; set; }

    public decimal? Grav27 { get; set; }

    public decimal? Iva0 { get; set; }

    public decimal? Iva25 { get; set; }

    public decimal? Iva5 { get; set; }

    public decimal? Iva105 { get; set; }

    public decimal? Iva21 { get; set; }

    public decimal? Iva27 { get; set; }

    public virtual Estado Estado { get; set; } = null!;

    //public virtual Iva? IdIvaNavigation { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;
}