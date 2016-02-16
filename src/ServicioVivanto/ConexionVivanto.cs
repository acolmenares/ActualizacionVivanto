using ServiceStack;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestServiceAutentica;
using DataAccessRest.Entities;
using System.Net.Http;
using System.IO;

namespace ServicioVivanto
{
    public class ConexionVivanto : IConexionVivanto
    {
        ParametrosServicio parametros;
        LoginVivanto login;
        XmlServiceClient cliente=null;
        Autorizado autorizado=null;
        
        public string HoraProceso { get; private set; }
        public DirectoryInfo DirInfoLog { get; private set; }

        public ConexionVivanto(ParametrosServicio parametros, LoginVivanto login)
        {
            this.parametros = parametros;
            this.login = login;
            HoraProceso = DateTime.Now.ToString("yyyyMMddHHmmss");
            DirInfoLog =Directory.CreateDirectory(HoraProceso);
        }

        public void IniciarSesion()
        {
            if(autorizado != null)
            {
                CerrarConexionVivanto();
            }

            disposedValue = false;
            cliente = new XmlServiceClient(parametros.UrlBase);
            
            using (var httpResponse = cliente.Get<HttpWebResponse>("{0}/{1},{2},{3}".Fmt(parametros.UrlLogin, parametros.IdAplicacion, login.Usuario, login.Clave)))
            {
                var responseStream = httpResponse.GetResponseStream();
                var r2 = responseStream.ToUtf8String();
                var dd = ServiceStack.Text.XmlSerializer.DeserializeFromString<List<Autorizado>>(r2);
                if (dd.Count > 0)
                {
                    autorizado = dd[0];
                    long prueba;
                    if (!long.TryParse(autorizado.IdUsuario, out prueba))
                    {
                        var msg = autorizado.IdUsuario;
                        autorizado = null;
                        throw new ExcepcionServicioVivanto(DirInfoLog, msg);
                    }

                    Log.Autorizado(DirInfoLog, "{0} {1}".Fmt(autorizado.IdUsuario, autorizado.Token));
                    return;
                }
            }
            throw new ExcepcionServicioVivanto(DirInfoLog, "Metodo de autenticación no retornó Autorizados");
        }


        public void CerrarSession()
        {
            CerrarConexionVivanto();
        }

        private T ObtenerRespuesta<T>(string urlPeticion)
        {
            try
            {
                /*using (var httpResponse = cliente.Get<HttpWebResponse>(urlPeticion))
                {
                    var responseStream = httpResponse.GetResponseStream();
                    var r2 = responseStream.ToUtf8String();
                    return  ServiceStack.Text.XmlSerializer.DeserializeFromString<T>(r2);
                }
                */

                var r2 = GetHttpResponse(parametros.UrlBase + "/" + urlPeticion);
                return ServiceStack.Text.XmlSerializer.DeserializeFromString<T>(r2);

            }
            catch (HttpRequestException wex)
            {
                throw new ExcepcionServicioVivanto(DirInfoLog, "{0}{1}{2}/{3}{4}{5}{6}"
                    .Fmt(wex.Message, Environment.NewLine,
                    cliente.BaseUri, urlPeticion, Environment.NewLine,
                    wex.GetResponseBody(), Environment.NewLine
                    ));
            }
            catch (WebServiceException wex)
            {
                throw new ExcepcionServicioVivanto(DirInfoLog,"{0}{1}StatusCode:{2} ErrorCode:{3} ErrorMessage: {4}{5}{6}/{7}{8}{9}{10}"
                    .Fmt(wex.Message, Environment.NewLine,
                    wex.StatusCode, wex.ErrorCode, wex.ErrorMessage, Environment.NewLine,
                    cliente.BaseUri, urlPeticion, Environment.NewLine,
                    wex.ResponseBody, Environment.NewLine
                    ));
            }
            catch (WebException wex)
            {
                throw new ExcepcionServicioVivanto(DirInfoLog,"{0}{1}Status:{2}{3}{4}/{5}{6}{7}{8}"
                    .Fmt(wex.Message, Environment.NewLine,
                    wex.Status,  Environment.NewLine,
                    cliente.BaseUri, urlPeticion, Environment.NewLine,
                    wex.GetResponseBody(), Environment.NewLine
                    ));
            }
            catch (Exception ex)
            {
                throw new ExcepcionServicioVivanto(DirInfoLog, "{0}{1}".Fmt(ex.Message, Environment.NewLine));
            }
        }

        public List<DatosBasicos> ConsultarDatosBasicos(int documento)
        {
            return ConsultarDatosBasicos(documento.ToString());
        }

        public List<DatosBasicos> ConsultarDatosBasicos(string documento)
        {
            var r = ObtenerRespuesta<List<DatosBasicos>>("{0}/{1},{2},{3},{4}".Fmt(parametros.UrlConsultarDocumento, parametros.IdAplicacion, autorizado.IdUsuario, autorizado.Token, documento));
            Console.WriteLine("Datos Basicos obtenidos ");
            return r;
                    
        }

        public List<DatosDetallados> ConsultarHechos(string IdPersona, string fuente)
        {
            var r = ObtenerRespuesta<List<DatosDetallados>>("{0}/{1},{2},{3},{4},{5}"
                .Fmt(parametros.UrlConsultarHechos, parametros.IdAplicacion, autorizado.IdUsuario, autorizado.Token, IdPersona, fuente));

            Console.WriteLine("hechos obtenidos ");
            return r;
        }

        public List<DatosDetallados> ConsultarHechos(DatosBasicos datoBasico)
        {
            return ConsultarHechos(datoBasico.ID_PERSONA, datoBasico.FUENTE);
        }




        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        public string Token
        {
            get
            {
                return autorizado != null ? autorizado.Token : string.Empty;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    CerrarConexionVivanto();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        private void CerrarConexionVivanto()
        {
            if (cliente != null)
            {
                using (cliente)
                {
                    if (autorizado != null)
                    {
                        try {
                            using (var httpResponse = cliente.Get<HttpWebResponse>("{0}/{1},{2},{3}".Fmt(parametros.UrlLogout, parametros.IdAplicacion, autorizado.IdUsuario, autorizado.Token)))
                            {
                                var responseStream = httpResponse.GetResponseStream();
                                var r2 = responseStream.ToUtf8String();
                                Log.Sesion(DirInfoLog, r2);
                            }
                        }
                        catch(Exception ex)
                        {
                            Log.Sesion(DirInfoLog, ex.Message);
                        }
                        autorizado = null;
                    }
                }
                cliente = null;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~ConexionVivanto() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }


        #endregion


        private static string HttpGet(string URI)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(URI);
            //req.Proxy = new System.Net.WebProxy(ProxyString, true); //true means no proxy
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            return sr.ReadToEnd().Trim();
        }


        private static string GetHttpResponse(string uri)
        {
            System.Threading.Thread.Sleep(500);
            using (HttpClient client = new HttpClient())
            {
                
                var t = client.GetStringAsync(uri);
                t.Wait();
                return t.Result;

            }

                
        }


    }
}

