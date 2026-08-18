using Microsoft.EntityFrameworkCore;
using ServiciosEC.Interfaces;
using ServiciosEC.Models;
using ServiciosEC.Utilidades;
using ServiciosEC.Utilidades.ModelosDTO;
using ServiciosEC.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.ExceptionServices;


namespace ServiciosEC.Managers
{
    public class VentaManager : IVentaManager
    {
        private IExcelDataHandler _excelDataHandler;
        private readonly ECContext _context;

        public VentaManager(ECContext context, IExcelDataHandler excelDataHandler)
        {
            _excelDataHandler = excelDataHandler;
            _context = context;
        }

        
        public async Task<(IEnumerable<Venta> VentasCorrectas, IEnumerable<Venta> VentasParaRevisar, IEnumerable<Venta> VentasFallidas, bool excelValido)> AgregarVentas(Stream stream, int idPersona, string cuit, CancellationToken cancellationToken)
        {
            

            var (list, primeraCelda) = _excelDataHandler.ImportarVentas(stream, idPersona);
            bool excelValido = true;

            List<Venta> ventasParaRevisar = new List<Venta>();
            List<Venta> ventasCorrectas = new List<Venta>();
            List<Venta> ventasFallidas = new List<Venta>();

            if (primeraCelda.Contains("Ventas", StringComparison.OrdinalIgnoreCase) && primeraCelda.Contains($"{cuit}", StringComparison.OrdinalIgnoreCase))
            {
                foreach (Venta venta in list)
                {
                    //Comprueba que la factura no este ya registrada en la base de datos.
                    if(await facturaExistente(venta, cancellationToken))
                    {
                        ventasFallidas.Add(venta);
                        continue;
                    }
                    
                    if (await IVaValido(venta, cancellationToken))
                    {
                        await AsignarMontoIva(venta, cancellationToken);

                        ventasCorrectas.Add(venta);

                        await Insertar(venta, cancellationToken);
                        continue;
                    }
                    else
                    {
                        //si el iva no es valido ,
                        ventasParaRevisar.Add(venta);
                        await Insertar(venta, cancellationToken);
                        continue;
                    }

                    
                }

                return (ventasCorrectas, ventasParaRevisar, ventasFallidas, excelValido);
            }
            else
            {
                excelValido = false;
                return (ventasCorrectas, ventasParaRevisar, ventasFallidas, excelValido);
            }


        }
        /// <summary>
        /// Realiza una Busqueda en base de datos de una factura en particular. Los parametros de busqueda son, id , PuntoVenta y NumeroDesde
        /// Si existe una factura que repita esos 3 valores al mismo tiempo, retorna True y si no retorna False.
        /// </summary>
        /// <param name="venta"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task<bool> facturaExistente(Venta venta, CancellationToken cancellationToken)
        {
            bool existe = false;

            var factura = await _context.Ventas
                .Where(v => v.EstadoId == (int)ECContext.EstadosEnum.Activo
                    && v.IdPersona  == venta.IdPersona && v.PuntoVenta == venta.PuntoVenta && v.NroDesde == venta.NroDesde)
                .FirstOrDefaultAsync(cancellationToken);

            if (factura != null)
            {
                existe = true;
            }

            return existe;

        }

        public IEnumerable<Venta> ObtenerPorClienteYTipoFact(Cliente cliente, string TipoFact = "")
        {
            //var ventas = _context.Ventas
            //    .Where(v => v.ClienteId == cliente.IdCliente && v.NetoGravado != null && v.NetoGravado != 0)
            //    .Where(v => v.TipoFact.Contains(TipoFact.Trim()))
            //    .ToList();

            //return ventas;
            throw new NotImplementedException("El método ObtenerPorClienteYTipoFact no está implementado.");
        }

