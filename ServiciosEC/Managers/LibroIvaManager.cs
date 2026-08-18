using ServiciosEC.Interfaces.Managers;
using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Managers
{
    public class LibroIvaManager : ILibroIvaManager
    {
        private readonly ECContext _context;
        public LibroIvaManager(ECContext context)
        {
            _context = context;
        }
        public async Task Insertar(LibroIva entidad, CancellationToken cancellationToken)
        {
            _context.LibrosIva.Add(entidad);
            await _context.SaveChangesAsync(cancellationToken);

        }
        public Task Borrar(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task Editar(LibroIva entidad, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }


        public Task<int> ObtenerCantidad(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<LibroIva> ObtenerPorId(int id, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<LibroIva>> ObtenerTodos(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
