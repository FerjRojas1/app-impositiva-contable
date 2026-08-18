using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiciosEC.Models;

namespace AppEstudioContable.Models
{
    public class IngresosBrutosMensualModel
    {
        public int id { get; set; }
        public int? Periodo { get; set; }
        public int? Anio { get; set; }

        public List<Ingresosbrutos>? listaIbMensual { get; set; } = new();
        [BindNever]
        public Ingresosbrutos? ResumenMensual { get; set; }

    }
}
