using ServiciosEC.Models;
using ServiciosEC.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Interfaces.Managers
{
    /// <summary>
    /// Es necesaria para test
    /// </summary>
    public interface IUsuariosManager : IManager<Usuario>
    {
        Task<Usuario?> ObtenerPorNombreAsync(string nombre, CancellationToken cancellationToken);
        Task<Usuario?> ObtenerPorEmailAsync(string email, CancellationToken cancellationToken);
        Task EditarSinContrasenia(Usuario usuario, CancellationToken cancellationToken);
        Task<DatosExistenciaUsuario> DatosExistentesAsync(Usuario usuario, CancellationToken cancellationToken);
        Task<List<Role>> ObtenerRoleUsuarioAsync(CancellationToken cancellationToken);
        Task<IEnumerable<Usuario>> Buscar(string filtroBusqueda, CancellationToken cancellationToken);
        bool ValidarContraseña(Usuario u, string contraseniaIngresada);

        Task<IEnumerable<Usuario>> ObtenerInactivosAsync(CancellationToken cancellationToken);
        Task <IEnumerable<Auditoria>>MostrarActividad(CancellationToken cancellationToken);
    }

}
