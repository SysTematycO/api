namespace DeltaAPI.ModelsRN
{   /// <summary>
    /// DTO 
    /// </summary>
    public class IngresoFuturoEstatico
    {
        public double IngresosActividad { get; set; }
        public double InflacionAnual { get; set; }
        public double ResolucionTonelada { get; set; }
        public double CapacidadCargaInicial { get; set; }
        public double CapacidadCargaMenor { get; set; }
        public double MargenInicial { get; set; }
        public double MargenMenor { get; set; }
        public double Factor { get; set; }
        public double ExponenteFactor { get; set; }
        public double PorcentajeMargenInicial { get; set; }
        public double PorcentajeMargenMenor { get; set; }
        public double MargenLimite { get; set; }
        public double GastoxUso { get; set; }
        public double LongitudMaxima { get; set; }
        public double LongitudBloque { get; set; }
        public double AnioResolucionTonelada { get; set; }
        public double DiferenciaAnioGastoxUso { get; set; }
        public double CostoConductor { get; set; }
        public double MargenConductorMinimo { get; set; }
        public double MargenConductorMaximo { get; set; }
    }
}
