
namespace ServiciosEC.Models.DTOs 
{
    public class DatosExistenciaUsuario
    {
        public bool Existe { get; set; }
        public string? Campo { get; set; } 
        public int? IdExistente { get; set; } 
    }
}