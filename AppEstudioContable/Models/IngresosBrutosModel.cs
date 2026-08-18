using System.ComponentModel.DataAnnotations;

namespace AppEstudioContable.Models
{
    public class IngresosBrutosModel
    {

        public int? IdIngresosbrutos { get; set; }

        public int id { get; set; }

        [Range(1, 12, ErrorMessage = "El periodo debe ser un número entero entre 1 y 12.")]
        public int Periodo { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "El año debe ser un número entero mayor que cero.")]
        public int Anio { get; set; }

        public decimal? Gravado { get; set; }

        public int JurisdiccionId { get; set; }

        [Range(0.00001, 1, ErrorMessage = "El coeficiente debe ser mayor que 0 y menor o igual a 1.")]
        public decimal Coeficiente { get; set; } = 1m;

        public decimal? GravadoPais { get; set; }

        public decimal? GravadoJurisdiccion { get; set; }

        [Range(0.0000, 15.0000, ErrorMessage = "La alícuota debe estar entre 0 y 15.")]
        public decimal? Alicuota { get; set; } = 0m;

        public decimal? ImpuestoDeterminado { get; set; }

        public decimal? Retenciones { get; set; }

        public decimal? RetencionesBancarias { get; set; }

        public decimal? Percepciones { get; set; }

        public decimal? Aduaneras { get; set; }

        public decimal? Saldo { get; set; }

    }
}
