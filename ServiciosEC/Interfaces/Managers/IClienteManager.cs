using ServiciosEC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Interfaces.Managers
{
    public interface IClienteManager : IManager<Cliente>
    {
        public Task<bool> Existe(int id, CancellationToken cancellationToken);

        public Task<bool> ExisteCliente(Cliente cliente, CancellationToken cancellationToken);

        public Task<Cliente?> ObtenerClientePorCuitAsync(string cuit, CancellationToken cancellationToken);

        public Task<IEnumerable<Cliente>> ObtenerTodosAsync(CancellationToken cancellationToken);

        public Task<IEnumerable<Cliente>> ObtenerInactivosAsync(CancellationToken cancellationToken);

        public Task<List<Estado>> ObtenerEstadosAsync(CancellationToken cancellationToken);



    }
}
