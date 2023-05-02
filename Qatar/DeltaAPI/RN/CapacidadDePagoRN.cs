using DeltaAPI.Helpers;
using DeltaAPI.Models;
using DeltaAPI.ModelsRN;
using DeltaAPI.Repositories;
using DeltaAPI.RN.Interfaces;

namespace DeltaAPI.RN
{
    public class CapacidadDePagoRN : ICapacidadDePagoRN
    {
        private readonly QatarContext _qatarContext;
        private readonly IConfiguration _configuracion;
        private CapacidadPagoCreditoEstatico creditoEstatico;

        public CapacidadDePagoRN(QatarContext qatarContext, IConfiguration configuration)
        {
            this._qatarContext = qatarContext;
            this._configuracion = configuration;
            creditoEstatico = new CapacidadPagoCreditoEstatico();
        }
        
        public double CalcularCapacidadMonto(CapacidadPago capacidadPago)
        {
            RCapacidadPago rCapacidadPago = new RCapacidadPago(_qatarContext, _configuracion);
            IngresosFuturosRN ingresosFuturosRN = new IngresosFuturosRN(_qatarContext, _configuracion);
            InformacionObjetos informacionObjetos = new InformacionObjetos(_qatarContext, _configuracion);
            ModeloPricingRN modeloPricingRN = new ModeloPricingRN(_qatarContext, _configuracion);

            IngresosFuturos ingresosFuturos = new IngresosFuturos();
            informacionObjetos.CopiarPropiedades(capacidadPago, ingresosFuturos);

            ModelsRN.ModeloPricing modeloPricing = new ModelsRN.ModeloPricing();
            informacionObjetos.CopiarPropiedades(capacidadPago, modeloPricing);

          
            double cuotaFutura = 0;
            double seguro = 0;
            double montoPeriodoGracia = 0;
            double cuotaCorriente = 0;
            double capacidadMonto = 0;
            double gastos = 0;
            double ingresos = 0;

            double tasa = 0;
            double ingresosFuturo = 0;
            double porcentajeEnduedamiento = 0;
            double porcentajeSeguro = 0;
            double porcentajeGracia = 0;


            if (capacidadPago.MesesGracia >= 0 && capacidadPago.MesesGracia <= 2)
            {
                ingresosFuturo = ingresosFuturosRN.CalcularIngresosFuturos(ingresosFuturos);
                tasa = modeloPricingRN.CalcularModeloPricing(modeloPricing);

                porcentajeEnduedamiento = (double)rCapacidadPago.GetValorFijoConcepto(0, "PorcentajeEndeudamiento", "CapacidadPago").Valor;
                porcentajeSeguro = (double)rCapacidadPago.GetValorFijoConcepto(capacidadPago.MesesGracia, "PorcentajeSeguro", "CapacidadPago").Valor;
                porcentajeGracia = (double)rCapacidadPago.GetValorFijoConcepto(capacidadPago.MesesGracia, "PorcentajeGracia", "CapacidadPago").Valor;
                gastos = capacidadPago.OtrosGastos + capacidadPago.CuotaCreditosFijos + capacidadPago.RotativosTC;

                ingresos = capacidadPago.IngresosActuales + ingresosFuturo;

                cuotaFutura = ((porcentajeEnduedamiento - (gastos / ingresos)) * ingresos) + capacidadPago.CuotaGps;

                montoPeriodoGracia = cuotaFutura * porcentajeGracia;

                seguro = (cuotaFutura - capacidadPago.CuotaGps - montoPeriodoGracia) - ((cuotaFutura - capacidadPago.CuotaGps - montoPeriodoGracia)
                        / (1 + porcentajeSeguro));

                cuotaCorriente = cuotaFutura - capacidadPago.CuotaGps - seguro - montoPeriodoGracia;

                capacidadMonto = cuotaCorriente * ((Math.Pow(1 + tasa, capacidadPago.Plazo - capacidadPago.MesesGracia) - 1) /
                                ((tasa * Math.Pow(1 + tasa, capacidadPago.Plazo - capacidadPago.MesesGracia)))) - capacidadPago.InstalacionGps;
            }
            return capacidadMonto;
        }

