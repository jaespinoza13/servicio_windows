using Newtonsoft.Json;
using System.Reflection;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Application.Common.Interfaces;
using Infraestructure.Common.Interfaces;
using Application.Servicios.ProcesarSms;

namespace Infraestructure.InterfacesApi.Sms
{
    public class ProcesarSmsApi : IProcesarSmsApi
    {
        private readonly IHttpService _httpService;
        private readonly Configuracion _config;
        private readonly ILogs _logs;
        private readonly string _clase;
        private static SolicitarServicio _solicitarServicio = new SolicitarServicio();

        public ProcesarSmsApi(IHttpService httpService, IOptionsMonitor<Configuracion> config, ILogs logs)
        {
            _httpService = httpService;
            _logs = logs;
            _clase = GetType().Name;
            _config = config.CurrentValue;
            _solicitarServicio.tipoAuth = _config.wsProcesarSms_type_auth;
            _solicitarServicio.authBasic = _config.wsProcesarSms_auth;
            _solicitarServicio.nombreServicio = _config.wsProcesarSms_nombre;
            _solicitarServicio.dcyHeadersAdicionales = new Dictionary<string, object>();
        }

        public async Task<RespuestaTransaccion> ProcesSms(ReqProcesarSmsApi req_procesar_sms_api)
        {
            var request = req_procesar_sms_api.header;
            string str_operacion = "LLAMAR_SERVICIO_WS_PROCESAR_SMS";
            var respuesta = new RespuestaTransaccion();
            if (request != null)
            {
                await _logs.SaveHeaderLogs(request, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
                
                try
                {
                    _solicitarServicio.tipoMetodo = "POST";
                    _solicitarServicio.urlServicio = $"{_config.wsProcesarSms_recurso}PROCESAR_SMS";
                    _solicitarServicio.objSolicitud = request;

                    var str_res_servicio = await _httpService.solicitar_servicio(_solicitarServicio);
                    var response = JsonConvert.DeserializeObject<ResProcesarSms>(str_res_servicio.ToString()!)!;
                    respuesta.obj_cuerpo = response;
                }
                catch (TaskCanceledException ex)
                {
                    await _logs.SaveExecptionLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex);
                    throw new Exception(ex.Message);
                }
                await _logs.SaveResponseLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
                return respuesta;
            }else
            {

                return respuesta;
            }
            
        }
    }
}
