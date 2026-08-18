using ServiciosEC.Models; 
using ServiciosEC.Utilidades; 
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace ServiciosEC.Managers 
{
    public class EstadoManager
    {
        private readonly ECContext _context;

        public EstadoManager(ECContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Estado>> ObtenerTodosAsync(CancellationToken cancellationToken)
        {
            
            return await _context.Estados.ToListAsync(cancellationToken);
        }

        
        public async Task<Estado> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Estados.FirstOrDefaultAsync(e => e.IdEstado == id, cancellationToken);
        }

       
    }
}