namespace DeltaAPI.ModelsRN
{   /// <summary>
    /// DTO 
    /// </summary>
    public class CapacidadPago
    {
        public int MesesGracia { get; set; }
        public int Plazo { get; set; }
        public double InstalacionGps { get; set; }
        public double CuotaGps { get; set; }
        public double IngresosActuales { get; set; }
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
