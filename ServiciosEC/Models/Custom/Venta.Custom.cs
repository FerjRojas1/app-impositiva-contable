using ServiciosEC.Interfaces;
using ServiciosEC.Utilidades.Atributos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Models
{
    [ClaseNoAuditable]
    public partial class Venta : /*IAuditable,*/ ISoftDeletable
    {
        //esta clase es necesaria para la auditoria, no borrar,
        //
    }
}
