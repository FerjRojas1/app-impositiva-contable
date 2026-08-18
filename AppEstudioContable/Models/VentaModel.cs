using System.ComponentModel.DataAnnotations;

namespace AppEstudioContable.Models
{
    public class VentaModel
    {
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]

        public DateOnly Fecha { get; set; }

        [Required(ErrorMessage = "El tipo de factura es obligatorio.")]
        [StringLength(50, ErrorMessage = "El tipo de factura no puede exceder los 50 caracteres.")]
        [Display(Name = "Tipo de Factura")]
        public string TipoFact { get; set; } = null!;
        [Required(ErrorMessage = "El punto de venta es obligatorio.")]
        [Range(1, 9999, ErrorMessage = "El punto de venta debe estar entre 1 y 9999.")]
        [Display(Name = "Punto de Venta")]
        public int? PuntoVenta { get; set; }

        [Required(ErrorMessage = "El número 'Desde' es obligatorio.")]
        [Range(1, 99999999, ErrorMessage = "El número 'Desde' debe estar entre 1 y 99999999.")]
        [Display(Name = "Número Desde")]
        public int NroDesde { get; set; }

        //[Required(ErrorMessage = "El número 'Hasta' es obligatorio.")]
        [Range(1, 99999999, ErrorMessage = "El número 'Hasta' debe estar entre 1 y 99999999.")]
        [Display(Name = "Número Hasta")]
        public int? NroHasta { get; set; }

        [StringLength(50, ErrorMessage = "El tipo de documento del comprador no puede exceder los 50 caracteres.")]
        [Display(Name = "Tipo Doc. Comprador")]
        public string? TipoDocComprador { get; set; }

        [StringLength(50, ErrorMessage = "El número de documento del comprador no puede exceder los 50 caracteres.")]
        [Display(Name = "Nro. Doc. Comprador")]
        public string? NroDocComprador { get; set; }

        [StringLength(200, ErrorMessage = "La denominación del comprador no puede exceder los 200 caracteres.")]
        [Display(Name = "Denominación Comprador")]
        public string? DenomComprador { get; set; }

        [Display(Name = "Tipo de Cambio")]
        public int? TipoCambio { get; set; }

        [StringLength(10, ErrorMessage = "La moneda no puede exceder los 10 caracteres.")]
        public string? Moneda { get; set; }

        [Display(Name = "Neto Gravado")]
        public decimal? NetoGravado { get; set; }

        [Display(Name = "No Gravado")]
        public decimal? NoGravado { get; set; }

        public decimal? Exento { get; set; }

        [Display(Name = "IVA")]
        public decimal? Iva { get; set; }

        [Required(ErrorMessage = "El total es obligatorio.")]
        public decimal? Total { get; set; }

        [Display(Name = "Cliente")]
        public int id { get; set; }

        //[Required(ErrorMessage = "El estado es obligatorio.")]
        [Display(Name = "Estado")]
        public int EstadoId { get; set; }

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