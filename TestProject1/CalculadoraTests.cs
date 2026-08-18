using ServiciosEC.Models;
using ServiciosEC.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject1
{
    public class CalculadoraTests
    {
        [Fact]
        public void CalcularTotales_DeberiaCalcularIVA21_Correctamente()
        {
            // asignacion de variables y datos falsos (arrange)
            var compras = new List<Compra>
            {
                new Compra
                {
                    Fecha = new DateOnly(2025, 5, 1),
                    TipoFact = "1 - Factura A",
                    PuntoVenta = 3,
                    NroDesde = 11,
                    NroHasta = 11, 
                    TipoDocVendedor = "CUIT",
                    NroDocVendedor = "27123456789",
                    DenomVendedor = "RIVAS",
                    TipoCambio = 1,
                    Moneda = "$",
                    NetoGravado = 1000,
                    NoGravado = 0,
                    Exento = 0m,
                    Iva = 210,
                    Total = 1210
                }

            };

            // realizacion de acciones (act)
            var resultado = Calculadora.CalcularTotales(compras);

            // comprobacion de resultados (assert)
            Assert.Equal(1000, resultado.Neto21);
            Assert.Equal(210, resultado.Iva21);
        }

    }
}
