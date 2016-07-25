using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DataAccessRest.Entities
{
    public class DatosBasicos
    {
        public string APELLIDO1 { get; set; }
        public string APELLIDO2 { get; set; }
        public string DISCAPACIDAD { get; set; }
        public string DOCUMENTO { get; set; }
        public string ESTADO_BINARIO { get; set; }
        public string ESTADO_TRANSACCION { get; set; }
        public string ETNIA { get; set; }
        public string FUENTE { get; set; }
        public string F_NACIMIENTO { get; set; }
        public string GENERO { get; set; }
        public string ID_PERSONA { get; set; }
        public string NOMBRE1 { get; set; }
        public string NOMBRE2 { get; set; }
        public string TIPO_DOCUMENTO { get; set; }
    }

}

/*
 <APELLIDO1>MARTINEZ</APELLIDO1>
<APELLIDO2>MARTINEZ</APELLIDO2>
<DISCAPACIDAD>-NINGUNA</DISCAPACIDAD>
<DOCUMENTO>25713773</DOCUMENTO>
<ESTADO_BINARIO>1</ESTADO_BINARIO>
<ESTADO_TRANSACCION>EXITOSA</ESTADO_TRANSACCION>
<ETNIA>Ninguna</ETNIA>
<FUENTE>RUV</FUENTE>
<F_NACIMIENTO>8/1/1965 12:00:00 AM</F_NACIMIENTO>
<GENERO>Mujer</GENERO>
<ID_PERSONA>13721331</ID_PERSONA>
<NOMBRE1>ANGELA</NOMBRE1>
<NOMBRE2>OLIVA</NOMBRE2>
<TIPO_DOCUMENTO>Cédula de Ciudadanía</TIPO_DOCUMENTO>

    <DatosBasicos>
<APELLIDO1>TORO</APELLIDO1>
<APELLIDO2>NARVAEZ</APELLIDO2>
<DISCAPACIDAD>-NINGUNA</DISCAPACIDAD>
<DOCUMENTO>48604166</DOCUMENTO>
<ESTADO_BINARIO>1</ESTADO_BINARIO>
<ESTADO_TRANSACCION>EXITOSA</ESTADO_TRANSACCION>
<ETNIA>Ninguna</ETNIA>
<FUENTE>RUV</FUENTE>
<F_NACIMIENTO>5/15/1973 12:00:00 AM</F_NACIMIENTO>
<GENERO>Mujer</GENERO>
<ID_PERSONA>13770962</ID_PERSONA>
<NOMBRE1>BLANCA</NOMBRE1>
<NOMBRE2>ELVIS</NOMBRE2>
<TIPO_DOCUMENTO>Cédula de Ciudadanía</TIPO_DOCUMENTO>
</DatosBasicos>


 */
