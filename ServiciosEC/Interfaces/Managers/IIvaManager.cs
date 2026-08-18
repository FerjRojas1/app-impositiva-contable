using ServiciosEC.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ServiciosEC.Interfaces.Managers
{
    public interface IIvaManager : IManager<Iva>
    {
        Task<IEnumerable<(DateOnly, IEnumerable<LibroIva>)>> ObtenerPeriodosPorClienteAsync(Cliente cliente, CancellationToken cancellationToken);
    }
}
