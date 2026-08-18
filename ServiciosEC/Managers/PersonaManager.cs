
using ServiciosEC.Models; 
using Microsoft.EntityFrameworkCore; 
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiciosEC.Managers
{
    public class PersonaManager
    {
        private readonly ECContext _context; 

        public PersonaManager(ECContext context)
        {
            _context = context;
        }

        
        public async Task<IEnumerable<Persona>> ObtenerClientesAsync(CancellationToken cancellationToken)
        {
          
            return await _context.Personas.ToListAsync(cancellationToken);
            
        }

      
        public async Task<Persona?> ObtenerPorId(int id, CancellationToken cancellationToken)
        {
            return await _context.Personas.FindAsync(new object[] { id }, cancellationToken);
        }
    }
}