using ServiciosEC.Interfaces;
using ServiciosEC.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Models
{
    public abstract partial class Persona : ISoftDeletable
    {
        //esta clase es necesaria para la auditoria, no borrar,
        //
    }
}
