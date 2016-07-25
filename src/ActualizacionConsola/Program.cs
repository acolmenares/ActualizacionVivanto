using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessRest.Entities;
using RestServiceAutentica;
using ServiceStack;
using System.Net;
using ServicioVivanto;
using ServiceStack.OrmLite;
using ServiceStack.Data;
using ServiceStack.Configuration;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
            string archivoPorProcesar = null;

			if (args.Length > 0) {
				archivoPorProcesar = args [0];
				Console.WriteLine (archivoPorProcesar);
			}
                        
            var appSettings = new AppSettings();

            var fechaRadicacionInicial = appSettings.Get<DateTime>("FechaRadicacionInicial", new DateTime(2015, 1, 7));
            var fechaRadicacionFinal = appSettings.Get<DateTime>("FechaRadicacionFinal", DateTime.Today);
            var urlBaseVivanto = appSettings.Get<string>("UrlBaseVivanto", "http://190.60.70.149:82");
            var varConexionBD = appSettings.Get<string>("ConexionBD", "APP_CONEXION_IRD");
            var varUsuarioVivanto = appSettings.Get<string>("UsuarioVivanto", "APP_USUARIO_VIVANTO");
            var varClaveVivanto = appSettings.Get<string>("ClaveVivanto", "APP_CLAVE_VIVANTO");

            var conexionBD = Environment.GetEnvironmentVariable(varConexionBD);
            var usuarioVivanto = Environment.GetEnvironmentVariable(varUsuarioVivanto);
            var claveVivanto = Environment.GetEnvironmentVariable(varClaveVivanto);

            Console.WriteLine(fechaRadicacionInicial);
            Console.WriteLine(fechaRadicacionFinal);
            Console.WriteLine(urlBaseVivanto);
            Console.WriteLine(varConexionBD);
            Console.WriteLine(varUsuarioVivanto);
            Console.WriteLine(varClaveVivanto);
            Console.WriteLine(conexionBD);
            Console.WriteLine(usuarioVivanto);
            Console.WriteLine(claveVivanto);
                                    
            var login = new LoginVivanto { Usuario = usuarioVivanto, Clave = claveVivanto };

            var par = new ParametrosServicio() { UrlBase = urlBaseVivanto };
            var parProcesamiento = new ParametrosProcesamiento()
            {
                FechaRadicacionInicial = fechaRadicacionInicial,
                FechaRadicacionFinal = fechaRadicacionFinal
            };

			using (IConexionVivanto cliente = new ConexionVivanto (par, login)) {

				var dbfactory = new OrmLiteConnectionFactory (conexionBD, SqlServerDialect.Provider);
				var dbcliente = new ConexionIRDCOL (dbfactory);
				cliente.IniciarSesion ();
				var proc = new Procesamiento (cliente, dbcliente, parProcesamiento);
				var np = proc.Iniciar (archivoPorProcesar);
				Console.WriteLine ("Listo primera fase");
				Console.WriteLine ("esperando 5 segundos para iniciar con los no procesados");
				System.Threading.Thread.Sleep (5 * 1000);
				Console.WriteLine ("Iniciando ahora con los no procesados...");
				//proc = new Procesamiento (cliente, dbcliente, parProcesamiento);
				proc.Iniciar (np);
				cliente.CerrarSession ();
			}
            
            return;
            
        }


    }

}

