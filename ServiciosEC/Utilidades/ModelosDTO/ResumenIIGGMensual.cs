using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiciosEC.Utilidades.ModelosDTO
{
    public class ResumenIIGGMensual
    {
        public List<TotalesIVA> PorMes { get; init; } = [];  
        public TotalesIVA TotalAnual { get; init; }

    }
}
