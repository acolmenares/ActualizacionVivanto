﻿using DataAccessRest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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

            //return Regex.Replace(nv.Numero_Declaracion, "[^0-9]", "") == Regex.Replace(hecho.NUM_FUD_NUM_CASO, "[^0-9]", "");
            
        }
    }
}