using DeltaAPI.Models;
using DeltaAPI.ModelsRN;
using DeltaAPI.Repositories;
using DeltaAPI.RN;
using DeltaAPI.RN.Interfaces;
using Microsoft.AspNetCore.Mvc;

using ModeloPricing = DeltaAPI.ModelsRN.ModeloPricing;

namespace DeltaAPI.Controllers
{
    /// <summary>
    /// Calcula la capacidad de pago de acuerdo a ingresos, tipo de vehiculo y score del cliente
    /// </summary>
    [ApiController]
    [Route("AreaCredito")]
    public class AreaCreditoController : ControllerBase
    {
        private readonly ICapacidadDePagoRN _capacidadDePagoRN;
        private readonly IIngresosFuturosRN _ingresosFuturoRN;
        private readonly IModeloPricingRN _modeloPricingRN;

        /// <summary>
        /// Inyeccion de dependencias
        /// </summary>
        /// <param name="capacidadDePagoRN">Interfaz capacidadDePagoRN</param>
        /// <param name="ingresosFuturosRN">Interfaz ingresosFuturosRN</param>
        /// <param name="modeloPricingRN">Interfaz modeloPricingRN</param>
        public AreaCreditoController(
            ICapacidadDePagoRN capacidadDePagoRN, IIngresosFuturosRN ingresosFuturosRN, IModeloPricingRN modeloPricingRN)
        {
            _capacidadDePagoRN = capacidadDePagoRN;
            _ingresosFuturoRN = ingresosFuturosRN;
            _modeloPricingRN = modeloPricingRN;
        }

