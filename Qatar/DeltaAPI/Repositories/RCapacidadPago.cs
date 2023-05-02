using DeltaAPI.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;


namespace DeltaAPI.Repositories
{   /// <summary>
    /// Persistencia, consulta de valores numericos fijos en BD para consumir en CapacidadDePagoRN e IngresosFuturosRN
    /// </summary>
    public class RCapacidadPago
    {
        private readonly QatarContext _qatarContext;
        private readonly IConfiguration configuracion;

        public RCapacidadPago(QatarContext qatarContext, IConfiguration configuration)
        {
            _qatarContext = qatarContext;
            configuracion = configuration;

        }

        public ValorXpolitica GetValorFijoConcepto(int tipo, string nombreConcepto, string reglaNegocio)
        {

            decimal valor = 0;

            ValorXpolitica valorXpolitica = new ValorXpolitica();

            valorXpolitica = _qatarContext.ValorXpoliticas.Where(x => x.Tipo == tipo && x.Nombre == nombreConcepto && x.ReglaNegocio == reglaNegocio).ToArray().First();

            return valorXpolitica;
        }
    }
}
