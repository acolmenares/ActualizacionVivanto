using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class ExcepcionServicioVivanto:Exception
    {
        public ExcepcionServicioVivanto(DirectoryInfo dir, string mensaje):base(mensaje)
        {
            Log.AutorizadoExcepcion(dir, mensaje);
        }

        
    }
}
