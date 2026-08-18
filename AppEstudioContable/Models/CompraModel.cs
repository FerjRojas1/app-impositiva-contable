using ServiciosEC.Models;
using System.ComponentModel.DataAnnotations;

namespace AppEstudioContable.Models
{
    public class CompraModel
    {       
        public DateOnly Fecha { get; set; }

        public int IdCompra { get; set; }
        public string TipoFact { get; set; } = null!;

        public int? PuntoVenta { get; set; }

        public int NroDesde { get; set; }

        public int? NroHasta { get; set; }

        public string? TipoDocVendedor { get; set; }

        public string? NroDocVendedor { get; set; }

        public string? DenomVendedor { get; set; }

        public int? TipoCambio { get; set; }

        public string? Moneda { get; set; }

        public decimal? NetoGravado { get; set; }

        public decimal? NoGravado { get; set; }

        public decimal? Exento { get; set; }

        public decimal? Iva { get; set; }

        public decimal? Total { get; set; }

        [Display(Name = "Cliente")]
        public int id { get; set; }

        [Display(Name = "Gravado 0%")]
        public decimal? Grav0 { get; set; }

        [Display(Name = "Gravado 25%")]
        public decimal? Grav25 { get; set; }

        [Display(Name = "Gravado 5%")]
        public decimal? Grav5 { get; set; }

        [Display(Name = "Gravado 10.5%")]
        public decimal? Grav105 { get; set; }

        [Display(Name = "Gravado 21%")]
        public decimal? Grav21 { get; set; }

        [Display(Name = "Gravado 27%")]
        public decimal? Grav27 { get; set; }

        [Display(Name = "IVA 0%")]
        public decimal? Iva0 { get; set; }

        [Display(Name = "IVA 25%")]
        public decimal? Iva25 { get; set; }

        [Display(Name = "IVA 5%")]
        public decimal? Iva5 { get; set; }

        [Display(Name = "IVA 10.5%")]
        public decimal? Iva105 { get; set; }

        [Display(Name = "IVA 21%")]
        public decimal? Iva21 { get; set; }

        [Display(Name = "IVA 27%")]
        public decimal? Iva27 { get; set; }

    }
}
