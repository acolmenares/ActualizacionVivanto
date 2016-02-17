using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class RuvConsultaNoValorados
    {
        public int Id_Declaracion { get; set; }
        public int Id_Regional { get; set; }
        public int Id_Persona { get; set; }
        public string Numero_Declaracion { get; set; }
        public DateTime Fecha_Desplazamiento { get; set; }
        public DateTime Fecha_Radicacion { get; set; }
        public DateTime Fecha_Declaracion { get; set; }
        /// <summary>
        /// Fecha Atencion en IRD
        /// </summary>
        public DateTime? Fecha_Valoracion { get; set; }  
        public int Id_Tipo_Identificacion { get; set; }
        public string Identificacion { get; set; }
        public string Primer_Apellido{ get; set; }
        public string Segundo_Apellido { get; set; }
        public string Primer_Nombre { get; set; }
        public string Segundo_Nombre { get; set; }

    }
}
