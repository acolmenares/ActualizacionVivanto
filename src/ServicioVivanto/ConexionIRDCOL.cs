using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class ConexionIRDCOL:IConexionIRDCOL
    {
        private IDbConnectionFactory dbFactory;

        public ConexionIRDCOL(IDbConnectionFactory dbFactory)
        {
            this.dbFactory = dbFactory;
            
        }
        /// <summary>
        /// Relación de Desplazados - Declarantes Elegibles Atender (Si o No Definido)
        /// sin registro en Declaracion_Unidades
        /// </summary>
        /// <param name="fechaRadicacionInicial">Radicados en IRD Desde</param>
        /// <param name="fechaRadicacionFinal">Radicados en IRD Hasta</param>
        /// <returns></returns>
        public List<RuvConsultaNoValorados> ConsultarNoActualizadosVUR(DateTime? fechaRadicacionInicial=null, DateTime? fechaRadicacionFinal=null)
        {
          
            using (var con = dbFactory.Open())
            {
                var r = con.SqlList<RuvConsultaNoValorados>("EXEC RuvConsultaNoValorados @FechaRadicacionInicial, @FechaRadicacionFinal",
                    new { FechaRadicacionInicial = fechaRadicacionInicial, FechaRadicacionFinal = fechaRadicacionFinal });
                return r;
            }
          
        }

        public void Declaracion_UnidadesInsertar(Declaracion_UnidadesInsertar registro)
        {
           
            using (var con = dbFactory.Open())
            {
                con.SqlScalar<int>("EXEC Declaracion_UnidadesInsertar @Id_Declaracion, @Id_Unidad, @Id_EstadoUnidad,  @Fecha_Inclusion, @Fecha_Investigacion, @Fecha_Creacion, @Id_Usuario_Creacion, @Fecha_Modificacion, @Id_Usuario_Modificacion, @Fecha_Cierre , @Cierre , @Fuente ", registro);
            }
        }
    }
}
