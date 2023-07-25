using System.Text;
using System.Text.Json;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Infraestructure.Common.Interfaces;
using System.Collections.ObjectModel;

namespace Infraestructure.Services
{
    public class HttpService : IHttpService
    {
        private readonly Configuracion _config;
        private readonly Dictionary<string, object>  _logs;
        private const string strRutaLog = "winGeneral/";

        public HttpService(IOptionsMonitor<Configuracion> option)
        {
            _config = option.CurrentValue;
            _logs = new Dictionary<string, object>();
            _logs.Add("str_base", _config.db_mongo);
            _logs.Add("str_collection", "respuestas");
            _logs.Add("tipo_log", "respuestas");
            _logs.Add(_config.typeAuthAccesoLogs, _config.auth_acceso_logs);
        }

        public async Task<object> solicitar_servicio(SolicitarServicio solicitarServicio)
        {
            var peticion = solicitarServicio.objSolicitud;

            try
            {
                var client = new HttpClient();
                var request = createRequest(solicitarServicio);

                addHeaders(solicitarServicio, client);
                client.Timeout = TimeSpan.FromMinutes(10);
                var response = client.SendAsync(request).Result;

                saveResponseHttp(solicitarServicio.idTransaccion, response);

                if (Convert.ToInt32(response.StatusCode) == 200)
                    return await response.Content.ReadAsStringAsync();
                else
                    throw new ArgumentException(response.StatusCode.ToString());
            }
            catch (TaskCanceledException ex)
            {
                saveErrorHttp(solicitarServicio, peticion, ex);
                throw new ArgumentException(ex.Message);
            }
        }

        public object solicitar_servicio_async(SolicitarServicio solicitarServicio)
        {
            var respuesta = new object { };

            try
            {
                var client = new HttpClient();
                var request = createRequest(solicitarServicio);

                addHeadersLogs(solicitarServicio, client);

                client.SendAsync(request);                
            }
            catch (TaskCanceledException ex) {
                throw new ArgumentNullException(ex.Message);
            }
            return respuesta;
        }

        private static HttpRequestMessage createRequest(SolicitarServicio solicitarServicio)
        {
            string str_solicitud = JsonSerializer.Serialize(solicitarServicio.objSolicitud);

            var request = new HttpRequestMessage();
            if (solicitarServicio.contentType == "application/x-www-form-urlencoded")
            {
                var parametros = JsonSerializer.Deserialize<Dictionary<string, string>>(str_solicitud)!;
                request.Content = new FormUrlEncodedContent(parametros);
            }
            else
                request.Content = new StringContent(str_solicitud, Encoding.UTF8, solicitarServicio.contentType);

            request.Method = new HttpMethod(solicitarServicio.tipoMetodo);
            request.RequestUri = new Uri(solicitarServicio.urlServicio, System.UriKind.RelativeOrAbsolute);
            request.Content.Headers.Add("No-Paging", "true");
            return request;
        }

        private static void addHeadersLogs(SolicitarServicio solicitarServicio, HttpClient httpClient)
        {
            if (solicitarServicio.authBasic != null)
                httpClient.DefaultRequestHeaders.Add(solicitarServicio.tipoAuth, solicitarServicio.authBasic);

            foreach (var header in solicitarServicio.dcyHeadersAdicionales)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
            }
        }

        private void addHeaders(SolicitarServicio solicitarServicio, HttpClient httpClient)
        {
            if (solicitarServicio.authBasic != null)
            {
                switch (solicitarServicio.nombreServicio)
                {
                    case "wsSistemas":
                        httpClient.DefaultRequestHeaders.Add(_config.sistemas_type_auth, _config.sistemas_auth);
                        break;
                    case "wsIdentity":
                        httpClient.DefaultRequestHeaders.Add(_config.wsIdentity_type_auth, _config.wsIdentity_auth);
                        break;

                    // Agregar nuevos servicios -- Hace referencia al nombre de cada servicio del appsettings
                    case "wsTransferencias":
                        httpClient.DefaultRequestHeaders.Add(_config.wsTransferencias_type_auth, _config.wsTransferencias_auth);
                        break;
                    case "wsProcesarSms":
                        httpClient.DefaultRequestHeaders.Add(_config.wsProcesarSms_type_auth, _config.wsProcesarSms_auth);
                        break;
                }
            }
            foreach (var header in solicitarServicio.dcyHeadersAdicionales)
            {
                httpClient.DefaultRequestHeaders.Add(header.Key, header.Value.ToString());
            }
        }

        private void saveResponseHttp(string str_id_transaccion, HttpResponseMessage response)
        {
            SolicitarServicio solicitarServicio = new();
            solicitarServicio.urlServicio = _config.url_acceso_logs + strRutaLog;
            solicitarServicio.dcyHeadersAdicionales = _logs;

            string str_result = response.Content.ReadAsStringAsync().Result;
            var resultado = new object();
            if (str_result == "")
                resultado = new { status = "OK" };
            else
                resultado = response.Content.ReadAsStringAsync().Result;


            solicitarServicio.objSolicitud = new
            {
                respuesta = resultado,
                str_id_transaccion = str_id_transaccion
            };

            solicitar_servicio_async(solicitarServicio);
        }

        private void saveErrorHttp(SolicitarServicio solicitarServicio, object objPeticion, Exception ex)
        {
            solicitarServicio.objSolicitud = new { objPeticion, error = ex.Message };
            solicitarServicio.urlServicio = _config.url_acceso_logs + strRutaLog;
            solicitarServicio.dcyHeadersAdicionales = _logs;
            solicitarServicio.dcyHeadersAdicionales["str_collection"] = _config.errores_http;
            solicitarServicio.dcyHeadersAdicionales["tipo_log"] = _config.errores_http;
            solicitar_servicio_async(solicitarServicio);
        }
    }
}
