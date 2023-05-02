using DeltaAPI.ModelsRN;
using DeltaAPI.RN.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeltaAPI.Controllers
{
    /// <summary>
    /// Se asigna el fondeador de acuerdo a politicas establecidas
    /// </summary>
    [ApiController]
    [Route("Fondeadores")]
    public class FondeadorController : ControllerBase
    {
        /// <summary>
        /// VacioTemporalmente
        /// </summary>
        public FondeadorController()
        {
        }

        /// <summary>
        /// Summary: Se devuelve fondeador segun parametros por area financiera
        /// </summary>
        /// <returns>Retorna el NIT del fondeador</returns>
        /// <response code="200">Returns 200 ...</response>
        /// <response code="400">Returns 400 if the service has problems</response>
        [HttpPost]
        [Route("ObtenerFondeador")]
        public ActionResult<Respuesta> ObtenerFondeador()
        {
            Respuesta respuesta = new Respuesta();
            try
            {
                dynamic objeto = new { NitFondeador = 901335972 };
                respuesta = ConstruirRespuesta.CrearRespuesta(0, "Correcto", objeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(respuesta);

        }

    }
}