using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ServiciosEC.Interfaces.Managers;
using Microsoft.EntityFrameworkCore.Internal;

namespace ServiciosEC.Managers
{
    public class IvaManager : IIvaManager
    {
        public readonly ECContext _context;
        public IvaManager(ECContext context)
        {
            _context = context;
        }

        public Task Borrar(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Editar(Iva entidad, CancellationToken cancellationToken)
        {
            _context.Entry(entidad).State = EntityState.Modified;
            return _context.SaveChangesAsync(cancellationToken);
        }

        public async Task Insertar(Iva entidad, CancellationToken cancellationToken)
        {
            _context.Ivas.Add(entidad);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public Task<int> ObtenerCantidad(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Devuelve un listado de tuplas ano - mes
        /// </summary>
        /// <param name="cliente"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<(DateOnly, IEnumerable<LibroIva>)>> ObtenerPeriodosPorClienteAsync(Cliente cliente, CancellationToken cancellationToken)
        {
            if (cliente == null)
            {
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");
            }

            var ventasPeriodos = _context.Ventas
                .Where(v => v.IdPersona == cliente.IdPersona)
                .Select(v => new { Año = v.Fecha.Year, Mes = v.Fecha.Month });
                //.Select(v => v.Fecha);

            var comprasPeriodos = _context.Compras
                .Where(c => c.IdPersona == cliente.IdPersona)
                .Select(c => new { Año = c.Fecha.Year, Mes = c.Fecha.Month });
                //.Select(c => c.Fecha);

            var libros = await _context.LibrosIva
                .Where(l => l.IdPersona == cliente.IdPersona)
                //.Select(l => new { l.Año, l.Mes })
                .ToListAsync();

            var periodos = await ventasPeriodos
                .Union(comprasPeriodos)
                .Distinct()
                .OrderBy(p => p.Año)
                .ThenBy(p => p.Mes)
                .ToListAsync();

            var periodosLibros = periodos
                .Select(p => (
                    fecha:new DateOnly(p.Año,p.Mes,1), 
                    libros:libros.Where(l => l.Año == p.Año && l.Mes == p.Mes)
                ));

            return periodosLibros;

            //return listadoSinRepetir
            //    .Select(p => (new DateOnly(p.Año,p.Mes,1), libros));
        }



        public async Task<Iva> ObtenerPorId(int id, CancellationToken cancellationToken)
        {
            var iva = await _context.Ivas.
                FirstOrDefaultAsync(i => i.IdIva == id, cancellationToken);

            if (iva == null)
            {
                throw new KeyNotFoundException($"No se encontró el IVA con id: {id}");
            }
            return iva;
        }

        public async Task<IEnumerable<Iva>> ObtenerTodos(CancellationToken cancellationToken)
        {
            return await _context.Ivas
                //.Where(await i => i.EstadoId == (int)ECContext.EstadosEnum.Activo)
                .ToListAsync(cancellationToken);
        }
    }
}
