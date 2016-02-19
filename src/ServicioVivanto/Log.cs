using DataAccessRest.Entities;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;

namespace ServicioVivanto
{
    public static class Log
    {
		static readonly string NO_VALORADOS_ENCABEZADO = "Id_Declaracion;Regional;TI;Documento;Declaracion;Fecha_Declaracion;Fecha_Radicacion;Fecha_Desplazamiento;Fecha_atencion;En_WS_Vivanto;RUV_ESTADO;RUV_FECHA_VALORACION;RUV_FECHA_DECLARACION;RUV_FECHA_SINIESTRO;RUV_NUM_FUD_NUM_CASO;OK_F_DECLARACION;OK_NUMERO_DECLARACION";
        static readonly string VALORADOS_ENCABEZADO = "Actualizado;Id_Declaracion;Regional;TI;Documento;Declaracion;Fecha_Declaracion;Fecha_Radicacion;Fecha_Desplazamiento;Fecha_atencion;RUV_ESTADO;RUV_FECHA_VALORACION;RUV_FECHA_DECLARACION;RUV_FECHA_SINIESTRO;RUV_NUM_FUD_NUM_CASO;OK_F_DECLARACION;OK_NUMERO_DECLARACION";

        public static void Autorizado(DirectoryInfo dir, string msg)
        {
            LogConexionVivantoImpl(dir,"ConexionVivanto.log", msg);
        }

        public static void AutorizadoExcepcion(DirectoryInfo dir, string msg)
        {
            LogConexionVivantoImpl(dir,"ConexionVivanto.log", msg);
        }

      
        public static void Sesion(DirectoryInfo dir, string msg)
        {
            LogConexionVivantoImpl(dir, "ConexionVivanto.log", msg);
        }


		public static void RegistrarProcesado(DirectoryInfo dir, RuvConsultaNoValorados nv, List<DatosBasicos>basicos,
			List<DatosDetallados> hechos,
			DatosDetallados hecho, bool insertado, ParametrosProcesamiento parProcesamiento)
        {
            // valorados
            // actualizado, id_declaracion, regional, TI, documento, declaracion, fecha_declaracion,  fecha_radicacion, fecha_desplazamiento, fecha_atencion, 
            // RUV_ESTADO, RUV_FECHA_VALORACION, RUV_FECHA_DECLARACION, RUV_FECHA_SINIESTRO, OK_FechaDeclaracion, OK_numero_declaracion
            // no valorados
            //id_declaracion, regional, TI, documento, declaracion, fecha_declaracion,  fecha_radicacion, fecha_desplazamiento, fecha_atencion

            string fn;
            string encabezado;
            var linea = "{0};{1};{2};{3};{4};{5};{6};{7};{8}".Fmt(nv.Id_Declaracion,
                parProcesamiento.ObtenerRegional(nv.Id_Regional),
                nv.Id_Tipo_Identificacion,
                nv.Identificacion,
                nv.Numero_Declaracion,
                nv.Fecha_Declaracion.CsvFecha(),
                nv.Fecha_Radicacion.CsvFecha(),
                nv.Fecha_Desplazamiento.CsvFecha(),
                nv.Fecha_Valoracion.CsvFecha());

            if (hecho == null)
            {
                fn = "No_Valorados.txt";
                encabezado = NO_VALORADOS_ENCABEZADO;     
				linea = "{0};{1}".Fmt (linea, (basicos != null && basicos.Count > 0) ? "SI" : "NO");
				DatosDetallados h;
				if( BuscarHecho(nv, hechos, parProcesamiento, out h)){
					linea = "{0};{1};{2};{3};{4};{5};{6};{7}".Fmt(
						linea,
						h.ESTADO,
						h.F_VALORACION.CsvFecha(),
						h.F_DECLARACION.CsvFecha(),
						h.FECHA_SINIESTRO.CsvFecha(),
						h.NUM_FUD_NUM_CASO,
						h.F_DECLARACION.Date == nv.Fecha_Declaracion.Date ? "SI" : "NO",
						FnVal.NumeroDeclaracion(nv, h)?"SI" : "NO"
					);
				}
            }
            else
            {
                fn = "Valorados.txt";
                encabezado = VALORADOS_ENCABEZADO;
                linea = "{0};{1};{2};{3};{4};{5};{6};{7};{8}".Fmt(insertado ? "SI" : "NO",
                    linea,
                    hecho.ESTADO,
                    hecho.F_VALORACION.CsvFecha(),
                    hecho.F_DECLARACION.CsvFecha(),
                    hecho.FECHA_SINIESTRO.CsvFecha(),
                    hecho.NUM_FUD_NUM_CASO,
                    hecho.F_DECLARACION.Date == nv.Fecha_Declaracion.Date ? "SI" : "NO",
					FnVal.NumeroDeclaracion(nv, hecho)?"SI" : "NO"
                    );
            }
            try {
                AsegurarQueExisteEncabezado(dir, fn, encabezado);
                File.AppendAllText(NombreArhivo(dir, fn), linea + Environment.NewLine);
            }
            catch(Exception)
            {

            }
            
        }


        private static void AsegurarQueExisteEncabezado(DirectoryInfo dir, string nombreArchivo, string encabezado)
        {
            var fn = NombreArhivo(dir, nombreArchivo);
            if (!File.Exists(fn))
            {
                File.AppendAllText(fn, encabezado+Environment.NewLine);
            }
            
        }

        private static void LogConexionVivantoImpl(DirectoryInfo dir, string fn, string msg)
        {
            Console.WriteLine(msg);
            try
            {
                System.IO.File.AppendAllText(NombreArhivo(dir, fn), msg+Environment.NewLine);
            }
            catch (Exception)
            {

            }
        }

        public static string NombreArhivo(DirectoryInfo dir, string fn)
        {
			return FnVal.NombreArhivo (dir, fn);
        }

        static string CsvFecha( this DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

		static string CsvFecha( this DateTime? fecha)
		{
			return fecha.HasValue? fecha.Value.ToString("dd/MM/yyyy"):"";
		}


		static bool BuscarHecho(RuvConsultaNoValorados nv, List<DatosDetallados> hechos, 
			ParametrosProcesamiento parProcesamiento,
			out DatosDetallados  hecho){
			hecho = null;

			foreach (var h in hechos) {
				if (!FnVal.ValidarHechoDesplazamentForzado (h, parProcesamiento))
					continue;
				if (FnVal.NumeroDeclaracion (nv, h) 
					|| FnVal.ValidarHechoPorFechaDeclaracion (h, nv)
					|| FnVal.ValidarHechoPorFechaValoracion(h, nv)) {
					hecho = h;
					break;
				}
			}

			return hecho != null;

		}

		public static void NoProcesados(DirectoryInfo dir, string archivo, List<RuvConsultaNoValorados> noprocesados){
		
			var fn = FnVal.NombreArhivo (dir, archivo);

			using (var tw = new System.IO.FileStream (fn, FileMode.Create)) {
				ServiceStack.Text.JsonSerializer.SerializeToStream(noprocesados, tw);
				tw.Close ();
			}
		}

		public static List<RuvConsultaNoValorados> CargarRuvConsultaNoValoradosDeArchivo( string archivoPorProcesar){
			
			using (var tw = new System.IO.FileStream (archivoPorProcesar, FileMode.Open, FileAccess.Read)) {
				Console.WriteLine ("Cargar de  {0}", archivoPorProcesar);
				return ServiceStack.Text.JsonSerializer.DeserializeFromStream<List<RuvConsultaNoValorados>>(tw);
			}
		}


    }
}
