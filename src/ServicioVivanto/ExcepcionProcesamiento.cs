using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class ExcepcionProcesamiento:Exception
    {
        public ExcepcionProcesamiento(DirectoryInfo dir, string msg) : base(msg)
        {
            Log.Sesion(dir, msg);
        }
    }
}
