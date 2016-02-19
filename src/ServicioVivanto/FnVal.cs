using DataAccessRest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.IO;

namespace ServicioVivanto
{
    public static class FnVal
    {

        public static bool NumeroDeclaracion(RuvConsultaNoValorados nv, DatosDetallados hecho)
        {
            var irdFUD = Regex.Replace(nv.Numero_Declaracion??"", "[^0-9]", "");
            var fud = Regex.Replace(hecho.NUM_FUD_NUM_CASO??"", "[^0-9]", "");

            long ird=0;
            long.TryParse(irdFUD, out ird);
            long vivanto = 0;
            long.TryParse(fud, out vivanto);

            Console.WriteLine("irdFud {0}; fud {1};  ird {2}  vivanto {3}; evaluacion {4}",
                irdFUD, fud, ird, vivanto,
                ird != 0 && vivanto != 0 && ird == vivanto);

            return ird != 0 && vivanto != 0 && ird == vivanto;
			                        
        }

		public static bool ValidarHechoDesplazamentForzado(DatosDetallados hecho, ParametrosProcesamiento parProcesamiento)
		{
			return (!string.IsNullOrEmpty(hecho.HECHO) && hecho.HECHO.ToUpper() == parProcesamiento.Hecho);
		}

		public static bool ValidarHechoPorFechaDeclaracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
		{
			return nv.Fecha_Declaracion.Date == hecho.F_DECLARACION.Date;
		}


		public static bool ValidarHechoPorFechaValoracion(DatosDetallados hecho, RuvConsultaNoValorados nv)
		{
			return (hecho.F_VALORACION.HasValue && nv.Fecha_Valoracion.HasValue)?
				hecho.F_VALORACION.Value.Date > nv.Fecha_Valoracion.Value.Date: false;
		}


		public static string NombreArhivo(DirectoryInfo dir, string fn)
		{
			return Path.Combine(dir.FullName, fn);
		}


    }
}
