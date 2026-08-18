using ExcelDataReader;
using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Interfaces
{
    public interface IExcelDataHandler 
    {
        
        (IEnumerable<Venta>, string primerValor) ImportarVentas(Stream stream, int clienteId);

        (IEnumerable<Compra>, string primerValor) ImportarCompras(Stream stream, int clienteId);


    }
}