//<ArrayOfDatosDetallados xmlns="http://schemas.datacontract.org/2004/07/DataAccessRest.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance"><DatosDetallados><APELLIDO1>CORDOBA</APELLIDO1><APELLIDO2>GAITAN</APELLIDO2><DEPTO_DECLA>HUILA</DEPTO_DECLA><DEPTO_OCU>CAQUETÁ</DEPTO_OCU><DISCAPACIDAD>-NINGUNA</DISCAPACIDAD><DOCUMENTO>99110308848</DOCUMENTO><ESTADO>No Incluido</ESTADO><ESTADO_BINARIO>1</ESTADO_BINARIO><ESTADO_TRANSACCION>EXITOSA</ESTADO_TRANSACCION><ETNIA>Ninguna</ETNIA><FECHA_SINIESTRO>1/13/2006 12:00:00 AM</FECHA_SINIESTRO><FUENTE>RUV</FUENTE><F_DECLARACION>1/20/2015 12:00:00 AM</F_DECLARACION><F_NACIMIENTO>11/3/1999 12:00:00 AM</F_NACIMIENTO><F_VALORACION>7/7/2015 3:43:52 PM</F_VALORACION><GENERO>Hombre</GENERO><HECHO>Desplazamiento forzado</HECHO><ID_ANEXO>776950</ID_ANEXO><ID_DECLARACION>2913083</ID_DECLARACION><ID_MIJEFE>13232260</ID_MIJEFE><ID_PERSONA>14988356</ID_PERSONA><ID_REG_PERSONA>13263459</ID_REG_PERSONA><ID_SINIESTRO>1340708</ID_SINIESTRO><MUN_DECLA>PITALITO</MUN_DECLA><MUN_OCU>SOLANO</MUN_OCU><NOMBRE1>SANTIAGO</NOMBRE1><NOMBRE2 i:nil="true"/><NUM_FUD_NUM_CASO>CL000211448</NUM_FUD_NUM_CASO><PARAM_HECHO>5</PARAM_HECHO><RELACION>Hijo(a)/Hijastro(a)</RELACION><RESPONSABLE>Grupos Guerrilleros</RESPONSABLE><TIPO_DESPLA>Individual</TIPO_DESPLA><TIPO_DOCUMENTO>Tarjeta de Identidad</TIPO_DOCUMENTO><TIPO_VICTIMA>DIRECTA</TIPO_VICTIMA></DatosDetallados><DatosDetallados><APELLIDO1>CORDOBA</APELLIDO1><APELLIDO2>GAITAN</APELLIDO2><DEPTO_DECLA>CAQUETÁ</DEPTO_DECLA><DEPTO_OCU>NARIÑO</DEPTO_OCU><DISCAPACIDAD>-NINGUNA</DISCAPACIDAD><DOCUMENTO>99110308848</DOCUMENTO><ESTADO>Incluido</ESTADO><ESTADO_BINARIO>1</ESTADO_BINARIO><ESTADO_TRANSACCION>EXITOSA</ESTADO_TRANSACCION><ETNIA>Ninguna</ETNIA><FECHA_SINIESTRO>2/18/2016 12:00:00 AM</FECHA_SINIESTRO><FUENTE>RUV</FUENTE><F_DECLARACION>3/15/2015 12:00:00 AM</F_DECLARACION><F_NACIMIENTO>11/3/1999 12:00:00 AM</F_NACIMIENTO><F_VALORACION>7/1/2016 12:00:00 AM</F_VALORACION><GENERO>Hombre</GENERO><HECHO>Desplazamiento forzado</HECHO><ID_ANEXO>1282094</ID_ANEXO><ID_DECLARACION>3315098</ID_DECLARACION><ID_MIJEFE>14597561</ID_MIJEFE><ID_PERSONA>14988356</ID_PERSONA><ID_REG_PERSONA>14597561</ID_REG_PERSONA><ID_SINIESTRO>1924114</ID_SINIESTRO><MUN_DECLA>FLORENCIA</MUN_DECLA><MUN_OCU>Tumaco</MUN_OCU><NOMBRE1>SANTIAGO</NOMBRE1><NOMBRE2 i:nil="true"/><NUM_FUD_NUM_CASO>NF000652085</NUM_FUD_NUM_CASO><PARAM_HECHO>5</PARAM_HECHO><RELACION>Jefe(a) de hogar (Declarante)</RELACION><RESPONSABLE>Grupos Guerrilleros</RESPONSABLE><TIPO_DESPLA>Individual</TIPO_DESPLA><TIPO_DOCUMENTO>Tarjeta de Identidad</TIPO_DOCUMENTO><TIPO_VICTIMA>DIRECTA</TIPO_VICTIMA></DatosDetallados><DatosDetallados><APELLIDO1>CORDOBA</APELLIDO1><APELLIDO2>GAITAN</APELLIDO2><DEPTO_DECLA>CAQUETÁ</DEPTO_DECLA><DEPTO_OCU>NARIÑO</DEPTO_OCU><DISCAPACIDAD>-NINGUNA</DISCAPACIDAD><DOCUMENTO>99110308848</DOCUMENTO><ESTADO>Incluido</ESTADO><ESTADO_BINARIO>1</ESTADO_BINARIO><ESTADO_TRANSACCION>EXITOSA</ESTADO_TRANSACCION><ETNIA>Ninguna</ETNIA><FECHA_SINIESTRO>2/10/2016 12:00:00 AM</FECHA_SINIESTRO><FUENTE>RUV</FUENTE><F_DECLARACION>3/15/2015 12:00:00 AM</F_DECLARACION><F_NACIMIENTO>11/3/1999 12:00:00 AM</F_NACIMIENTO><F_VALORACION>7/1/2016 12:00:00 AM</F_VALORACION><GENERO>Hombre</GENERO><HECHO>Amenaza</HECHO><ID_ANEXO>615086</ID_ANEXO><ID_DECLARACION>3315098</ID_DECLARACION><ID_MIJEFE>14597561</ID_MIJEFE><ID_PERSONA>14988356</ID_PERSONA><ID_REG_PERSONA>14597561</ID_REG_PERSONA><ID_SINIESTRO>1924113</ID_SINIESTRO><MUN_DECLA>FLORENCIA</MUN_DECLA><MUN_OCU>Tumaco</MUN_OCU><NOMBRE1>SANTIAGO</NOMBRE1><NOMBRE2 i:nil="true"/><NUM_FUD_NUM_CASO>NF000652085</NUM_FUD_NUM_CASO><PARAM_HECHO>2</PARAM_HECHO><RELACION>Jefe(a) de hogar (Declarante)</RELACION><RESPONSABLE>Grupos Guerrilleros</RESPONSABLE><TIPO_DESPLA>NO APLICA</TIPO_DESPLA><TIPO_DOCUMENTO>Tarjeta de Identidad</TIPO_DOCUMENTO><TIPO_VICTIMA>DIRECTA</TIPO_VICTIMA></DatosDetallados></ArrayOfDatosDetallados>
//Regex.Replace(s, "[^0-9]", "")