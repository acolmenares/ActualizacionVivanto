using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicioVivanto
{
    public class ParametrosServicio
    {
        public ParametrosServicio()
        {
            IdAplicacion = "146";
            UrlBase = "http://190.60.70.149:82";
            UrlLogin = "LoginRest/Autentica.svc/Login";
            UrlLogout = "LoginRest/Autentica.svc/Logout";
            UrlConsultarDocumento = "VivantoMovilRest/ServiceMovil.svc/Documento";
            UrlConsultarHechos = "VivantoMovilRest/ServiceMovil.svc/Hechos";
        }

        public string IdAplicacion { get; set; }
        public string UrlBase { get; set; }
        public string UrlLogin { get; set; }
        public string UrlLogout { get; set; }
        public string UrlConsultarDocumento { get; set; }
        public string UrlConsultarHechos { get; set; }
            }
}



//http://190.60.70.149:82/LoginRest/Autentica.svc/Login/146,AICOLMENARESA,XXXXXXXXXX
//http://190.60.70.149:82/LoginRest/Autentica.svc/Logout/146,IdUsuario,Token
//http://190.60.70.149:82/VivantoMovilRest/ServiceMovil.svc/Documento/146,IdUsuario,Token,DOCUMENTO
//http://190.60.70.149:82/VivantoMovilRest/ServiceMovil.svc/Hechos/146,IdUsuario,Token,ID_PERSONA,FUENTE
