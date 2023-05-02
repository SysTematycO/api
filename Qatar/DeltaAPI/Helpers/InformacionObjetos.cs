using DeltaAPI.Models;
using DeltaAPI.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using System.Reflection;

namespace DeltaAPI.Helpers
{   /// <summary>
    /// Clase para copiar las  propiedades de una clase a otra e
    /// insertar datos a las propiedades de un objeto
    /// </summary>
    public class InformacionObjetos
    {
        private readonly QatarContext qatarContext;
        private readonly IConfiguration configuracion;

        public InformacionObjetos(QatarContext QatarContext, IConfiguration configuration)
        {
            qatarContext = QatarContext;
            configuracion = configuration;
        }
        public object CargarDatosPropiedades(object objeto, int tipo, string reglaNegocio)
        {
            RCapacidadPago rCapacidadPago = new RCapacidadPago(qatarContext, configuracion);
            var propiedades = objeto.GetType().GetProperties();

            foreach (var prop in propiedades)
            {
                prop.SetValue(objeto, (double)rCapacidadPago.GetValorFijoConcepto(tipo, prop.Name, reglaNegocio).Valor);
            }
            return objeto;
        }

        public void CopiarPropiedades(object origen, object destino)
        {
            Type tipoOrigen = origen.GetType();
            Type tipoDestino = destino.GetType();

            PropertyInfo[] propiedadesOrigen = tipoOrigen.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo propiedadOrigen in propiedadesOrigen)
            {
                PropertyInfo propiedadDestino = tipoDestino.GetProperty(propiedadOrigen.Name, BindingFlags.Public | BindingFlags.Instance);
                if (propiedadDestino != null && propiedadDestino.CanWrite)
                {
                    object valorOrigen = propiedadOrigen.GetValue(origen, null);
                    propiedadDestino.SetValue(destino, valorOrigen, null);
                }
            }
        }
    }
}
