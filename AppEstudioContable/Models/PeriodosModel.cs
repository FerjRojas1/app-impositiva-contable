using ServiciosEC.Models;

namespace AppEstudioContable.Models
{
    public class PeriodosModel
    {
        public string Cuit { get; set; }
        public int Ano { get; set; }
        public int Mes { get; set; }
        public IEnumerable<LibroIva> Libros { get; internal set; }
    }
}
