using DeltaAPI.ModelsRN;

namespace DeltaAPI.RN.Interfaces
{   /// <summary>
    /// Contrato
    /// </summary>
    public interface IModeloPricingRN
    {
        double CalcularModeloPricing(ModeloPricing modeloPricing);
    }
}
