using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class ExcepcionServicioVivanto:Exception
    {
        public ExcepcionServicioVivanto(string mensaje):base(mensaje)
        {
            Log.AutorizadoExcepcion(mensaje);
        }

        public ExcepcionServicioVivanto(string mensaje, string token) : base(mensaje)
        {
            Log.Sesion(token, mensaje);
        }
    }
}
