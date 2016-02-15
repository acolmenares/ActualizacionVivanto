using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class Declaracion_UnidadesInsertar
    {
        public Declaracion_UnidadesInsertar()
        {
            Fecha_Investigacion = DateTime.Today;
            Fecha_Creacion = DateTime.Today;
            Id_Usuario_Creacion = 1;   
        }

        public int Id_Declaracion { get; set; }
        public int Id_Unidad { get; set; }
        public int Id_EstadoUnidad { get; set; }
        public DateTime Fecha_Inclusion { get; set; }
        public DateTime Fecha_Investigacion { get; set; }
        public DateTime Fecha_Creacion { get; set; }
        public int Id_Usuario_Creacion { get; set; }
        public DateTime? Fecha_Modificacion { get; set; }
        public int? Id_Usuario_Modificacion { get; set; }
        public DateTime? Fecha_Cierre { get; set; }
        public bool? Cierre { get; set; }
        public string Fuente { get; set; }
    }
}