        public async Task Insertar(Venta venta, CancellationToken cancellationToken)
        {
            _context.Ventas.Add(venta);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Venta>> ObtenerTodos(CancellationToken cancellationToken)
        {
            return await _context.Ventas
                .Where(v => v.EstadoId == (int)ECContext.EstadosEnum.Activo)
                .ToListAsync(cancellationToken);
        }

        public async Task<Venta> ObtenerPorId(int id, CancellationToken cancellationToken)
        {
            var venta = await _context.Ventas
                .Where(v => v.EstadoId == (int)ECContext.EstadosEnum.Activo)
                .FirstOrDefaultAsync(v => v.IdVenta == id, cancellationToken);

            if (venta == null)
            {
                throw new KeyNotFoundException($"No se encontró la venta con id: {id}");
            }
            return venta;
        }

        public async Task Editar(Venta entidad, CancellationToken cancellationToken)
        {
            _context.Entry(entidad).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Borrar(int id, CancellationToken cancellationToken)
        {
            var venta = await ObtenerPorId(id, cancellationToken);
            if (venta != null)
            {

                _context.Ventas.Remove(venta);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }


        /// <param name="idPersona">ID de la persona (cliente) a filtrar.</param>
        /// <param name="mes">Mes para filtrar (1-12). Si es 0, no filtra por mes.</param>
        /// <param name="ano">Año para filtrar. Si es 0, no filtra por año.</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Colección de ventas que coinciden con los criterios.</returns>
        public async Task<IEnumerable<Venta>> ObtenerVentasPorClienteYPeriodoAsync(int idPersona, int mes, int ano, CancellationToken cancellationToken)
        {
            IQueryable<Venta> query = _context.Ventas
                .Where(v => v.IdPersona == idPersona && v.EstadoId == (int)ECContext.EstadosEnum.Activo);

            if (ano > 0)
            {
                query = query.Where(v => v.Fecha.Year == ano);
            }

            if (mes > 0 && mes <= 12)
            {
                query = query.Where(v => v.Fecha.Month == mes);
            }


            return await query.OrderByDescending(v => v.Fecha).ToListAsync(cancellationToken);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="mes">Si mes = 0, trae todas las ventas del año.
        /// Si mes != 0, trae solo las del mes y año indicados</param>
        /// <param name="ano">año</param>
        /// <returns>Devuelve ventas agrupadas por TipoFact sin espacios ni diferencias de mayúsculas</returns>
        public Dictionary<string, List<Venta>> ObtenerPorClienteAgrupadas(Cliente cliente, int mes, int ano)
        {
            IQueryable<Venta> query = _context.Ventas
                .Where(v => v.IdPersona == cliente.IdPersona
                    //&& v.NetoGravado != null && v.NetoGravado != 0
                    && v.Fecha.Year == ano
                    && v.EstadoId == (int)ECContext.EstadosEnum.Activo); 


            //si el mes es 0 devuelve las ventas de todo el año
            if (mes != 0)
                query = query.Where(v => v.Fecha.Month == mes);


            var ventas = query
                .GroupBy(v => v.TipoFact.Trim().ToUpper())
                .ToDictionary(g => g.Key, g => g.ToList());

            return ventas;
        }


        /// <summary>
        /// Si el mes es 0, se consideran todas las ventas del año.
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="mes">Si el mes es 0, se consideran todas las ventas del año.</param>
        /// <param name="ano"></param>
        /// <returns>Devuelve el neto gravado de un periodo</returns>
        public decimal ObtenerNetoGravadoVentas(Cliente cliente, int mes, int ano)
        {
            IQueryable<Venta> query = _context.Ventas
                .Where(v => v.IdPersona == cliente.IdPersona
                    && v.NetoGravado.HasValue 
                    && v.Fecha.Year == ano
                    && v.EstadoId == (int)ECContext.EstadosEnum.Activo); 

            if (mes != 0)
                query = query.Where(v => v.Fecha.Month == mes);


            decimal totalNetoGravado = query.Sum(v => v.NetoGravado ?? 0m); 

            return totalNetoGravado;
        }


        public async Task<decimal> CalcularTotalNetoAsync(int idPersona, int mes, int ano, CancellationToken cancellationToken)
        {
            var ventas = await ObtenerVentasPorClienteYPeriodoAsync(idPersona, mes, ano, cancellationToken);


            decimal totalNetoGravado = ventas
                                        //.Where(v => v.NetoGravado.HasValue) 
                                        .Sum(v => v.NetoGravado ?? 0m);

            return totalNetoGravado;
        }



        public async Task<IEnumerable<TotalesPorComprobanteDTO>> CalcularTotalesPorComprobanteAsync(int idPersona, int mes, int ano, CancellationToken cancellationToken)
        {
            var ventas = await ObtenerVentasPorClienteYPeriodoAsync(idPersona, mes, ano, cancellationToken);

            var totalesPorComprobante = ventas
                .GroupBy(v => v.TipoFact.Trim().ToUpper())
                .Select(g => new TotalesPorComprobanteDTO
                {
                    TipoComprobante = g.Key,
                    TotalNetoGravado = g.Sum(v => v.NetoGravado ?? 0m), 
                    TotalIVA = g.Sum(v => v.Iva ?? 0m), 
                    TotalGeneral = g.Sum(v => v.Total ?? 0m) 
                })
                .OrderBy(t => t.TipoComprobante) 
                .ToList();

            return totalesPorComprobante;
        }

        /// <summary>
        /// Obtiene el saldo de jurisdiccion , que corresponde al periodo inmediatamente anterior.
        /// </summary>
        /// <param name="idPersona">id del cliente</param>
        /// <param name="anioActual">año actual</param>
        /// <param name="periodoActual">periodo o mes actual</param>
        /// <param name="jurisdiccionId">Jurisdiccion</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<decimal> GetSaldoAnteriorAsync(int idPersona, int periodoActual, int anioActual, int jurisdiccionId, CancellationToken cancellationToken)
        {

            if (periodoActual > 1)
            {
                var saldo = await _context.Ingresosbrutos
                    .Where(ib => ib.IdPersona == idPersona
                                 && ib.Anio == anioActual
                                 && ib.Periodo < periodoActual
                                 && ib.JurisdiccionId == jurisdiccionId)
                    .OrderByDescending(ib => ib.Periodo)
                    .ThenByDescending(ib => ib.FechaDeclaracion)
                    .ThenByDescending(ib => ib.IdIngresosbrutos)
                    .Select(ib => (decimal?)ib.Saldo) // Saldo es decimal? en el modelo, este cast es redundante pero inofensivo
                    .FirstOrDefaultAsync(cancellationToken);

                if (saldo.HasValue)
                    return saldo.Value; //si encuentra retorna
            }

            // obtener todos los años de esa persona
            var aniosAnteriores = await _context.Ingresosbrutos
                .Where(ib => ib.IdPersona == idPersona && ib.Anio < anioActual)
                .Select(ib => ib.Anio)
                .Distinct()
                .OrderByDescending(a => a)
                .ToListAsync(cancellationToken);

            // buscar el periodo mas alta de los años anteriores y obteener el primer valor que se enceuntre que corresponda a la misma juridiccion
            foreach (var anio in aniosAnteriores)
            {
                var saldo = await _context.Ingresosbrutos
                    .Where(ib => ib.IdPersona == idPersona && ib.Anio == anio && ib.JurisdiccionId == jurisdiccionId)
                    .OrderByDescending(ib => ib.Periodo)
                    .ThenByDescending(ib => ib.FechaDeclaracion)
                    .ThenByDescending(ib => ib.IdIngresosbrutos)
                    .Select(ib => (decimal?)ib.Saldo) // Saldo es decimal?
                    .FirstOrDefaultAsync(cancellationToken);

                if (saldo.HasValue)
                    return saldo.Value;
            }

            // si no hay saldo retorna 0
            return 0;
        }

        public async Task GenerarIngresosBrutosMensual(Ingresosbrutos ingresosbrutos, Cliente cliente, CancellationToken cancellationToken)
        {
         
            ingresosbrutos.GravadoJurisdiccion = ingresosbrutos.GravadoPais * ingresosbrutos.Coeficiente;

            decimal saldoAnterior = await GetSaldoAnteriorAsync(ingresosbrutos.IdPersona, ingresosbrutos.Periodo, ingresosbrutos.Anio, ingresosbrutos.JurisdiccionId, cancellationToken);

           
            decimal deducciones = (ingresosbrutos.Retenciones ?? 0m) + (ingresosbrutos.RetencionesBancarias ?? 0m) + (ingresosbrutos.Percepciones ?? 0m) + (ingresosbrutos.Aduaneras ?? 0m);

           
            ingresosbrutos.Saldo = (ingresosbrutos.GravadoJurisdiccion ?? 0m) * (ingresosbrutos.Alicuota ?? 0m) - deducciones - saldoAnterior;

            _context.Ingresosbrutos.Add(ingresosbrutos);
            await _context.SaveChangesAsync(cancellationToken);

        }

        /// <summary>
        /// Obtiene el coeficiente acumulado para el mes y año especificados. El coeficiente total del Mes tomando en cuenta 
        /// todas las jurisdicciones, con su registro mas actual, debe ser menor o igual a 1.
        /// </summary>
        /// <param name="ingresosbrutos"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<decimal> ObtenerCoeficienteAcumulado(Ingresosbrutos ingresosbrutos, CancellationToken cancellationToken)
        {
            var registrosMensuales = await GetAllIngresosbrutosMensual(ingresosbrutos.IdPersona, ingresosbrutos.Periodo, ingresosbrutos.Anio, cancellationToken);

            var coeficienteAcumulado = registrosMensuales
                .Where(ib=> ib.JurisdiccionId != ingresosbrutos.JurisdiccionId)
                .Sum(ib => ib.Coeficiente);
            return coeficienteAcumulado;


        }

        /// <summary>
        /// obtiene un registro de ingresos brutos mensual, dada una jurisdiccion
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="periodo"></param>
        /// <param name="anio"></param>
        /// <param name="jurisdiccionId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Ingresosbrutos> GetIngresosBrutosMensual(int idPersona, int periodo, int anio, int jurisdiccionId, CancellationToken cancellationToken)
        {
            return await _context.Ingresosbrutos
                .Where(ib => ib.IdPersona == idPersona)
                .Where(ib => ib.Periodo == periodo && ib.Anio == anio && ib.JurisdiccionId == jurisdiccionId)
                .FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Obtiene una lista de ingresos brutos mensuales. Solo los mas actuales.
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="periodo"></param>
        /// <param name="anio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Ingresosbrutos>> GetAllIngresosbrutosMensual(int idPersona, int periodo, int anio, CancellationToken cancellationToken)
        {
            var listaMensual = new List<Ingresosbrutos>();
            Ingresosbrutos? registroMensuaL = new Ingresosbrutos();

            //Busca las jurisdicciones del mes. Sin duplicados
            var jurisdicciones = await _context.Ingresosbrutos
                .Where(ib => ib.IdPersona == idPersona && ib.Anio == anio && ib.Periodo == periodo)
                .Select(ib => ib.JurisdiccionId)
                .Distinct()
                .ToListAsync(cancellationToken);

            //para cada jurisdiccion encontrada elige el registro mas actual y lo agrega a la lista
            foreach (var jurisdiccion in jurisdicciones)
            {
                registroMensuaL = await _context.Ingresosbrutos
                .Where(ib => ib.IdPersona == idPersona && ib.Anio == anio && ib.Periodo == periodo && ib.JurisdiccionId == jurisdiccion)
                .OrderByDescending(ib => ib.FechaDeclaracion)
                .ThenByDescending(ib => ib.IdIngresosbrutos)
                .FirstOrDefaultAsync(cancellationToken);

                if (registroMensuaL != null)
                {
                    listaMensual.Add(registroMensuaL);
                }

            }
            return listaMensual;

            
        }

        /// <summary>
        /// Obtiene un registro de ingresos brutos como resumen mensual correspondiente al periodo y año especificado.
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="periodo"></param>
        /// <param name="anio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Ingresosbrutos> TotalIngresosBrutosMensual(int idPersona, int periodo, int anio, CancellationToken cancellationToken)
        {
            IEnumerable<Ingresosbrutos> lista = await GetAllIngresosbrutosMensual(idPersona, periodo, anio, cancellationToken);
            Ingresosbrutos totalIbMensual = new Ingresosbrutos();

            totalIbMensual.IdIngresosbrutos = 0;
            totalIbMensual.IdPersona = idPersona;
            totalIbMensual.Anio = anio;
            totalIbMensual.Periodo = periodo;
            totalIbMensual.JurisdiccionId = 0;
            
            totalIbMensual.GravadoPais = lista.Select(ib => ib.GravadoPais).FirstOrDefault() ?? 0m;
            totalIbMensual.Coeficiente = lista.Sum(ib => ib.Coeficiente); 
            totalIbMensual.GravadoJurisdiccion = totalIbMensual.GravadoPais; 
            totalIbMensual.Alicuota = lista.Select(ib => ib.Alicuota).FirstOrDefault() ?? 0m;
            totalIbMensual.ImpuestoDeterminado = lista.Sum(ib => ib.ImpuestoDeterminado ?? 0m); 
            totalIbMensual.Retenciones = lista.Sum(ib => ib.Retenciones ?? 0m); 
            totalIbMensual.RetencionesBancarias = lista.Sum(ib => ib.RetencionesBancarias ?? 0m); 
            totalIbMensual.Percepciones = lista.Sum(ib => ib.Percepciones ?? 0m); 
            totalIbMensual.Aduaneras = lista.Sum(ib => ib.Aduaneras ?? 0m); 
            totalIbMensual.Saldo = lista.Sum(ib => ib.Saldo ?? 0m); 

            return totalIbMensual;

        }
        /// <summary>
        /// Obtiene una lista de totales mensuales de ingresos brutos, correspondiente a un año especificado.
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="periodo"></param>
        /// <param name="anio"></param>
        /// <param name="cancellation"></param>
        /// <returns></returns>
        public async Task<IEnumerable<Ingresosbrutos>> GetTotalesIbMensual(int idPersona, int anio, CancellationToken cancellation)
        {
            //obtiene los periodos del año que registran ingresos brutos,
            var periodos = await ObtenerPeriodos(idPersona, anio, cancellation);

            var listaTotales = new List<Ingresosbrutos>();

            foreach (var periodo in periodos)
            {
                var totalIbMensual = await TotalIngresosBrutosMensual(idPersona, periodo, anio, cancellation);

                listaTotales.Add(totalIbMensual);

            }

            return listaTotales;


        }

        /// <summary>
        /// Obtiene los periodos en los que el cliente registra ingresos brutos dado un año
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="anio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<IEnumerable<int>> ObtenerPeriodos(int idPersona, int anio, CancellationToken cancellationToken)
        {
            return await _context.Ingresosbrutos
                    .Where(ib => ib.IdPersona == idPersona && ib.Anio == anio)
                    .Select(ib => ib.Periodo)
                    .Distinct() //evitar los duplicados por jurisdiccion
                    .OrderBy(p => p)
                    .ToListAsync(cancellationToken);
        }


        /// <summary>
        /// Obtiene Todos los registros de ingresos brutos en un año en particular. Solo los mas actuales
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="anio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// 
        public async Task<IEnumerable<Ingresosbrutos>> GetIngresosbrutosAnual(int idPersona, int anio, CancellationToken cancellationToken)
        {
            var listaAnual = new List<Ingresosbrutos>();
            Ingresosbrutos? registroAnual = new Ingresosbrutos();

            //obtenemos los periodos en los que se registra ingresos brutos
            var periodos = await ObtenerPeriodos(idPersona, anio, cancellationToken);

            foreach (var periodo in periodos)
            {
                var listaMensual = await GetAllIngresosbrutosMensual(idPersona, periodo, anio, cancellationToken);

                //agrega la lista con objetos mensuales a la lista anual
                listaAnual.AddRange(listaMensual);

            }
            

            return listaAnual;

            //return await _context.Ingresosbrutos
            //    .Where(ib => ib.IdPersona == idPersona && ib.Anio == anio)
            //    .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Obtiene un objeto de ingresos brutos como total del año.
        /// </summary>
        /// <param name="idPersona"></param>
        /// <param name="anio"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<Ingresosbrutos> TotalIngresosBrutosAnual(int idPersona, int anio, CancellationToken cancellationToken)
        {
            IEnumerable<Ingresosbrutos> lista = await GetIngresosbrutosAnual(idPersona, anio, cancellationToken);

            Ingresosbrutos totalIbAnual = new Ingresosbrutos();
            totalIbAnual.IdIngresosbrutos = 0;
            totalIbAnual.IdPersona = idPersona;
            totalIbAnual.Anio = anio;
            totalIbAnual.Periodo = 0;
            totalIbAnual.JurisdiccionId = 0;
            totalIbAnual.GravadoPais = lista.Sum(ib => ib.GravadoJurisdiccion ?? 0m); 
            totalIbAnual.Coeficiente = 1; 
            totalIbAnual.GravadoJurisdiccion = totalIbAnual.GravadoPais;
            totalIbAnual.Alicuota = lista.Select(ib => ib.Alicuota).FirstOrDefault() ?? 0m;
            totalIbAnual.ImpuestoDeterminado = lista.Sum(ib => ib.ImpuestoDeterminado ?? 0m); 
            totalIbAnual.Retenciones = lista.Sum(ib => ib.Retenciones ?? 0m);
            totalIbAnual.RetencionesBancarias = lista.Sum(ib => ib.RetencionesBancarias ?? 0m); 
            totalIbAnual.Percepciones = lista.Sum(ib => ib.Percepciones ?? 0m); 
            totalIbAnual.Aduaneras = lista.Sum(ib => ib.Aduaneras ?? 0m); 
            totalIbAnual.Saldo = lista.Sum(ib => ib.Saldo ?? 0m); 

            return totalIbAnual;
        }

        public List<Venta> ObtenerPorCliente(Cliente cliente)
        {
            return _context.Ventas
                .Where(v => v.IdPersona == cliente.IdPersona && v.EstadoId == (int)ECContext.EstadosEnum.Activo)
                .ToList();
        }

        public async Task<(TotalesIVA, TotalesIVA)> ObtenerIVAPorClienteYPeriodo(Cliente cliente, int mes, int ano)
        {
            IQueryable<Venta> query = _context.Ventas
                .Where(v => v.IdPersona == cliente.IdPersona
                    //&& v.NetoGravado != null && v.NetoGravado != 0 
                    && v.Fecha.Year == ano
                    && v.EstadoId == (int)ECContext.EstadosEnum.Activo);


            //si el mes es 0 devuelve las ventas de todo el año
            if (mes != 0)
                query = query.Where(v => v.Fecha.Month == mes);


            var ventas = await query.ToListAsync();

            TotalesIVA TotalDebitoFiscal = Calculadora
                .CalcularTotales(ventas.Where(v => !v.TipoFact.ToLower().Contains("crédito")));

            TotalesIVA TotalRestitucionDebitoFiscal = Calculadora
                .CalcularTotales(ventas.Where(v => v.TipoFact.ToLower().Contains("crédito")));

            return (TotalDebitoFiscal, TotalRestitucionDebitoFiscal);
        }

        public async Task<ResumenIIGGMensual> ObtenerIVAMensualPorCliente(Cliente cliente, int año)
        {
            var ventas = await _context.Ventas
                .Where(v => v.IdPersona == cliente.IdPersona && v.Fecha.Year == año && v.EstadoId == (int)ECContext.EstadosEnum.Activo) // Añadido EstadoId == Activo para consistencia
                .ToListAsync();

            // netos e iva por mes
            var totalesPorMes = Enumerable.Range(1, 12)
                .Select(mes =>
                {
                    var ventasDelMes = ventas.Where(v => v.Fecha.Month == mes);
                    return Calculadora.CalcularTotales(ventasDelMes);
                })
                .ToList();

            // suma pra total anual
            var totalAnual = totalesPorMes.Aggregate(new TotalesIVA(), (acc, t) => acc + t);

            return new ResumenIIGGMensual
            {
                PorMes = totalesPorMes,
                TotalAnual = totalAnual
            };
        }



        public async Task<bool> IVaValido(Venta venta, CancellationToken cancellationToken)
        {
            bool ivaValido = false;
            const decimal tolerancia = 0.1m;

            var ivasAceptables = await _context.Ivas.ToListAsync(cancellationToken);

       
            if (venta.Iva.HasValue && venta.NetoGravado.HasValue && venta.NetoGravado.Value != 0)
            {
                decimal porcentajeCalculado = (venta.Iva.Value / venta.NetoGravado.Value) * 100;

                foreach (var iva in ivasAceptables)
                {
                    if (decimal.Abs(porcentajeCalculado - iva.Porcentaje) < tolerancia)
                    {
                        //venta.IdIva = iva.IdIva;
                        ivaValido = true;
                        break;
                    }
                }
            }

            return ivaValido;
        }

        public async Task<bool> ValidarTotales(Venta venta, CancellationToken cancellationToken)
        {
           
            if (venta.Total.HasValue && venta.NetoGravado.HasValue && venta.Iva.HasValue && venta.Exento.HasValue && venta.NoGravado.HasValue)
            {
                const decimal tolerancia = 0.01m;
                
                bool totalValido = Math.Abs((venta.NetoGravado.Value + venta.Iva.Value + venta.Exento.Value + venta.NoGravado.Value) - venta.Total.Value) < tolerancia;
                return totalValido; 
            }
            return false;
        }

        public async Task<bool> ValidacionIvaGravadoDeglosado(Venta venta, CancellationToken cancellationToken)
        {
            
            if (venta.Iva0.HasValue && venta.Iva25.HasValue && venta.Iva5.HasValue && venta.Iva105.HasValue && venta.Iva21.HasValue && venta.Iva27.HasValue &&
                venta.Grav0.HasValue && venta.Grav25.HasValue && venta.Grav5.HasValue && venta.Grav105.HasValue && venta.Grav21.HasValue && venta.Grav27.HasValue &&
                venta.Iva.HasValue && venta.NetoGravado.HasValue) 
            {
                const decimal tolerancia = 0.01m;
                
                bool IvaValido = Math.Abs((venta.Iva0.Value + venta.Iva25.Value + venta.Iva5.Value + venta.Iva105.Value + venta.Iva21.Value + venta.Iva27.Value) - venta.Iva.Value) < tolerancia;
               
                bool NetoValido = Math.Abs((venta.Grav0.Value + venta.Grav25.Value + venta.Grav5.Value + venta.Grav105.Value + venta.Grav21.Value + venta.Grav27.Value) - venta.NetoGravado.Value) < tolerancia;
                return IvaValido && NetoValido;
            }
            return false;
        }
        public async Task<Venta> AsignarMontoIva(Venta venta, CancellationToken cancellationToken)
        {
            // if (venta.NetoGravado == null || venta.Iva == null || venta.NetoGravado == 0) // Tu original.
            if (!venta.NetoGravado.HasValue || !venta.Iva.HasValue || venta.NetoGravado.Value == 0) // Equivalente y más claro con HasValue/Value
                return venta;

            decimal porcentajeCalculado = (venta.Iva.Value / venta.NetoGravado.Value) * 100;
            const decimal tolerancia = 0.1m;

            var ivasAceptables = await _context.Ivas.ToListAsync(cancellationToken);

            foreach (var iva in ivasAceptables)
            {
                if (decimal.Abs(porcentajeCalculado - iva.Porcentaje) < tolerancia)
                {
                    switch (iva.Porcentaje)
                    {
                        case 0.00m:
                            venta.Iva0 = venta.Iva;
                            venta.Grav0 = venta.NetoGravado;
                            break;
                        case 2.50m:
                            venta.Iva25 = venta.Iva;
                            venta.Grav25 = venta.NetoGravado;
                            break;
                        case 5.00m:
                            venta.Iva5 = venta.Iva;
                            venta.Grav5 = venta.NetoGravado;
                            break;
                        case 10.50m:
                            venta.Iva105 = venta.Iva;
                            venta.Grav105 = venta.NetoGravado;
                            break;
                        case 21.00m:
                            venta.Iva21 = venta.Iva;
                            venta.Grav21 = venta.NetoGravado;
                            break;
                        case 27.00m:
                            venta.Iva27 = venta.Iva;
                            venta.Grav27 = venta.NetoGravado;
                            break;
                    }

                    break;
                }
            }

            return venta;
        }

        public Task<(IEnumerable<Venta> VentasAgregadas, IEnumerable<Venta> VentasFallidas)> AgregarVentas(string fileName, int idPersona, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> ObtenerCantidad(CancellationToken cancellationToken)
        {
            return await _context.Ventas.CountAsync(cancellationToken); // Agregué cancellationToken
        }

    }
}