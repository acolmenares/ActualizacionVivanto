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
        static readonly string NO_VALORADOS_ENCABEZADO = "Id_Declaracion;Regional;TI;Documento;Declaracion;Fecha_Declaracion;Fecha_Radicacion;Fecha_Desplazamiento;Fecha_atencion";
        static readonly string VALORADOS_ENCABEZADO = "Actualizado;Id_Declaracion;Regional;TI;Documento;Declaracion;Fecha_Declaracion;Fecha_Radicacion;Fecha_Desplazamiento;Fecha_atencion;RUV_ESTADO;RUV_FECHA_VALORACION;RUV_FECHA_DECLARACION;RUV_FECHA_SINIESTRO;RUV_NUM_FUD_NUM_CASO;OK_F_DECLARACION;OK_NUMERO_DECLARACION";

        public static void Autorizado(string msg)
        {
            LogAutroizadoImpl("ConexionVivanto_Autorizado.log", msg);
        }

        public static void AutorizadoExcepcion(string msg)
        {
            LogAutroizadoImpl("ConexionVivanto_AutorizadoExcepcion.log", msg);
        }

      
        public static void Sesion(string sesion, string msg)
        {
            Console.WriteLine(msg);
            try
            {
                var fn = sesion + ".log";
                File.AppendAllText(fn, msg+Environment.NewLine);
            }
            catch (Exception)
            {

            }
        }


        public static void RegistrarProcesado(string sesion, RuvConsultaNoValorados nv, DatosDetallados hecho, bool insertado, ParametrosProcesamiento parProcesamiento)
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
                fn = "{0}_{1}".Fmt(sesion, "No_Valorados.txt");
                encabezado = NO_VALORADOS_ENCABEZADO;               
            }
            else
            {
                fn = "{0}_{1}".Fmt(sesion, "Valorados.txt");
                encabezado = VALORADOS_ENCABEZADO;
                linea = "{0};{1};{2};{3};{4};{5};{6};{7};{8}".Fmt(insertado ? "SI" : "NO",
                    linea,
                    hecho.ESTADO,
                    hecho.F_VALORACION.CsvFecha(),
                    hecho.F_DECLARACION.CsvFecha(),
                    hecho.FECHA_SINIESTRO.CsvFecha(),
                    hecho.NUM_FUD_NUM_CASO,
                    hecho.F_DECLARACION.Date == nv.Fecha_Declaracion.Date ? "SI" : "NO",
                    FnVal.NumeroDeclaracion(nv, hecho)
                    );
            }
            try {
                AsegurarQueExisteEncabezado(fn, encabezado);
                File.AppendAllText(fn, linea + Environment.NewLine);
            }
            catch(Exception)
            {

            }
            
        }


        private static void AsegurarQueExisteEncabezado(string nombreArchivo, string encabezado)
        {

            if (!File.Exists(nombreArchivo))
            {
                File.AppendAllText(nombreArchivo, encabezado+Environment.NewLine);
            }
            
        }

        private static void LogAutroizadoImpl(string archivo, string msg)
        {
            Console.WriteLine(msg);
            try
            {
                var fn = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + archivo;
                System.IO.File.WriteAllText(fn, msg);
            }
            catch (Exception)
            {

            }
        }

        static string CsvFecha( this DateTime fecha)
        {
            return fecha.ToString("dd/MM/yyyy");
        }

    }
}
