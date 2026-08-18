using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiciosEC.Utilidades.ModelosDTO;


namespace ServiciosEC.Utilidades
{
    public class Mappers
    {
        public static LibroIva MapearLibroIvaModelAEntidad(LibroIvaModel model, int? idPersona = null)
        {
            return new LibroIva
            {
                IdPersona = model.Cliente?.IdPersona ?? idPersona,
                Cuit = model.Cuit,
                Mes = model.Mes,
                Año = model.Año,

                // Débito fiscal
                DebitoNeto27 = model.TotalDebitoFiscal.Neto27,
                DebitoNeto21 = model.TotalDebitoFiscal.Neto21,
                DebitoNeto105 = model.TotalDebitoFiscal.Neto105,
                DebitoNeto5 = model.TotalDebitoFiscal.Neto5,
                DebitoNeto25 = model.TotalDebitoFiscal.Neto25,
                DebitoNeto0 = model.TotalDebitoFiscal.Neto0,
                DebitoIva27 = model.TotalDebitoFiscal.Iva27,
                DebitoIva21 = model.TotalDebitoFiscal.Iva21,
                DebitoIva105 = model.TotalDebitoFiscal.Iva105,
                DebitoIva5 = model.TotalDebitoFiscal.Iva5,
                DebitoIva25 = model.TotalDebitoFiscal.Iva25,
                DebitoIva0 = model.TotalDebitoFiscal.Iva0,

                DebitoNoGravado = model.TotalDebitoFiscal.NoGravado,
                DebitoExento = model.TotalDebitoFiscal.Exento,
                
                ////DebitoNetoOtros = model.TotalDebitoFiscal.NetoOtros,
                ////DebitoIvaOtros = model.TotalDebitoFiscal.IvaOtros,

                // Restitución débito
                RestDebitoNeto27 = model.TotalRestitucionDebitoFiscal.Neto27,
                RestDebitoNeto21 = model.TotalRestitucionDebitoFiscal.Neto21,
                RestDebitoNeto105 = model.TotalRestitucionDebitoFiscal.Neto105,
                RestDebitoNeto5 = model.TotalRestitucionDebitoFiscal.Neto5,
                RestDebitoNeto25 = model.TotalRestitucionDebitoFiscal.Neto25,
                RestDebitoNeto0 = model.TotalRestitucionDebitoFiscal.Neto0,
                RestDebitoIva27 = model.TotalRestitucionDebitoFiscal.Iva27,
                RestDebitoIva21 = model.TotalRestitucionDebitoFiscal.Iva21,
                RestDebitoIva105 = model.TotalRestitucionDebitoFiscal.Iva105,
                RestDebitoIva5 = model.TotalRestitucionDebitoFiscal.Iva5,
                RestDebitoIva25 = model.TotalRestitucionDebitoFiscal.Iva25,
                RestDebitoIva0 = model.TotalRestitucionDebitoFiscal.Iva0,
                RestDebitoNoGravado = model.TotalRestitucionDebitoFiscal.NoGravado,
                RestDebitoExento = model.TotalRestitucionDebitoFiscal.Exento,
                //RestDebitoNetoOtros = model.TotalRestitucionDebitoFiscal.NetoOtros,
                //RestDebitoIvaOtros = model.TotalRestitucionDebitoFiscal.IvaOtros,

                // Crédito fiscal
                CreditoNeto27 = model.TotalCreditoFiscal.Neto27,
                CreditoNeto21 = model.TotalCreditoFiscal.Neto21,
                CreditoNeto105 = model.TotalCreditoFiscal.Neto105,
                CreditoNeto5 = model.TotalCreditoFiscal.Neto5,
                CreditoNeto25 = model.TotalCreditoFiscal.Neto25,
                CreditoNeto0 = model.TotalCreditoFiscal.Neto0,
                CreditoIva27 = model.TotalCreditoFiscal.Iva27,
                CreditoIva21 = model.TotalCreditoFiscal.Iva21,
                CreditoIva105 = model.TotalCreditoFiscal.Iva105,
                CreditoIva5 = model.TotalCreditoFiscal.Iva5,
                CreditoIva25 = model.TotalCreditoFiscal.Iva25,
                CreditoIva0 = model.TotalCreditoFiscal.Iva0,
                CreditoNoGravado = model.TotalCreditoFiscal.NoGravado,
                CreditoExento = model.TotalCreditoFiscal.Exento,
                //CreditoNetoOtros = model.TotalCreditoFiscal.NetoOtros,
                //CreditoIvaOtros = model.TotalCreditoFiscal.IvaOtros,

                // Restitución crédito
                RestCreditoNeto27 = model.TotalRestitucionCreditoFiscal.Neto27,
                RestCreditoNeto21 = model.TotalRestitucionCreditoFiscal.Neto21,
                RestCreditoNeto105 = model.TotalRestitucionCreditoFiscal.Neto105,
                RestCreditoNeto5 = model.TotalRestitucionCreditoFiscal.Neto5,
                RestCreditoNeto25 = model.TotalRestitucionCreditoFiscal.Neto25,
                RestCreditoNeto0 = model.TotalRestitucionCreditoFiscal.Neto0,
                RestCreditoIva27 = model.TotalRestitucionCreditoFiscal.Iva27,
                RestCreditoIva21 = model.TotalRestitucionCreditoFiscal.Iva21,
                RestCreditoIva105 = model.TotalRestitucionCreditoFiscal.Iva105,
                RestCreditoIva5 = model.TotalRestitucionCreditoFiscal.Iva5,
                RestCreditoIva25 = model.TotalRestitucionCreditoFiscal.Iva25,
                RestCreditoIva0 = model.TotalRestitucionCreditoFiscal.Iva0,
                RestCreditoNoGravado = model.TotalRestitucionCreditoFiscal.NoGravado,
                RestCreditoExento = model.TotalRestitucionCreditoFiscal.Exento,
                //RestCreditoNetoOtros = model.TotalRestitucionCreditoFiscal.NetoOtros,
                //RestCreditoIvaOtros = model.TotalRestitucionCreditoFiscal.IvaOtros,

                // Saldos manuales
                SaldoTecnicoAnterior = model.SaldoTecnicoAnterior,
                SaldoLibreDisponibilidad = model.SaldoLibreDisponibilidad,
                RetencionesIva = model.RetencionesIVA,
                PercepcionesIva = model.PercepcionesIVA,

                GravadoDebitoFical = model.GravadoDebitoFical,
                IvaDebitoFiscal = model.IVADebitoFiscal,
                GravadoCreditoFiscal = model.GravadoCreditoFical,
                IvaCreditoFiscal = model.IVACreditoFiscal,

                SaldoTecnico = model.SaldoTecnico,
                SaldoTecnicoNeto = model.SaldoTecnicoNeto,

                FechaDeclaracion = DateTime.Now // Asignar la fecha actual como fecha de declaración

            };
        }

    }
}
