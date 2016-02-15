using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class ExcepcionProcesamiento:Exception
    {
        public ExcepcionProcesamiento(string msg, string token) : base(msg)
        {
            Log.Sesion(token, msg);
        }
    }
}
