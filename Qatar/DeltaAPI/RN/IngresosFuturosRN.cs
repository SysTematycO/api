using DeltaAPI.ModelsRN;
using DeltaAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using DeltaAPI.RN.Interfaces;
using DeltaAPI.Repositories;
using DeltaAPI.Helpers;

namespace DeltaAPI.RN
{
    public class IngresosFuturosRN: IIngresosFuturosRN
    {
        private readonly QatarContext _qatarContext;
        private readonly IConfiguration _configuracion;

        public IngresosFuturosRN(QatarContext QatarContext, IConfiguration configuration)
        {
            this._qatarContext = QatarContext;
            this._configuracion = configuration;
        }

        public double CalcularIngresosFuturos(IngresosFuturos ingresosFuturos)
        {
            RCapacidadPago rCapacidadPago = new RCapacidadPago(_qatarContext, _configuracion);
            InformacionObjetos informacionObjetos = new InformacionObjetos(_qatarContext, _configuracion);
            IngresoFuturoEstatico ingresoFuturoEstatico = new IngresoFuturoEstatico();
            TipoCarga tipoCarga = new TipoCarga();

            double ingresoBruto = 0;
            double margenInicial = 0;
            double valorTipoCarga = 0;
            double porcentajeCapacidadInicial = 0;
            double porcentajeCapacidadFinal = 0;
            double margen = 0;
            double ingresoFuturo = 0;
            int minimo = 0;
            int maximo = 0;

            if (ingresosFuturos.ActividadCliente.ToLower().Trim().Equals("transportador"))
            {
                ingresoFuturoEstatico = (IngresoFuturoEstatico)informacionObjetos.CargarDatosPropiedades(ingresoFuturoEstatico, 0, "IngresosFuturos");
                tipoCarga = (TipoCarga)informacionObjetos.CargarDatosPropiedades(tipoCarga, 0, "IngresosFuturos");

                if (ingresosFuturos.Modelo < ingresoFuturoEstatico.AnioResolucionTonelada)
                {
                    ingresoFuturoEstatico.ResolucionTonelada = (double)rCapacidadPago.GetValorFijoConcepto(1, "ResolucionTonelada", "IngresosFuturos").Valor;
                }
                if (ingresosFuturos.Modelo > DateTime.Now.Year - ingresoFuturoEstatico.DiferenciaAnioGastoxUso)
                {
                    ingresoFuturoEstatico.GastoxUso = (double)rCapacidadPago.GetValorFijoConcepto(1, "GastoXUso", "IngresosFuturos").Valor;
                }

                string[] numeros = ingresosFuturos.CapacidadCarga.Split('-');
                minimo = int.Parse(numeros[0]);
                maximo = int.Parse(numeros[1]);

                ingresosFuturos.TipoCarroceria = ingresosFuturos.TipoCarroceria.ToLower().Trim();
                ingresosFuturos.TipoCarroceria = ingresosFuturos.TipoCarroceria.Replace(" ", string.Empty);

                if (ingresosFuturos.TipoCarroceria.Equals("grua"))
                {
                    valorTipoCarga = tipoCarga.TipoCargaGrua;
                }
                else if (ingresosFuturos.TipoCarroceria.Equals("thermoking"))
                {
                    valorTipoCarga = tipoCarga.TipoCargaRefrigerada;
                }
                else
                {
                    valorTipoCarga = tipoCarga.TipoCargaSeca;
                }

                if (minimo <= ingresoFuturoEstatico.CapacidadCargaMenor)
                {
                    porcentajeCapacidadInicial = ingresoFuturoEstatico.PorcentajeMargenMenor + ((ingresoFuturoEstatico.MargenMenor - minimo)
                        * (ingresoFuturoEstatico.Factor * ingresoFuturoEstatico.ExponenteFactor));

                    porcentajeCapacidadFinal = ingresoFuturoEstatico.PorcentajeMargenMenor + ((ingresoFuturoEstatico.MargenMenor - maximo)
                        * (ingresoFuturoEstatico.Factor * ingresoFuturoEstatico.ExponenteFactor));

                    margen = (porcentajeCapacidadInicial + porcentajeCapacidadFinal) / 2;
                }
                else if (minimo >= ingresoFuturoEstatico.CapacidadCargaInicial)
                {
                    porcentajeCapacidadInicial = ingresoFuturoEstatico.PorcentajeMargenInicial + ((ingresoFuturoEstatico.MargenInicial - minimo)
                        * ingresoFuturoEstatico.Factor);

                    porcentajeCapacidadFinal = ingresoFuturoEstatico.PorcentajeMargenInicial + ((ingresoFuturoEstatico.MargenInicial - maximo)
                        * ingresoFuturoEstatico.Factor);

                    margen = (porcentajeCapacidadInicial + porcentajeCapacidadFinal) / 2;
                }

                if (margen < ingresoFuturoEstatico.MargenLimite)
                {
                    margen = ingresoFuturoEstatico.MargenLimite;
                }

                if (maximo <= ingresoFuturoEstatico.MargenConductorMinimo)
                {
                    ingresoFuturoEstatico.CostoConductor = (double)rCapacidadPago.GetValorFijoConcepto(0, "CostoConductor", "IngresosFuturos").Valor;
                }
                else if (maximo <= ingresoFuturoEstatico.MargenConductorMaximo)
                {
                    ingresoFuturoEstatico.CostoConductor = (double)rCapacidadPago.GetValorFijoConcepto(1, "CostoConductor", "IngresosFuturos").Valor;

                }
                else if (maximo > ingresoFuturoEstatico.MargenConductorMaximo)
                {
                    ingresoFuturoEstatico.CostoConductor = (double)rCapacidadPago.GetValorFijoConcepto(2, "CostoConductor", "IngresosFuturos").Valor;
                }

                ingresoBruto = (valorTipoCarga * minimo + valorTipoCarga * maximo) / 2;

                ingresoFuturo = ingresoBruto * ingresoFuturoEstatico.IngresosActividad * ingresoFuturoEstatico.InflacionAnual
                    * ingresoFuturoEstatico.ResolucionTonelada * ingresoFuturoEstatico.GastoxUso * margen - ingresoFuturoEstatico.CostoConductor;
            }

            return ingresoFuturo;
        }
    }
}
