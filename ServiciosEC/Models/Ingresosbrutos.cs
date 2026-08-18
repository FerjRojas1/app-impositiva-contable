using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Ingresosbrutos
{
    public int IdIngresosbrutos { get; set; }

    public int IdPersona { get; set; }

    public int Periodo { get; set; }

    public int Anio { get; set; }

    public int JurisdiccionId { get; set; }

    public decimal Coeficiente { get; set; }

    public decimal? GravadoPais { get; set; }

    public decimal? GravadoJurisdiccion { get; set; }

    public decimal? Alicuota { get; set; }

    public decimal? ImpuestoDeterminado { get; set; }

    public decimal? Retenciones { get; set; }

    public decimal? RetencionesBancarias { get; set; }

    public decimal? Percepciones { get; set; }

    public decimal? Aduaneras { get; set; }

    public decimal? Saldo { get; set; }

    public DateTime FechaDeclaracion { get; set; }

    public virtual Persona IdPersonaNavigation { get; set; } = null!;

    public virtual Jurisdicciones Jurisdiccion { get; set; } = null!;
}
