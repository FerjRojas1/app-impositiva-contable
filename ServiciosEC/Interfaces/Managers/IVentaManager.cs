using ServiciosEC.Models;
using ServiciosEC.Utilidades.ModelosDTO;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiciosEC.Interfaces.Managers
{
    public interface IVentaManager : IManager<Venta>
    {
        
        Task<(IEnumerable<Venta> VentasCorrectas, IEnumerable<Venta> VentasParaRevisar, IEnumerable<Venta> VentasFallidas, bool excelValido)> AgregarVentas(Stream stream, int idPersona, string cuit, CancellationToken cancellationToken);
        IEnumerable<Venta> ObtenerPorClienteYTipoFact(Cliente cliente, string TipoFact = "");
        Task<IEnumerable<Venta>> ObtenerVentasPorClienteYPeriodoAsync(int idPersona, int mes, int ano, CancellationToken cancellationToken);
        Dictionary<string, List<Venta>> ObtenerPorClienteAgrupadas(Cliente cliente, int mes, int ano);
        decimal ObtenerNetoGravadoVentas(Cliente cliente, int mes, int ano);
        Task<decimal> CalcularTotalNetoAsync(int idPersona, int mes, int ano, CancellationToken cancellationToken);
        Task<IEnumerable<TotalesPorComprobanteDTO>> CalcularTotalesPorComprobanteAsync(int idPersona, int mes, int ano, CancellationToken cancellationToken);
        Task<decimal> GetSaldoAnteriorAsync(int idPersona, int anioActual, int periodoActual, int jurisdiccionId, CancellationToken cancellationToken);
        Task GenerarIngresosBrutosMensual(Ingresosbrutos ingresosbrutos, Cliente cliente, CancellationToken cancellationToken);
        Task<Ingresosbrutos> GetIngresosBrutosMensual(int idPersona, int periodo, int anio, int jurisdiccionId, CancellationToken cancellationToken);
        Task<IEnumerable<Ingresosbrutos>> GetAllIngresosbrutosMensual(int idPersona, int periodo, int anio, CancellationToken cancellationToken);
        Task<Ingresosbrutos> TotalIngresosBrutosMensual(int idPersona, int periodo, int anio, CancellationToken cancellationToken);
        Task<IEnumerable<Ingresosbrutos>> GetTotalesIbMensual(int idPersona, int anio, CancellationToken cancellation);
        Task<IEnumerable<int>> ObtenerPeriodos(int idPersona, int anio, CancellationToken cancellationToken);
        Task<IEnumerable<Ingresosbrutos>> GetIngresosbrutosAnual(int idPersona, int anio, CancellationToken cancellationToken);
        Task<Ingresosbrutos> TotalIngresosBrutosAnual(int idPersona, int anio, CancellationToken cancellationToken);
        List<Venta> ObtenerPorCliente(Cliente cliente);
        Task<(TotalesIVA, TotalesIVA)> ObtenerIVAPorClienteYPeriodo(Cliente cliente, int mes, int ano);
        Task<ResumenIIGGMensual> ObtenerIVAMensualPorCliente(Cliente cliente, int año);
        Task<bool> IVaValido(Venta venta, CancellationToken cancellationToken);
        Task<bool> ValidarTotales(Venta venta, CancellationToken cancellationToken);
        Task<bool> ValidacionIvaGravadoDeglosado(Venta venta, CancellationToken cancellationToken);
        Task<Venta> AsignarMontoIva(Venta venta, CancellationToken cancellationToken);
        Task<decimal> ObtenerCoeficienteAcumulado(Ingresosbrutos ingresosbrutos, CancellationToken cancellationToken);
    }
}
