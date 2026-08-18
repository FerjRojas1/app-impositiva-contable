using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiciosEC.Models;

namespace AppEstudioContable.Models
{
    public class CompraViewModel
    {
        public int id { get; set; }

        [BindNever]
        public List<Compra> comprasCorrectas { get; set; } = new();

        [BindNever]
        public List<Compra> comprasParaRevisar { get; set; } = new();
        [BindNever]
        public List<Compra> comprasFallidas { get; set; } = new();
        public IFormFile? ArchivoExcel { get; set; }
    }
}
