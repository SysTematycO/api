namespace DeltaAPI.ModelsRN
{
    /// <summary>
    /// DTO
    /// </summary>
    public class CapacidadPagoCredito
    {
        public bool Gps { get; set; }
        public bool InstalacionGps { get; set; }
        public int FactorGps { get; set; }
        public string LineaCredito { get; set; }
        public bool TipoD { get; set; }
        public int NumeroDeudores { get; set; }
        public int MesesGracia { get; set; }
        public int PlazoSolicitado { get; set; }
        public double IngresosActividad { get; set; }
        public double PorcentajeActividad { get; set; }
        public double OtroIngresos { get; set; }
        public double Arriendos { get; set; }
        public double OtrosGastos { get; set; }
        public double CuotaCreditosFijos { get; set; }
        public double RotativosTC { get; set; }
        public string CapacidadCarga { get; set; }
        public string TipoCarroceria { get; set; }
        public string ActividadCliente { get; set; }
        public bool Conductor { get; set; }
        public int Modelo { get; set; }
        public int Score { get; set; }
    }
}
