using DeltaAPI.ModelsRN;

namespace DeltaAPI.RN.Interfaces
{   /// <summary>
    /// Contrato
    /// </summary>
    public interface ICapacidadDePagoRN
    {
        double CalcularCapacidadMonto(CapacidadPago capacidadPago);
        double CapacidadPagoCredito(CapacidadPagoCredito capacidadPagoCredito);
        object CuotaAprobada(CuotaAprobada cuotaAprobada);
    }
}