using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public interface IConexionIRDCOL
    {
        List<RuvConsultaNoValorados> ConsultarNoActualizadosVUR(DateTime? fechaRadicacionInicial = null, DateTime? fechaRadicacionFinal = null);
        void Declaracion_UnidadesInsertar(Declaracion_UnidadesInsertar registro);
    }

}
