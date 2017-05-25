using ServiceStack.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessRest.Entities;
using ServiceStack;

namespace ServicioVivanto
{
    public class Procesamiento
    {
        IConexionVivanto vivanto;
        IConexionIRDCOL ird;
        ParametrosProcesamiento parProcesamiento;
		List<RuvConsultaNoValorados> noprocesados = new List<RuvConsultaNoValorados> ();

		private readonly string logNoprocesado = "NoProcesados.json";

        public bool IgnorarExcepciones { get; set; }

        public Procesamiento (IConexionVivanto vivanto, IConexionIRDCOL ird, ParametrosProcesamiento parProcesamiento)
        {
            this.vivanto = vivanto;
            this.ird = ird;
            this.parProcesamiento = parProcesamiento;
            IgnorarExcepciones = true;
        }

		public string Iniciar(string archivoPorProcesar= null) {

			var ret = string.Empty;
            //using (vivanto)
            //{
                //vivanto.IniciarSesion();
				ProcesarRegistros(archivoPorProcesar);
				//vivanto.CerrarSession ();
				return  FnVal.NombreArhivo (vivanto.DirInfoLog, logNoprocesado);
			//}
			//return ret;
        }


		void ProcesarRegistros(string archivoNoprocesados=null)
        {
			var lnv = ConsultarNoValoradosRuv( archivoNoprocesados);

            var items = 0;
            foreach (var nv in lnv)
            {
                //if (nv.Identificacion != "25713773") continue; //solo una prueba puntual

                items++;
                //vivanto.IniciarSesion();
                Console.WriteLine("{0} {1} {2}", nv.Identificacion, nv.Numero_Declaracion, nv.Fecha_Declaracion);
                List<DatosBasicos> datosbasicos = ConsultarEnVivanto(nv);
                ProcesarDatosBasicos(nv, datosbasicos);
                //vivanto.CerrarSession();
                if (items == 4)
                {
                    Console.WriteLine("esperando 5 segundos para el siguiente lote");
                    System.Threading.Thread.Sleep(5 * 1000);
                    Console.WriteLine("***************************  continuamos ************************");
                    items = 0;
                }
                System.Threading.Thread.Sleep(500);
            }

			GuardarNoProcesado ();
        }

        private List<DatosBasicos> ConsultarEnVivanto(RuvConsultaNoValorados nv)
        {
            List<DatosBasicos> db = new List<DatosBasicos>();
            try {
                db = vivanto.ConsultarDatosBasicos(nv.Identificacion);
            }
            catch(Exception ex)
            {
				AgregarNoProcesados (nv);
                IgnorarOLanzarExcepcion(ex);
            }

            return db;
        }

		List<RuvConsultaNoValorados> ConsultarNoValoradosRuv(string archivoPorProcesar=null)
        {
            List<RuvConsultaNoValorados> lnv = new List<RuvConsultaNoValorados>();
            try
            {
				lnv =  string.IsNullOrEmpty(archivoPorProcesar)?
					ird.ConsultarNoActualizadosVUR(parProcesamiento.FechaRadicacionInicial, parProcesamiento.FechaRadicacionFinal):
					CargarDeArchivo(archivoPorProcesar);
            }
            catch(Exception ex)
            {
                IgnorarOLanzarExcepcion(ex);
            }
            
            return lnv;
        }

        private void ProcesarDatosBasicos(RuvConsultaNoValorados nv, List<DatosBasicos> datosbasicos)
        {
			var hl = new List<DatosDetallados> ();

            DatosDetallados hecho = null;
            foreach (var dato in datosbasicos)
            {
                if (!(dato.FUENTE == "RUV" || dato.FUENTE == "SIPOD")) continue;
				List<DatosDetallados> hechos;
				if (!ConsultarHechosEnVivanto (dato, out hechos)) {
					AgregarNoProcesados (nv);
					continue;
				}
				hl.AddRange (hechos);
                if (ProcesarHechos(nv, hechos, out hecho))
                {					
                    // uno de los hechos cumple los requisitos 
                    // no es necesario revisar mas los datos basicos de la declaracion
                    break;
                }
            }
            GuardarSiSeEncontroHecho(nv, datosbasicos, hl, hecho);
			ConfirmarComoNoProcesado (nv, hecho);
        }

