using ServiciosEC.Models;
using ServiciosEC.Utilidades.ModelosDTO;

namespace ServiciosEC.Utilidades.ModelosDTO
{
    public class LibroIvaModel
    {
        #region Ventas
        public TotalesIVA TotalDebitoFiscal { get; set; }
        public TotalesIVA TotalRestitucionDebitoFiscal { get; set; }
        public TotalesIVA TotalNetoDebitoFiscal => TotalDebitoFiscal - TotalRestitucionDebitoFiscal;
        #endregion

        


        #region Compras
        public TotalesIVA TotalCreditoFiscal { get; set; }
        public TotalesIVA TotalRestitucionCreditoFiscal { get; set; }
        public TotalesIVA TotalNetoCreditoFiscal => TotalCreditoFiscal - TotalRestitucionCreditoFiscal;
        #endregion

        /// <summary>
        /// Determinacion del saldo
        /// </summary>
        public decimal GravadoDebitoFical => TotalDebitoFiscal.NetoGravado + TotalRestitucionCreditoFiscal.NetoGravado;
        public decimal IVADebitoFiscal => TotalDebitoFiscal.Iva + TotalRestitucionCreditoFiscal.Iva;

        public decimal GravadoCreditoFical => TotalCreditoFiscal.NetoGravado + TotalRestitucionDebitoFiscal.NetoGravado;
        public decimal IVACreditoFiscal => TotalCreditoFiscal.Iva + TotalRestitucionDebitoFiscal.Iva;

        /// <summary>
        /// IVA debito fiscal - IVA credito fiscal
        /// </summary>
        public decimal SaldoTecnico => IVADebitoFiscal - IVACreditoFiscal - SaldoTecnicoAnterior;


        #region ingreso manual
        public decimal SaldoTecnicoAnterior { get; set; }

        public decimal SaldoLibreDisponibilidad{ get; set; }
        public decimal RetencionesIVA { get; set; }
        public decimal PercepcionesIVA { get; set; }
        #endregion

        public decimal SaldoTecnicoNeto => SaldoTecnico - (SaldoLibreDisponibilidad + RetencionesIVA + PercepcionesIVA);



        #region Datos del cliente y periodo
        public Cliente Cliente { get; set; }

        public int? idPersona { get; set; }
        public string? Cuit { get; set; }
        public int? Mes { get; set; }
        public int? Año { get; set; }
        #endregion


    }

}
