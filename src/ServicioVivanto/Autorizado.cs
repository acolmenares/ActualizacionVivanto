using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestServiceAutentica
{

    public class Autorizado
    {
        public string IdUsuario { get; set; }
        public string Token { get; set; }
    }

}

/*
 <ArrayOfAutorizado xmlns="http://schemas.datacontract.org/2004/07/RestServiceAutentica" xmlns:i="http://www.w3.org/2001/XMLSchema-instance">
<Autorizado>
<IdUsuario>
El usuario o la clave es INCORRECTO, vuelva a intentar....
</IdUsuario>
<Token>
El usuario o la clave es INCORRECTO, vuelva a intentar....
</Token>
</Autorizado>
</ArrayOfAutorizado>
 */