		private bool ConsultarHechosEnVivanto(DatosBasicos dato, out List<DatosDetallados> hechos)
        {
			hechos = null;
            try {
                hechos = vivanto.ConsultarHechos(dato);
            }
            catch(Exception ex)
            {
                IgnorarOLanzarExcepcion(ex);
            }
            return hechos!=null;
        }

        private bool ProcesarHechos(RuvConsultaNoValorados nv, List<DatosDetallados> hechos, out DatosDetallados hecho)
        {
            hecho = null;
            foreach( var h in hechos)
            {                
                if (ValidarHechoCompleto(nv, h))
                {
                    hecho = h;
                    break; // hecho cumple requisitos no se revisan los otros
                }
                
            }
            return hecho!=null;
        }

        private bool ValidarHechoCompleto(RuvConsultaNoValorados nv, DatosDetallados hecho)
        {
            // se deben cumplir los dos            
            if (!ValidarHechoDesplazamentForzado(hecho) || !ValidarEstadoIncluido(hecho)) return false;

            if (hecho.FUENTE == "RUV")
            {
                if (!ValidarHechoPorNumeroDeclaracion(hecho, nv) || !ValidarHechoPorFechaValoracion(hecho, nv)) return false;
            }
            else if (hecho.FUENTE == "SIPOD")
            {
                if (!ValidarHechoPorNumeroDeclaracion(hecho, nv) || !ValidarHechoPorFechaDeclaracion(hecho, nv)) return false;
            }
            else return false;

            return true;
        }


		private void GuardarSiSeEncontroHecho(RuvConsultaNoValorados nv, List<DatosBasicos> basicos,
			List<DatosDetallados> hechos,
			DatosDetallados hecho)
        {
            bool insertado = false;
            try
            {
                if (hecho != null)
                {
                    Declaracion_UnidadesInsertar registro = new Declaracion_UnidadesInsertar()
                    {
                        Fuente = "WS Vivanto {0}".Fmt(hecho.FUENTE),
                        Id_EstadoUnidad = parProcesamiento.Obtener_Id_EstadoUnidad(hecho.ESTADO),
                        Id_Unidad = parProcesamiento.Id_Unidad,
						Fecha_Inclusion = hecho.F_VALORACION.Value,
                        Id_Declaracion = nv.Id_Declaracion,
                    };
                    ird.Declaracion_UnidadesInsertar(registro);
                    insertado = true;
                }
            }
            catch (Exception ex)
            {
				AgregarNoProcesados (nv);
                IgnorarOLanzarExcepcion(ex);
            }
            finally
            {
				Log.RegistrarProcesado(vivanto.DirInfoLog, nv, basicos, hechos, hecho, insertado, parProcesamiento);
            }
        }
                        

        private bool ValidarHechoPorFechaDeclaracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
        {
			return FnVal.ValidarHechoPorFechaDeclaracion (hecho, nv);
        }

        private bool ValidarHechoPorFechaValoracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
        {
			return FnVal.ValidarHechoPorFechaValoracion (hecho, nv);
        }

        private bool ValidarHechoPorNumeroDeclaracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
        {
            return FnVal.NumeroDeclaracion(nv, hecho);
        }

        private bool ValidarHechoDesplazamentForzado(DatosDetallados hecho)
        {
			return FnVal.ValidarHechoDesplazamentForzado (hecho, parProcesamiento);
        }

        private bool ValidarEstadoIncluido(DatosDetallados hecho)
        {
            return parProcesamiento.Obtener_Id_EstadoUnidad(hecho.ESTADO)!=0;
        }

        private void IgnorarOLanzarExcepcion(Exception ex)
        {
			Log.Sesion(vivanto.DirInfoLog, ex.Message);
			if (!IgnorarExcepciones)
				throw new ExcepcionProcesamiento (vivanto.DirInfoLog, ex.Message);
        }

		private void ConfirmarComoNoProcesado (RuvConsultaNoValorados nv, DatosDetallados hecho){
			if (hecho != null) {
				noprocesados.RemoveAll (q => q.Id_Persona == nv.Id_Persona);
			}
		}

		private void AgregarNoProcesados(RuvConsultaNoValorados nv){
			noprocesados.AddIfNotExists (nv);
		}

		private void GuardarNoProcesado (){
			Log.NoProcesados (vivanto.DirInfoLog, logNoprocesado, noprocesados);
		}

		private List<RuvConsultaNoValorados>  CargarDeArchivo(string archivoPorProcesar){
			return Log.CargarRuvConsultaNoValoradosDeArchivo (archivoPorProcesar);
		}

    }
}
