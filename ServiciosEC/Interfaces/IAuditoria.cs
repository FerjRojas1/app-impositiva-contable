using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Interfaces
{
    public interface IAuditoria
    {
        Task RegistrarAuditoriaAsync(string tabla, string accion, int IdPersona, string? datosAntes, string? datosDespues);
    }
}
