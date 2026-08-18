using ExcelDataReader;
using Microsoft.SqlServer.Server;
using ServiciosEC.Interfaces;
using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ServiciosEC.Utilidades
{
    public class ExcelDataHandler : IExcelDataHandler
    {

        public (IEnumerable<Compra> , string primerValor) ImportarCompras(Stream stream, int clienteId)
        {
            var results = new List<Compra>();
            
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);


            string primeraCelda = string.Empty;

            try
            {
                
                using var reader = ExcelReaderFactory.CreateReader(stream);

                bool saltearLinea = true;
                int contador = 0;


                while (reader.Read())
                {
                    if (contador == 0)
                    {
                        primeraCelda = reader.GetValue(0).ToString() ?? string.Empty;
                        Debug.WriteLine(primeraCelda);
                        contador++;
                        continue;
                    }

                    if (saltearLinea || contador < 2)
                    {
                        saltearLinea = false;
                        contador++;
                        continue;
                    }

                    var formatosDeFecha = new[]
{
                        "d/M/yyyy",
                        "dd/M/yyyy",
                        "d/MM/yyyy",
                        "dd/MM/yyyy"
                    };

                    string sFecha = reader.GetValue(0)?.ToString() ?? string.Empty;
                    if (DateOnly.TryParseExact(sFecha, formatosDeFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                    {
                        Debug.WriteLine($"Fecha válida: {fecha}");
                    }
                    else
                    {
                        Debug.WriteLine($"Formato inválido: valor default {fecha}");
                        continue;
                    }

                    var compra = new Compra
                    {
                        IdPersona = clienteId,
                        Fecha = fecha,
                        TipoFact = reader.GetValue(1)?.ToString() ?? string.Empty,
                        PuntoVenta = int.TryParse(reader.GetValue(2)?.ToString(), out var pv) ? pv : 0,
                        NroDesde = int.TryParse(reader.GetValue(3)?.ToString(), out var nd) ? nd : 0,
                        NroHasta = int.TryParse(reader.GetValue(4)?.ToString(), out var nh) ? nh : 0,
                        TipoDocVendedor = reader.GetValue(5)?.ToString() ?? string.Empty,
                        NroDocVendedor = reader.GetValue(6)?.ToString() ?? string.Empty,
                        DenomVendedor = reader.GetValue(7)?.ToString() ?? string.Empty,
                        TipoCambio = int.TryParse(reader.GetValue(8)?.ToString(), out var tc) ? tc : 0,
                        Moneda = reader.GetValue(9)?.ToString() ?? string.Empty,
                        NetoGravado = decimal.TryParse(reader.GetValue(10)?.ToString(), out var ng) ? ng : 0m,
                        NoGravado = decimal.TryParse(reader.GetValue(11)?.ToString(), out var ngg) ? ngg : 0m,
                        Exento = decimal.TryParse(reader.GetValue(12)?.ToString(), out var ex) ? ex : 0m,
                        Iva = decimal.TryParse(reader.GetValue(13)?.ToString(), out var iva) ? iva : 0m,
                        Total = decimal.TryParse(reader.GetValue(14)?.ToString(), out var tot) ? tot : 0m
                    };

                    results.Add(compra);
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al importar archivo de compras: {ex.Message}");
            }
            

            return (results, primeraCelda);

        }

        
        public (IEnumerable<Venta>, string primerValor) ImportarVentas(Stream stream, int clienteId)
        {
            var results = new List<Venta>();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            string primeraCelda = string.Empty;

            try
            {
                //using var stream = File.Open(filePath, FileMode.Open, FileAccess.Read);
                using var reader = ExcelReaderFactory.CreateReader(stream);

                bool saltearLinea = true;
                int contador = 0;


                while (reader.Read())
                {
                    if (contador == 0)
                    {
                        primeraCelda = reader.GetValue(0).ToString() ?? string.Empty;
                        Debug.WriteLine(primeraCelda);
                        contador++;
                        continue;
                    }

                    if (saltearLinea || contador < 2)
                    {
                        saltearLinea = false;
                        contador++;
                        continue;
                    }

                    var formatosDeFecha = new[]
{
                        "d/M/yyyy",
                        "dd/M/yyyy",
                        "d/MM/yyyy",
                        "dd/MM/yyyy"
                    };

                    

                    string sFecha = reader.GetValue(0)?.ToString() ?? string.Empty;
                    if (DateOnly.TryParseExact(sFecha, formatosDeFecha, CultureInfo.InvariantCulture, DateTimeStyles.None, out var fecha))
                    {
                        Debug.WriteLine($"Fecha válida: {fecha}");
                    }
                    else
                    {
                        Debug.WriteLine($"Formato inválido: valor default {fecha}");
                        continue;
                    }

                    var venta = new Venta
                    {
                        IdPersona = clienteId,
                        Fecha = fecha,
                        TipoFact = reader.GetValue(1)?.ToString() ?? string.Empty,
                        PuntoVenta = int.TryParse(reader.GetValue(2)?.ToString(), out var pv) ? pv : 0,
                        NroDesde = int.TryParse(reader.GetValue(3)?.ToString(), out var nd) ? nd : 0,
                        NroHasta = int.TryParse(reader.GetValue(4)?.ToString(), out var nh) ? nh : 0,
                        TipoDocComprador = reader.GetValue(5)?.ToString() ?? string.Empty,
                        NroDocComprador = reader.GetValue(6)?.ToString() ?? string.Empty,
                        DenomComprador = reader.GetValue(7)?.ToString() ?? string.Empty,
                        TipoCambio = int.TryParse(reader.GetValue(8)?.ToString(), out var tc) ? tc : 0,
                        Moneda = reader.GetValue(9)?.ToString() ?? string.Empty,
                        NetoGravado = decimal.TryParse(reader.GetValue(10)?.ToString(), out var ng) ? ng : 0m,
                        NoGravado = decimal.TryParse(reader.GetValue(11)?.ToString(), out var ngg) ? ngg : 0m,
                        Exento = decimal.TryParse(reader.GetValue(12)?.ToString(), out var ex) ? ex : 0m,
                        Iva = decimal.TryParse(reader.GetValue(13)?.ToString(), out var iva) ? iva : 0m,
                        Total = decimal.TryParse(reader.GetValue(14)?.ToString(), out var tot) ? tot : 0m
                    };

                    results.Add(venta);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al importar archivo de Ventas: {ex.Message}");
            }
            

            return (results, primeraCelda);
        }

        
    }
}
