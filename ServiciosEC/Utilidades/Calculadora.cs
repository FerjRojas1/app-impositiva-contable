using ServiciosEC.Models;
using ServiciosEC.Utilidades.ModelosDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Utilidades
{
    /// <summary>
    /// Clase para realizar calculos complejos o largos.
    /// </summary>
    public class Calculadora
    {

        public static TotalesIVA CalcularTotales(IEnumerable<Venta> ventas)
        {
            // poner negativo los valores de las notas de crédito
            foreach (var v in ventas)
            {
                RestarNotasCredito(v);
            }

            var ventasConNeto = ventas.Where(v => v.NetoGravado!=null && v.NetoGravado!=0).ToList();

            

            var totalesIVA = new TotalesIVA
            {
                Neto27 = ventas.Sum(v => v.Grav27) ?? 0,
                Neto21 = ventas.Sum(v => v.Grav21) ?? 0,
                Neto105 = ventas.Sum(v => v.Grav105) ?? 0,
                Neto0 = ventas.Sum(v => v.Grav0) ?? 0,
                Neto25 = ventas.Sum(v => v.Grav25) ?? 0,
                Neto5 = ventas.Sum(v => v.Grav5) ?? 0,
                

                Iva27 = ventas.Sum(v => v.Iva27) ?? 0,
                Iva21 = ventas.Sum(v => v.Iva21) ?? 0,
                Iva105 = ventas.Sum(v => v.Iva105) ?? 0,
                Iva0 = ventas.Sum(v => v.Iva0) ?? 0,
                Iva25 = ventas.Sum(v => v.Iva25) ?? 0,
                Iva5 = ventas.Sum(v => v.Iva5) ?? 0,

                NoGravado = ventas.Sum(v => v.NoGravado) ?? 0,
                Exento = ventas.Sum(v => v.Exento) ?? 0,
            };

            return totalesIVA;
        }


        public static TotalesIVA CalcularTotales(IEnumerable<Compra> compras)
        {
            // poner negativo los valores de las notas de crédito
            foreach (var v in compras)
            {
                RestarNotasCredito(v);
            }

            var totalesIVA = new TotalesIVA
            {
                Neto0 = compras.Sum(c => c.Grav0 ?? 0),
                Neto25 = compras.Sum(c => c.Grav25 ?? 0),
                Neto5 = compras.Sum(c => c.Grav5 ?? 0),
                Neto105 = compras.Sum(c => c.Grav105 ?? 0),
                Neto21 = compras.Sum(c => c.Grav21 ?? 0),
                Neto27 = compras.Sum(c => c.Grav27 ?? 0),

                Iva0 = compras.Sum(c => c.Iva0 ?? 0),
                Iva25 = compras.Sum(c => c.Iva25 ?? 0),
                Iva5 = compras.Sum(c => c.Iva5 ?? 0),
                Iva105 = compras.Sum(c => c.Iva105 ?? 0),
                Iva21 = compras.Sum(c => c.Iva21 ?? 0),
                Iva27 = compras.Sum(c => c.Iva27 ?? 0),

                NoGravado = compras.Sum(c => c.NoGravado ?? 0),
                Exento = compras.Sum(c => c.Exento ?? 0),
            };

            return totalesIVA;
        }

        public static Dictionary<string, TotalesIVA> CalcularTotales(Dictionary<string, List<Venta>> ventasAgrupadas)
        {
            var totales = new Dictionary<string, TotalesIVA>();

            foreach (var grupo in ventasAgrupadas)
            {
                var tipo = grupo.Key;
                var ventas = grupo.Value;

                totales.Add(tipo, CalcularTotales(ventas));
            }

            return totales;
        }

        public static void RestarNotasCredito(Venta v)
        {
            var esNotaCredito = v.TipoFact.ToLower().Contains("crédito");
            if (esNotaCredito)
            {
                v.NetoGravado = -v.NetoGravado;
                v.Iva = -v.Iva;
            }
        }







        public static Dictionary<string, TotalesIVA> CalcularTotalesDict(IEnumerable<Compra> compras)
        {
            var totalesAgrupadosPorTipoFactura = new Dictionary<string, TotalesIVA>();

            foreach (var compra in compras)
            {
                RestarNotasCredito(compra); 

                if (!totalesAgrupadosPorTipoFactura.ContainsKey(compra.TipoFact))
                {
                    totalesAgrupadosPorTipoFactura[compra.TipoFact] = new TotalesIVA();
                }

                var totalesParaTipoFactura = totalesAgrupadosPorTipoFactura[compra.TipoFact];

                
                if (compra.NetoGravado.HasValue && compra.Iva.HasValue && compra.NetoGravado.Value != 0)
                {
                    decimal tasaCalculada = Math.Round((decimal)(compra.Iva.Value / compra.NetoGravado.Value), 2);
                    decimal tasaCalculada3Digitos = Math.Round((decimal)(compra.Iva.Value / compra.NetoGravado.Value), 3);

                    if (tasaCalculada == 0.21m)
                    {
                        totalesParaTipoFactura.Neto21 += compra.NetoGravado.Value;
                        totalesParaTipoFactura.Iva21 += compra.Iva.Value;
                    }
                    else if (tasaCalculada3Digitos == 0.105m)
                    {
                        totalesParaTipoFactura.Neto105 += compra.NetoGravado.Value;
                        totalesParaTipoFactura.Iva105 += compra.Iva.Value;
                    }
                    //else
                    //{
                    //    totalesParaTipoFactura.NetoOtros += compra.NetoGravado.Value;
                    //    totalesParaTipoFactura.IvaOtros += compra.Iva.Value;
                    //}
                }

                // Sumar NoGravado y Exento a las nuevas propiedades de TotalesIVA
                if (compra.NoGravado.HasValue) totalesParaTipoFactura.NoGravado += compra.NoGravado.Value;
                if (compra.Exento.HasValue) totalesParaTipoFactura.Exento += compra.Exento.Value;

                // Sumar el Total directamente a la nueva propiedad TotalGeneral
                if (compra.Total.HasValue)
                {
                    //totalesParaTipoFactura.TotalGeneral += compra.Total.Value;
                }
            }

            return totalesAgrupadosPorTipoFactura;
        }

        
        public static void RestarNotasCredito(Compra c)
        {
            var esNotaCredito = c.TipoFact.ToLower().Contains("crédito");
            if (esNotaCredito)
            {
                
                if (c.NetoGravado.HasValue) c.NetoGravado = -c.NetoGravado.Value;
                if (c.Iva.HasValue) c.Iva = -c.Iva.Value;
                if (c.NoGravado.HasValue) c.NoGravado = -c.NoGravado.Value;
                if (c.Exento.HasValue) c.Exento = -c.Exento.Value;
                if (c.Total.HasValue) c.Total = -c.Total.Value;
            }
        }







    }
}
