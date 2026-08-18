using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiciosEC.Models;

namespace AppEstudioContable.Models
{
    public class VentaViewModel
    {
        public int id { get; set; }

        [BindNever]
        public List<Venta> ventasCorrectas { get; set; } = new();

        [BindNever]
        public List<Venta> ventasParaRevisar{ get; set; } = new();
        [BindNever]
        public List<Venta> ventasFallidas { get; set; } = new();


    }
}
