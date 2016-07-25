using System;
using System.Globalization;
using System.Runtime.Serialization;
using System.Xml.Serialization;

namespace DataAccessRest.Entities
{
    [DataContract]
    public class DatosDetallados
    {
        [DataMember]
        public string APELLIDO1 { get; set; }
        [DataMember]
        public string APELLIDO2 { get; set; }
        [DataMember]
        public string DEPTO_DECLA { get; set; }
        [DataMember]
        public string DEPTO_OCU { get; set; }
        [DataMember]
        public string DISCAPACIDAD { get; set; }
        [DataMember]
        public string DOCUMENTO { get; set; }
        [DataMember]
        public string ESTADO { get; set; }
        [DataMember]
        public string ESTADO_BINARIO { get; set; }
        [DataMember]
        public string ESTADO_TRANSACCION { get; set; }
        [DataMember]
        public string ETNIA { get; set; }

        public DateTime FECHA_SINIESTRO { get; set; }
        [DataMember(Name ="FECHA_SINIESTRO")]
        public string FechaSiniestro
        {
            get { return FECHA_SINIESTRO.ToString("M/d/yyyy"); }
            set
            {
                var fecha = ToDate(value);
                FECHA_SINIESTRO = fecha.HasValue ? fecha.Value : DateTime.MinValue;

            }
        }

        [DataMember]
        public string FUENTE { get; set; }
        
        public DateTime F_DECLARACION { get; set; }
        [DataMember(Name ="F_DECLARACION")]
        public string FechaDeclaracion
        {
            get { return F_DECLARACION.ToString("d/MM/yyyy"); }
            set
            {
                var fecha = ToDate(value);
                F_DECLARACION = fecha.HasValue ? fecha.Value : DateTime.MinValue;

            }
        }

        public DateTime F_NACIMIENTO { get; set; }
        [DataMember(Name ="F_NACIMIENTO")]
        public string FechaDNacimiento
        {
            get { return F_NACIMIENTO.ToString("d/MM/yyyy"); }
            set
            {
                var fecha = ToDate(value);
                F_NACIMIENTO = fecha.HasValue ? fecha.Value : DateTime.MinValue;

            }
        }

        public DateTime? F_VALORACION { get; set; }
        [DataMember(Name ="F_VALORACION")]
        public string FechaValoracion
        {
            get { return F_VALORACION.HasValue? F_VALORACION.Value.ToString("d/MM/yyyy"):""; }
            set
            {
                F_VALORACION = ToDate(value);
            }
        }

        [DataMember]
        public string GENERO { get; set; }
        [DataMember]
        public string HECHO { get; set; }
        [DataMember]
        public string ID_ANEXO { get; set; }
        [DataMember]
        public string ID_DECLARACION { get; set; }
        [DataMember]
        public string ID_MIJEFE { get; set; }
        [DataMember]
        public string ID_PERSONA { get; set; }
        [DataMember]
        public string ID_REG_PERSONA { get; set; }
        [DataMember]
        public string ID_SINIESTRO { get; set; }
        [DataMember]
        public string MUN_DECLA { get; set; }
        [DataMember]
        public string MUN_OCU { get; set; }
        [DataMember]
        public string NOMBRE1 { get; set; }
        [DataMember]
        public string NOMBRE2 { get; set; }
        [DataMember]
        public string NUM_FUD_NUM_CASO { get; set; }
        [DataMember]
        public string PARAM_HECHO { get; set; }
        [DataMember]
        public string RELACION { get; set; }
        [DataMember]
        public string RESPONSABLE { get; set; }
        [DataMember]
        public string TIPO_DESPLA { get; set; }
        [DataMember]
        public string TIPO_DOCUMENTO { get; set; }
        [DataMember]
        public string TIPO_VICTIMA { get; set; }

        private DateTime? ToDate(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;

            var espacio = value.IndexOf(" ");

            var sfecha = value.Substring(0, espacio);

            return DateTime.ParseExact(sfecha, "M/d/yyyy", CultureInfo.InvariantCulture);        
            
        }

    }
}

/*
 <ArrayOfDatosDetallados xmlns="http://schemas.datacontract.org/2004/07/DataAccessRest.Entities" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
<DatosDetallados>
<APELLIDO1>MOSQUERA</APELLIDO1>
<APELLIDO2>LEITON</APELLIDO2>
<DEPTO_DECLA>CAUCA</DEPTO_DECLA>
<DEPTO_OCU>CAUCA</DEPTO_OCU>
<DISCAPACIDAD>-NINGUNA</DISCAPACIDAD>
<DOCUMENTO>76345095</DOCUMENTO>
<ESTADO>DECLARACION EN PROCESO DE VALORACION (-)</ESTADO>
<ESTADO_BINARIO>1</ESTADO_BINARIO>
<ESTADO_TRANSACCION>EXITOSA</ESTADO_TRANSACCION>
<ETNIA>Ninguna</ETNIA>
<FECHA_SINIESTRO>4/6/2016 12:00:00 AM</FECHA_SINIESTRO>
<FUENTE>RUV</FUENTE>
<F_DECLARACION>4/8/2016 12:00:00 AM</F_DECLARACION>
<F_NACIMIENTO>4/3/1982 12:00:00 AM</F_NACIMIENTO>
<F_VALORACION i:nil="true"/>
<GENERO>Hombre</GENERO>
<HECHO>Desplazamiento forzado</HECHO>
<ID_ANEXO>1290301</ID_ANEXO>
<ID_DECLARACION>3322289</ID_DECLARACION>
<ID_MIJEFE>14630117</ID_MIJEFE>
<ID_PERSONA>15022692</ID_PERSONA>
<ID_REG_PERSONA>14630117</ID_REG_PERSONA>
<ID_SINIESTRO>1931574</ID_SINIESTRO>
<MUN_DECLA>POPAYÁN</MUN_DECLA>
<MUN_OCU>LA VEGA</MUN_OCU>
<NOMBRE1>DIVER</NOMBRE1>
<NOMBRE2 i:nil="true"/>
<NUM_FUD_NUM_CASO>NI000643478</NUM_FUD_NUM_CASO>
<PARAM_HECHO>5</PARAM_HECHO>
<RELACION>Jefe(a) de hogar (Declarante)</RELACION>
<RESPONSABLE>Sin Información</RESPONSABLE>
<TIPO_DESPLA>Individual</TIPO_DESPLA>
<TIPO_DOCUMENTO>Cédula de Ciudadanía</TIPO_DOCUMENTO>
<TIPO_VICTIMA>DIRECTA</TIPO_VICTIMA>
</DatosDetallados>
</ArrayOfDatosDetallados>
 */