        public double CapacidadPagoCredito(CapacidadPagoCredito capacidadPagoCredito)
        {
            RCapacidadPago rCapacidadPago = new RCapacidadPago(_qatarContext, _configuracion);
            IngresosFuturosRN ingresosFuturosRN = new IngresosFuturosRN(_qatarContext, _configuracion);
            InformacionObjetos informacionObjetos = new InformacionObjetos(_qatarContext, _configuracion);
            ModeloPricingRN modeloPricingRN = new ModeloPricingRN(_qatarContext, _configuracion);
            
            IngresosFuturos ingresosFuturos = new IngresosFuturos();
            informacionObjetos.CopiarPropiedades(capacidadPagoCredito, ingresosFuturos);

            ModelsRN.ModeloPricing modeloPricing = new ModelsRN.ModeloPricing();
            informacionObjetos.CopiarPropiedades(capacidadPagoCredito, modeloPricing);

            double tasa = 0;
            double ingresosFuturo = 0;
            double capacidadPagoTotal = 0;

            ingresosFuturo = ingresosFuturosRN.CalcularIngresosFuturos(ingresosFuturos);
            tasa = modeloPricingRN.CalcularModeloPricing(modeloPricing);

            creditoEstatico.PorcentajeArriendos = (double)rCapacidadPago.GetValorFijoConcepto(0, "PorcentajeArriendos", "CapacidadPagoCredito").Valor;

            creditoEstatico.IngresosActuales = (capacidadPagoCredito.IngresosActividad * capacidadPagoCredito.PorcentajeActividad)
                + capacidadPagoCredito.OtroIngresos + (capacidadPagoCredito.Arriendos * creditoEstatico.PorcentajeArriendos);

            creditoEstatico.IngresosTotales = creditoEstatico.IngresosActuales + ingresosFuturo;

            creditoEstatico.InteresTarjetaCredito = (double)rCapacidadPago.GetValorFijoConcepto(0, "InteresTarjetaCredito", "CapacidadPagoCredito").Valor;
            creditoEstatico.PlazoTarjetaCredito = (double)rCapacidadPago.GetValorFijoConcepto(0, "PlazoTarjetaCredito", "CapacidadPagoCredito").Valor;

            creditoEstatico.EgresosTotales = -((capacidadPagoCredito.RotativosTC * creditoEstatico.InteresTarjetaCredito) /
                ((Math.Pow(1 + creditoEstatico.InteresTarjetaCredito, -creditoEstatico.PlazoTarjetaCredito)) - 1));

            creditoEstatico.EgresosTotales = creditoEstatico.EgresosTotales + capacidadPagoCredito.CuotaCreditosFijos + capacidadPagoCredito.OtrosGastos;

            creditoEstatico.PorcentajeGracia = (double)rCapacidadPago.GetValorFijoConcepto(capacidadPagoCredito.MesesGracia, "PorcentajeGracia", "CapacidadPago").Valor;

            if (capacidadPagoCredito.LineaCredito.ToLower().Trim().Equals("vps") == false)
            {
                creditoEstatico.PorcentajeSeguro = (double)rCapacidadPago.GetValorFijoConcepto(0, "PorcentajeSeguroTaxi", "CapacidadPagoCredito").Valor;
            }
            else
            {
                creditoEstatico.PorcentajeSeguro = (double)rCapacidadPago.GetValorFijoConcepto(capacidadPagoCredito.MesesGracia, "PorcentajeSeguro", "CapacidadPago").Valor;
            }

            if (capacidadPagoCredito.TipoD == true)
            {
                creditoEstatico.PorcentajeEndeudamiento = (double)rCapacidadPago.GetValorFijoConcepto(0, "PorcentajeEndeudamientoTipoD", "CapacidadPagoCredito").Valor;
            }
            else
            {
                creditoEstatico.PorcentajeEndeudamiento = (double)rCapacidadPago.GetValorFijoConcepto(0, "PorcentajeEndeudamiento", "CapacidadPago").Valor;
            }

            if (capacidadPagoCredito.Gps == true && capacidadPagoCredito.InstalacionGps == true)
            {
                creditoEstatico.InstalacionGps = (double)rCapacidadPago.GetValorFijoConcepto(0, "InstalacionGps", "CapacidadPagoCredito").Valor;
            }

            if (capacidadPagoCredito.Gps == true) {
                creditoEstatico.CanonGps = (double)rCapacidadPago.GetValorFijoConcepto(0, "CanonGps", "CapacidadPagoCredito").Valor;
                creditoEstatico.CanonGps = capacidadPagoCredito.FactorGps * creditoEstatico.CanonGps;
            }

            creditoEstatico.CapacidadMonto = ((creditoEstatico.PorcentajeEndeudamiento - (creditoEstatico.EgresosTotales / creditoEstatico.IngresosTotales)) * creditoEstatico.IngresosTotales) + creditoEstatico.CanonGps;

            creditoEstatico.MontoPeriodoGracia = creditoEstatico.CapacidadMonto * creditoEstatico.PorcentajeGracia;

            creditoEstatico.MontoSeguro = ((creditoEstatico.CapacidadMonto - creditoEstatico.CanonGps - creditoEstatico.MontoPeriodoGracia) -
                ((creditoEstatico.CapacidadMonto - creditoEstatico.CanonGps - creditoEstatico.MontoPeriodoGracia) / (1 + creditoEstatico.PorcentajeSeguro))) * capacidadPagoCredito.NumeroDeudores;

            creditoEstatico.CuotaCorriente = creditoEstatico.CapacidadMonto - creditoEstatico.CanonGps - creditoEstatico.MontoPeriodoGracia - creditoEstatico.MontoSeguro;

            capacidadPagoTotal = creditoEstatico.CuotaCorriente * ((1 - Math.Pow((1 + tasa), -capacidadPagoCredito.PlazoSolicitado)) / tasa) - creditoEstatico.InstalacionGps;

            return capacidadPagoTotal;
        }

