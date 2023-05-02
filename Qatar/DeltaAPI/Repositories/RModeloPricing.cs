using DeltaAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DeltaAPI.Repositories
{   /// <summary>
    /// Persistencia, consulta de valores numericos fijos en BD para consumir en ModeloPrincingRN
    /// </summary>
    public class RModeloPricing
    {
        private readonly QatarContext _qatarContext;
        private readonly IConfiguration configuracion;

        public RModeloPricing(QatarContext qatarContext, IConfiguration configuration)
        {
            _qatarContext = qatarContext;
            configuracion = configuration;
        }

        public ModeloPricing GetTasaEfectivaAnualSinExepcion(int score)
        {
            ModeloPricing tablaPricing = new ModeloPricing();  
            tablaPricing = _qatarContext.ModeloPricings.Where(x => x.Score <= score).ToArray().Last();

            return tablaPricing;
        }
    }
}
