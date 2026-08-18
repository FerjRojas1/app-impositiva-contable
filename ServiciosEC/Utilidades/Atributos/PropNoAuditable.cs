using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Utilidades
{
    [AttributeUsage(AttributeTargets.Property)]
    public class PropNoAuditableAttribute : Attribute
    {
        //
    }
}