        /// <summary>
        /// Summary: Calculo de la capacidad de pago del cliente
        /// </summary>
        /// <param name="capacidadPago">Se envia un objeto tipo JSON con datos obtenidos al radicar una solicitud</param>
        /// <returns>Retorna la capacidad de pago del cliente</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "mesesGracia" : 2,
        ///        "plazo": 60,
        ///        "instalacionGps":410432,
        ///        "cuotaGps": 60000,
        ///        "ingresosActuales": 2500000,
        ///        "otrosGastos":100000,
        ///        "cuotaCreditosFijos":0,
        ///        "rotativosTC":0,
        ///        "capacidadCarga": 3081,
        ///        "tipoCarroceria":"grua",
        ///        "actividadCliente":"transportador",
        ///        "conductor": true,
        ///        "modelo":2022,
        ///        "score":400
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Returns 200 ...</response>
        /// <response code="400">Returns 400 if the service has problems</response>
        [HttpPost]
        [Route("CapacidadPago")]
        public ActionResult<Respuesta> CapacidadMonto([FromBody] CapacidadPago capacidadPago)
        {
            double capacidadMonto = 0;
            Respuesta respuesta = new Respuesta();

            try
            {
                capacidadMonto = _capacidadDePagoRN.CalcularCapacidadMonto(capacidadPago);
                object objeto = new { CapacidadPago = capacidadMonto };
                respuesta = ConstruirRespuesta.CrearRespuesta(0, "Correcto", objeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(respuesta);
        }

        /// <summary>
        /// Calculo de la capacidad de pago en estudio de credito
        /// </summary>
        /// <param name="capacidadPagoCredito">Se envia un objeto tipo JSON con datos obtenidos en el estudio credito</param>
        /// <returns>Retorna capacidad de pago total en linea de credito</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "gps": true,
        ///        "instalacionGps": true,
        ///        "factorGps": 1,
        ///        "lineaCredito": "vps",
        ///        "tipoD": false,
        ///        "numeroDeudores": 1,
        ///        "mesesGracia": 0,
        ///        "plazoSolicitado": 60,
        ///        "ingresosActividad": 2000000,
        ///        "porcentajeActividad": 0.35,
        ///        "otroIngresos": 1000000,
        ///        "arriendos": 700000,
        ///        "otrosGastos":  150000,
        ///        "cuotaCreditosFijos": 350000,
        ///        "rotativosTC": 3000000,
        ///        "capacidadCarga": "4001-4200",
        ///        "tipoCarroceria": "furgon",
        ///        "actividadCliente": "transportador",
        ///        "conductor": true,
        ///        "modelo": 2023,
        ///        "score": 800
        ///     }
        /// </remarks>
        /// <response code="200">Returns 200 ...</response>
        /// <response code="400">Returns 400 if the service has problems</response>
        [HttpPost]
        [Route("CapacidadPagoCredito")]
        public ActionResult<Respuesta> CapacidadPagoCredito([FromBody] CapacidadPagoCredito capacidadPagoCredito)
        {
            double capacidadPago = 0;
            Respuesta respuesta = new Respuesta();

            try
            {
                capacidadPago = _capacidadDePagoRN.CapacidadPagoCredito(capacidadPagoCredito);
                object objeto = new { CapacidadPagoCredito = capacidadPago };
                respuesta = ConstruirRespuesta.CrearRespuesta(0, "Correcto", objeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(respuesta);
        }

        /// <summary>
        /// Calcula cuota final despues de condiciones aprobadas
        /// </summary>
        /// <param name="cuotaAprobada"> Se envia un objeto tipo JSON con datos obtenidos</param>
        /// <returns>Retorna la cuota aprobada final</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///          "gps": true,
        ///          "instalacionGps": true,
        ///          "factorGps": 1,
        ///          "lineaCredito": "vps",
        ///          "tipoD": false,
        ///          "numeroDeudores": 1,
        ///          "mesesGracia": 0,
        ///          "plazoSolicitado": 60,
        ///          "ingresosActividad": 2000000,
        ///          "porcentajeActividad": 0.35,
        ///          "otroIngresos": 1000000,
        ///          "arriendos": 700000,
        ///          "otrosGastos":  150000,
        ///          "cuotaCreditosFijos": 350000,
        ///          "rotativosTC": 3000000,
        ///          "capacidadCarga": "4001-4200",
        ///          "tipoCarroceria": "furgon",
        ///          "actividadCliente": "transportador",
        ///          "conductor": true,
        ///          "modelo": 2023,
        ///          "score": 800,   
        ///          "MontoAprobado": 84195312,
        ///          "PlazoAprobado": 60,
        ///          "TasaAprobada": 0.0259,
        ///          "ValorGarantia": 135000000
        ///     }
        /// </remarks>
        /// <response code="200">Returns 200 ...</response>
        /// <response code="400">Returns 400 if the service has problems</response>
        [HttpPost]
        [Route("CuotaAprobada")]
        public ActionResult<Respuesta> CuotaAprobada([FromBody] CuotaAprobada cuotaAprobada)
        {
            double cuota = 0;
            Respuesta respuesta = new Respuesta();

            try
            {
                object objeto = _capacidadDePagoRN.CuotaAprobada(cuotaAprobada);
                respuesta = ConstruirRespuesta.CrearRespuesta(0, "Correcto", objeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return Ok(respuesta);
        }

        /// <summary>
        /// Summary: Calculo de los ingresos futuros del cliente
        /// </summary>
        /// <param name="ingresosFuturos">Se envia un objeto tipo JSON con datos del vehiculo y del cliente</param>
        /// <returns>Retorna los ingresos futuros del cliente</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "capacidadCarga": 3081,
        ///        "tipoCarroceria":"grua",
        ///        "actividadCliente":"transportador",
        ///        "conductor": true,
        ///        "modelo":2022
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Returns 200 ...</response>
        /// <response code="400">Returns 400 if the service has problems</response>
        [HttpPost]
        [Route("IngresoFuturo")]
        public ActionResult<Respuesta> IngresoFuturo([FromBody] IngresosFuturos ingresosFuturos)
        {
            double ingresos = 0;
            Respuesta respuesta = new Respuesta();
            try
            {
                ingresos = _ingresosFuturoRN.CalcularIngresosFuturos(ingresosFuturos);
                object objeto = new { IngresoFuturo = ingresos };
                respuesta = ConstruirRespuesta.CrearRespuesta(0, "Correcto", objeto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
            return Ok(respuesta);
        }


        /// <summary>
        /// Summary: Calculo de la tasa con respecto al score del cliente
        /// </summary>
        /// <param name="modeloPricing">Se envia un objeto tipo JSON con el puntaje del cliente</param>
        /// <returns>Retorna la tasa en </returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
        ///     {
        ///        "score":400
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">Returns 200 ...</response>
        /// <response code="400">Returns 400 if the service has problems</response>
        [HttpPost]
        [Route("ModeloPricing")]
        public ActionResult<Respuesta> ModeloPricing([FromBody] ModeloPricing modeloPricing)
        {
            double tasa = 0;
            Respuesta respuesta = new Respuesta();
            try
            {
                tasa = _modeloPricingRN.CalcularModeloPricing(modeloPricing);
                object objeto = new { Tasa = tasa };
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

