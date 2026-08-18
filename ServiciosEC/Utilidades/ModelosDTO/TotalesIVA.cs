namespace ServiciosEC.Utilidades.ModelosDTO
{
    public class TotalesIVA
    {
        //netos gravados
        public decimal Neto27 { get; set; }
        public decimal Neto21 { get; set; }
        public decimal Neto105 { get; set; }
        public decimal Neto0 { get; set; }
        public decimal Neto25 { get; set; }
        public decimal Neto5 { get; set; }


        //ivas
        public decimal Iva27 { get; set; }
        public decimal Iva21 { get; set; }
        public decimal Iva105 { get; set; }
        public decimal Iva0 { get; set; }
        public decimal Iva25 { get; set; }
        public decimal Iva5 { get; set; }

        //netos no gravado/exento
        public decimal NoGravado { get; set; } = 0m;
        public decimal Exento { get; set; } = 0m;


        ////otros comprobantes
        //public decimal NetoOtros { get; set; }
        //public decimal IvaOtros { get; set; }




        //totales
        /// <summary>
        /// Neto gravado total 
        /// </summary>
        public decimal NetoGravado => Neto27 + Neto21 + Neto105 + Neto0 + Neto25 + Neto5;

        /// <summary>
        /// iva total
        /// </summary>
        public decimal Iva => Iva27 + Iva21 + Iva105 + Iva0 + Iva25 + Iva5;

        /// <summary>
        /// neto total + iva total
        /// </summary>
        public decimal TotalGeneral => NetoGravado + Iva + NoGravado + Exento;




        // ✅ Sobrecarga del operador -
        public static TotalesIVA operator -(TotalesIVA a, TotalesIVA b)
        {
            return new TotalesIVA
            {
                Neto27 = a.Neto27 - b.Neto27,
                Neto21 = a.Neto21 - b.Neto21,
                Neto105 = a.Neto105 - b.Neto105,
                Neto0 = a.Neto0 - b.Neto0,
                Neto25 = a.Neto25 - b.Neto25,
                Neto5 = a.Neto5 - b.Neto5,

                Iva27 = a.Iva27 - b.Iva27,
                Iva21 = a.Iva21 - b.Iva21,
                Iva105 = a.Iva105 - b.Iva105,
                Iva0 = a.Iva0 - b.Iva0,
                Iva25 = a.Iva25 - b.Iva25,
                Iva5 = a.Iva5 - b.Iva5,

                NoGravado = a.NoGravado - b.NoGravado,
                Exento = a.Exento - b.Exento,

                //NetoOtros = a.NetoOtros - b.NetoOtros,
                //IvaOtros = a.IvaOtros - b.IvaOtros,

            };
        }

        public static TotalesIVA operator +(TotalesIVA a, TotalesIVA b)
        {
            return new TotalesIVA
            {
                Neto27 = a.Neto27 + b.Neto27,
                Neto21 = a.Neto21 + b.Neto21,
                Neto105 = a.Neto105 + b.Neto105,
                Neto0 = a.Neto0 + b.Neto0,
                Neto25 = a.Neto25 + b.Neto25,
                Neto5 = a.Neto5 + b.Neto5,

                Iva27 = a.Iva27 + b.Iva27,
                Iva21 = a.Iva21 + b.Iva21,
                Iva105 = a.Iva105 + b.Iva105,
                Iva0 = a.Iva0 + b.Iva0,
                Iva25 = a.Iva25 + b.Iva25,
                Iva5 = a.Iva5 + b.Iva5,

                NoGravado = a.NoGravado + b.NoGravado,
                Exento = a.Exento + b.Exento,

                //NetoOtros = a.NetoOtros + b.NetoOtros,
                //IvaOtros = a.IvaOtros + b.IvaOtros,

            };
        }

    }
}
