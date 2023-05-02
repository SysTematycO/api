using Newtonsoft.Json;

namespace DeltaAPI.ModelsRN
{   /// <summary>
    /// Clase para construir las respuestas de las del controller
    /// </summary>
    public abstract class ConstruirRespuesta  
    {

        public static Respuesta CrearRespuesta(int codigo, string estado, dynamic json)
        {
            Respuesta respuesta = new Respuesta();
            respuesta.Codigo = codigo;
            respuesta.Estado = estado;
            respuesta.Json = json;
            return respuesta;
        }
    }
}
