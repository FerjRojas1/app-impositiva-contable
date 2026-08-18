using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Middleware
{
    public class AuditoriaTemporal
    {
        public string Tabla { get; set; } = "";
        public string Accion { get; set; } = "";
        public int IdPersona { get; set; }
        public string? DatosAntes { get; set; }
        public string? DatosDespues { get; set; }
    }
}
