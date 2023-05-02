using DeltaAPI.Repositories;
using DeltaAPI.RN.Interfaces;
using DeltaAPI.Models;
using DeltaAPI.ModelsRN;

namespace DeltaAPI.RN
{
    public class ModeloPricingRN: IModeloPricingRN
    {
        private readonly QatarContext _qatarContext;
        private readonly IConfiguration _configuracion;

        public ModeloPricingRN(QatarContext qatarContext, IConfiguration configuration)
        {
            this._qatarContext = qatarContext;
            this._configuracion = configuration;
        }
        public double CalcularModeloPricing(ModelsRN.ModeloPricing modeloPricing)
        {
            double tasaMensual = 0;

            RModeloPricing rModeloPricing = new RModeloPricing(_qatarContext, _configuracion);
            Models.ModeloPricing modelo = new Models.ModeloPricing();

            modelo = rModeloPricing.GetTasaEfectivaAnualSinExepcion(modeloPricing.Score);

            tasaMensual = Math.Pow(1.0 + (double)modelo.TasaEase, 1.0 / 12.0) - 1.0;

            return tasaMensual;
        }
    }
}