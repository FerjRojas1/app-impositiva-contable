using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Utilidades.Atributos
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ClaseNoAuditableAttribute : Attribute
    {
        //
    }
}
