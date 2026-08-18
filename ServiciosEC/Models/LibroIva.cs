using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ServiciosEC.Models;

public partial class LibroIva
{
    public int Id { get; set; }

    public int? IdPersona { get; set; }

    public string? Cuit { get; set; }

    public int? Mes { get; set; }

    public int? Año { get; set; }

    public decimal? DebitoNeto27 { get; set; }

    public decimal? DebitoNeto21 { get; set; }

    public decimal? DebitoNeto105 { get; set; }

    public decimal? DebitoIva27 { get; set; }

    public decimal? DebitoIva21 { get; set; }

    public decimal? DebitoIva105 { get; set; }

    public decimal? DebitoNoGravado { get; set; }

    public decimal? DebitoExento { get; set; }

    public decimal? DebitoNetoOtros { get; set; }

    public decimal? DebitoIvaOtros { get; set; }

    public decimal? RestDebitoNeto27 { get; set; }

    public decimal? RestDebitoNeto21 { get; set; }

    public decimal? RestDebitoNeto105 { get; set; }

    public decimal? RestDebitoIva27 { get; set; }

    public decimal? RestDebitoIva21 { get; set; }

    public decimal? RestDebitoIva105 { get; set; }

    public decimal? RestDebitoNoGravado { get; set; }

    public decimal? RestDebitoExento { get; set; }

    public decimal? RestDebitoNetoOtros { get; set; }

    public decimal? RestDebitoIvaOtros { get; set; }

    public decimal? CreditoNeto27 { get; set; }

    public decimal? CreditoNeto21 { get; set; }

    public decimal? CreditoNeto105 { get; set; }

    public decimal? CreditoIva27 { get; set; }

    public decimal? CreditoIva21 { get; set; }

    public decimal? CreditoIva105 { get; set; }

    public decimal? CreditoNoGravado { get; set; }

    public decimal? CreditoExento { get; set; }

    public decimal? CreditoNetoOtros { get; set; }

    public decimal? CreditoIvaOtros { get; set; }

    public decimal? RestCreditoNeto27 { get; set; }

    public decimal? RestCreditoNeto21 { get; set; }

    public decimal? RestCreditoNeto105 { get; set; }

    public decimal? RestCreditoIva27 { get; set; }

    public decimal? RestCreditoIva21 { get; set; }

    public decimal? RestCreditoIva105 { get; set; }

    public decimal? RestCreditoNoGravado { get; set; }

    public decimal? RestCreditoExento { get; set; }

    public decimal? RestCreditoNetoOtros { get; set; }

    public decimal? RestCreditoIvaOtros { get; set; }

    public decimal? SaldoTecnicoAnterior { get; set; }

    public decimal? SaldoLibreDisponibilidad { get; set; }

    public decimal? RetencionesIva { get; set; }

    public decimal? PercepcionesIva { get; set; }

    public decimal GravadoDebitoFical { get; set; }

    public decimal IvaDebitoFiscal { get; set; }

    public decimal GravadoCreditoFiscal { get; set; }

    public decimal IvaCreditoFiscal { get; set; }

    public decimal SaldoTecnico { get; set; }

    public decimal SaldoTecnicoNeto { get; set; }

    public DateTime FechaDeclaracion { get; set; }

    public decimal? DebitoNeto5 { get; set; }

    public decimal? DebitoNeto25 { get; set; }

    public decimal? DebitoNeto0 { get; set; }

    public decimal? DebitoIva5 { get; set; }

    public decimal? DebitoIva25 { get; set; }

    public decimal? DebitoIva0 { get; set; }

    public decimal? RestDebitoNeto5 { get; set; }

    public decimal? RestDebitoNeto25 { get; set; }

    public decimal? RestDebitoNeto0 { get; set; }

    public decimal? RestDebitoIva5 { get; set; }

    public decimal? RestDebitoIva25 { get; set; }

    public decimal? RestDebitoIva0 { get; set; }

    public decimal? CreditoNeto5 { get; set; }

    public decimal? CreditoNeto25 { get; set; }

    public decimal? CreditoNeto0 { get; set; }

    public decimal? CreditoIva5 { get; set; }

    public decimal? CreditoIva25 { get; set; }

    public decimal? CreditoIva0 { get; set; }

    public decimal? RestCreditoNeto5 { get; set; }

    public decimal? RestCreditoNeto25 { get; set; }

    public decimal? RestCreditoNeto0 { get; set; }

    public decimal? RestCreditoIva5 { get; set; }

    public decimal? RestCreditoIva25 { get; set; }

    public decimal? RestCreditoIva0 { get; set; }


    [JsonIgnore]
    public virtual Persona? IdPersonaNavigation { get; set; }
}