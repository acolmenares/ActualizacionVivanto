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

            IConexionVivanto cliente = new ConexionVivanto(par, login);
            
            var dbfactory = new OrmLiteConnectionFactory(conexionBD, SqlServerDialect.Provider);
            var dbcliente = new ConexionIRDCOL(dbfactory);

            var proc = new Procesamiento(cliente, dbcliente, parProcesamiento);
			var np = proc.Iniciar(archivoPorProcesar);
            Console.WriteLine("Listo");
			Console.WriteLine ("Iniciando ahora con los no procesados...");
			proc = new Procesamiento(cliente, dbcliente, parProcesamiento);
			proc.Iniciar(np);
            //Console.ReadLine();
            return;
            
        }


    }

}


//Regex.Replace(s, "[^0-9]", "")