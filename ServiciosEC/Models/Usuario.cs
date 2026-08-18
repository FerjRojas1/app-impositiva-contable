using ServiciosEC.Utilidades;
using System;
using System.Collections.Generic;

namespace ServiciosEC.Models;

public partial class Usuario : Persona
{

    public string NombreUsuario { get; set; } = null!;

    [PropNoAuditable]
    public string Contrasenia { get; set; } = null!;
}
