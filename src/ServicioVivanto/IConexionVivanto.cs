using DataAccessRest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public interface IConexionVivanto:IDisposable
    {
        void IniciarSesion();
        void CerrarSession();
        List<DatosBasicos> ConsultarDatosBasicos(string documento);
        List<DatosBasicos> ConsultarDatosBasicos(int documento);
        List<DatosDetallados> ConsultarHechos(DatosBasicos datoBasico);
        List<DatosDetallados> ConsultarHechos(string IdPersona, string fuente);
        string Token { get; }
        string HoraProceso { get; }

    }
}
