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
        private string horasesion;

        public bool IgnorarExcepciones { get; set; }

        public Procesamiento (IConexionVivanto vivanto, IConexionIRDCOL ird, ParametrosProcesamiento parProcesamiento)
        {
            this.vivanto = vivanto;
            this.ird = ird;
            this.parProcesamiento = parProcesamiento;
            IgnorarExcepciones = true;
            

        }

        public void Iniciar() {

            using (vivanto)
            {
                //vivanto.IniciarSesion();
                ProcesarRegistros();   
            }

        }


        void ProcesarRegistros()
        {
            var lnv = ConsultarNoValoradosRuv();

            var items = 0;
            foreach (var nv in lnv)
            {
                //if (nv.Identificacion != "1059908291") continue; solo una prueba puntual

                items++;
                vivanto.IniciarSesion();
                Console.WriteLine("{0} {1} {2}", nv.Identificacion, nv.Numero_Declaracion, nv.Fecha_Declaracion);
                List<DatosBasicos> datosbasicos = ConsultarEnVivanto(nv);
                ProcesarDatosBasicos(nv, datosbasicos);
                vivanto.CerrarSession();
                if (items == 4)
                {
                    Console.WriteLine("esperando 5 segundos para el siguiente lote");
                    System.Threading.Thread.Sleep(5 * 1000);
                    Console.WriteLine("***************************  continuamos ************************");
                    items = 0;
                }
                System.Threading.Thread.Sleep(500);
            }
        }

        private List<DatosBasicos> ConsultarEnVivanto(RuvConsultaNoValorados nv)
        {
            List<DatosBasicos> db = new List<DatosBasicos>();
            try {
                db = vivanto.ConsultarDatosBasicos(nv.Identificacion);
            }
            catch(Exception ex)
            {
                IgnorarOLanzarExcepcion(ex);
            }

            return db;
        }

        List<RuvConsultaNoValorados> ConsultarNoValoradosRuv()
        {
            List<RuvConsultaNoValorados> lnv = new List<RuvConsultaNoValorados>();
            try
            {
                lnv = ird.ConsultarNoActualizadosVUR(parProcesamiento.FechaRadicacionInicial, parProcesamiento.FechaRadicacionFinal);
            }
            catch(Exception ex)
            {
                IgnorarOLanzarExcepcion(ex);
            }
            
            return lnv;
        }

        private void ProcesarDatosBasicos(RuvConsultaNoValorados nv, List<DatosBasicos> datosbasicos)
        {
            
            DatosDetallados hecho = null;
            foreach (var dato in datosbasicos)
            {
                if (!(dato.FUENTE == "RUV" || dato.FUENTE == "SIPOD")) continue;
                List<DatosDetallados> hechos = ConsultarHechosEnVivanto(dato);
                if (ProcesarHechos(nv, hechos, out hecho))
                {
                    // uno de los hechos cumple los requisitos 
                    // no es necesario revisar mas los datos basicos de la declaracion
                    break;
                }
            }
            GuardarSiSeEncontroHecho(nv, hecho);
        }

        private List<DatosDetallados> ConsultarHechosEnVivanto(DatosBasicos dato)
        {
            List<DatosDetallados> hechos = new List<DatosDetallados>();
            try {
                hechos = vivanto.ConsultarHechos(dato);
            }
            catch(Exception ex)
            {
                IgnorarOLanzarExcepcion(ex);
            }
            return hechos;
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
                if (!ValidarHechoPorNumeroDeclaracion(hecho, nv) && !ValidarHechoPorFechaValoracion(hecho, nv)) return false;
            }
            else if (hecho.FUENTE == "SIPOD")
            {
                if (!ValidarHechoPorFechaDeclaracion(hecho, nv)) return false;
            }
            else return false;

            return true;
        }


        private void GuardarSiSeEncontroHecho(RuvConsultaNoValorados nv,  DatosDetallados hecho)
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
                        Fecha_Inclusion = hecho.F_VALORACION,
                        Id_Declaracion = nv.Id_Declaracion,
                    };
                    ird.Declaracion_UnidadesInsertar(registro);
                    insertado = true;
                }
            }
            catch (Exception ex)
            {
                IgnorarOLanzarExcepcion(ex);
            }
            finally
            {
                Log.RegistrarProcesado(vivanto.DirInfoLog, nv, hecho, insertado, parProcesamiento);
            }
        }
                        

        private bool ValidarHechoPorFechaDeclaracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
        {
            return nv.Fecha_Declaracion.Date == hecho.F_DECLARACION.Date;
        }

        private bool ValidarHechoPorFechaValoracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
        {
            return hecho.F_VALORACION.Date > nv.Fecha_Valoracion.Date;
        }

        private bool ValidarHechoPorNumeroDeclaracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
        {
            return FnVal.NumeroDeclaracion(nv, hecho);
        }

        private bool ValidarHechoDesplazamentForzado(DatosDetallados hecho)
        {
            try {
                return hecho.HECHO.ToUpper() == parProcesamiento.Hecho;
            }
            catch
            {
                return false;
            }
        }

        private bool ValidarEstadoIncluido(DatosDetallados hecho)
        {
            return parProcesamiento.Obtener_Id_EstadoUnidad(hecho.ESTADO)!=0;
        }

        private void IgnorarOLanzarExcepcion(Exception ex)
        {
            if (IgnorarExcepciones)
                Log.Sesion(vivanto.DirInfoLog, ex.Message);
            else
                throw new ExcepcionProcesamiento(vivanto.DirInfoLog, ex.Message);
        }

    }
}
