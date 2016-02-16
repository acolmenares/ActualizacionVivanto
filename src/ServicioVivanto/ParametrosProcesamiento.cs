using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class ParametrosProcesamiento
    {

        public ParametrosProcesamiento()
        {
            Id_Unidad = 32;
            Hecho = "Desplazamiento forzado".ToUpper();
            Id_Usuario_Creacion = 1;
            Id_EstadoUnidad_Incluido = 371;
            Id_EstadoUnidad_NoIncluido = 372;
            Id_Regional_Florencia = 1637;
            Id_Regional_Popayan = 4521;

        }

        public DateTime? FechaRadicacionInicial { get; set; }
        public DateTime? FechaRadicacionFinal { get; set; }


        public string Hecho { get; set; }
        public int Id_Unidad { get; set; }
        public int Id_EstadoUnidad_Incluido { get; set; }
        public int Id_EstadoUnidad_NoIncluido { get; set; }

        public int Id_Usuario_Creacion { get; set; }

        public int Id_Regional_Popayan { get; set; }

        public int Id_Regional_Florencia { get; set; }

        public string ObtenerRegional(int id_regional)
        {
            return id_regional == Id_Regional_Florencia ? "FLORENCIA" : id_regional == Id_Regional_Popayan ? "POPAYAN" : "";
        }

        public int Obtener_Id_EstadoUnidad( string estado_)
        {
            var e = !string.IsNullOrEmpty(estado_)?  estado_.ToUpper():"";
            return e == "INCLUIDO" ? Id_EstadoUnidad_Incluido : e == "NO INCLUIDO" ? Id_EstadoUnidad_NoIncluido : 0;
        }
    }


}
