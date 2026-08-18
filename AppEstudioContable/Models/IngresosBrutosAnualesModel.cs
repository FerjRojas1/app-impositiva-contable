using Microsoft.AspNetCore.Mvc.ModelBinding;
using ServiciosEC.Models;

namespace AppEstudioContable.Models
{
    public class IngresosBrutosAnualesModel
    {

        public int id { get; set; }
        public int? Anio { get; set; }

        //public List<IngresosBrutosMensualModel>? listaDetalleIbMensual { get; set; } = new();

        public List<Ingresosbrutos>? listaAnual { get; set; } = new();

        public List<Ingresosbrutos>? listaIbTotalesPorMes { get; set; } = new();
        [BindNever]
        public Ingresosbrutos? ResumenAnual { get; set; }

    }
}
