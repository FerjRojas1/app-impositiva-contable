using ServiciosEC.Models;

namespace AppEstudioContable.Models
{
    public class VerNetoViewModel
    {
        public string Cuit { get; set; }
        public int Mes { get; set; }
        public int Año { get; set; }
        public decimal NetoGravado { get; set; }
    }
}
