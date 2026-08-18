// En ServiciosEC/Utilidades/ModelosDTO/TotalesPorComprobanteDTO.cs

namespace ServiciosEC.Utilidades.ModelosDTO
{
    public class TotalesPorComprobanteDTO
    {
        public string TipoComprobante { get; set; }
        public decimal TotalNetoGravado { get; set; }
        public decimal TotalIVA { get; set; }
        public decimal TotalGeneral { get; set; }
    }
}