        public object CuotaAprobada(CuotaAprobada cuotaAprobada)
        {
            RCapacidadPago rCapacidadPago = new RCapacidadPago(_qatarContext, _configuracion);
            IngresosFuturosRN ingresosFuturosRN = new IngresosFuturosRN(_qatarContext, _configuracion);
            InformacionObjetos informacionObjetos = new InformacionObjetos(_qatarContext, _configuracion);
            ModeloPricingRN modeloPricingRN = new ModeloPricingRN(_qatarContext, _configuracion);

            IngresosFuturos ingresosFuturos = new IngresosFuturos();
            informacionObjetos.CopiarPropiedades(cuotaAprobada, ingresosFuturos);

            ModelsRN.ModeloPricing modeloPricing = new ModelsRN.ModeloPricing();
            informacionObjetos.CopiarPropiedades(cuotaAprobada, modeloPricing);

            CapacidadPagoCredito capacidadPagoCredito = new CapacidadPagoCredito();
            informacionObjetos.CopiarPropiedades(cuotaAprobada, capacidadPagoCredito);

            double tasa = 0;
            double ingresosFuturo = 0;
            double capacidadPagoTotal = 0;
            double cuotaCorriente = 0;
            double ltvAprobado = 0;
            double seguro = 0;
            double factorSeguro = 0;
            double porcentajeEndeudamiento = 0;
            double cuotaMensualTotal = 0;
            double capitalGraciaUno = 0;
            double capitalGraciaDos = 0;
            double capitalGracia = 0;
            double valorPgracia = 0;

            ingresosFuturo = ingresosFuturosRN.CalcularIngresosFuturos(ingresosFuturos);
            tasa = modeloPricingRN.CalcularModeloPricing(modeloPricing);
            capacidadPagoTotal = CapacidadPagoCredito(capacidadPagoCredito);

            cuotaCorriente = -((cuotaAprobada.MontoAprobado + creditoEstatico.InstalacionGps) * cuotaAprobada.TasaAprobada) /
                ((Math.Pow(1 + cuotaAprobada.TasaAprobada, -(cuotaAprobada.PlazoAprobado - cuotaAprobada.MesesGracia))) - 1);

            ltvAprobado = cuotaAprobada.MontoAprobado / cuotaAprobada.ValorGarantia;

            factorSeguro = (double) rCapacidadPago.GetValorFijoConcepto(0, "FactorSeguro", "CuotaAprobada").Valor;

            seguro = ((cuotaAprobada.MontoAprobado + creditoEstatico.InstalacionGps) / 1000000) * factorSeguro * cuotaAprobada.NumeroDeudores;

            capitalGraciaUno = (creditoEstatico.InstalacionGps + cuotaAprobada.MontoAprobado) * cuotaAprobada.TasaAprobada + seguro + creditoEstatico.CanonGps;

            capitalGraciaDos = (creditoEstatico.InstalacionGps + cuotaAprobada.MontoAprobado + capitalGraciaUno) * cuotaAprobada.TasaAprobada + seguro + creditoEstatico.CanonGps;

            if (cuotaAprobada.MesesGracia == 2)
            {
                capitalGracia = capitalGraciaUno + capitalGraciaDos;
            }
            else if (cuotaAprobada.MesesGracia == 1)
            {
                capitalGracia = capitalGraciaUno;
            }
            else
            {
                capitalGracia = 0;
            }

            valorPgracia = -((capitalGracia) * cuotaAprobada.TasaAprobada) /
                ((Math.Pow(1 + cuotaAprobada.TasaAprobada, -(cuotaAprobada.PlazoAprobado - cuotaAprobada.MesesGracia))) - 1);

            cuotaMensualTotal = cuotaCorriente + seguro + creditoEstatico.CanonGps + valorPgracia;

            porcentajeEndeudamiento = (cuotaMensualTotal + creditoEstatico.EgresosTotales - creditoEstatico.CanonGps) / creditoEstatico.IngresosTotales;

            return new
            {
                PorcentajeEndeudamiento = porcentajeEndeudamiento,
                CuotaMensualTotal = cuotaMensualTotal,
                LTVAprobado = ltvAprobado
            };
        }
    }
}
