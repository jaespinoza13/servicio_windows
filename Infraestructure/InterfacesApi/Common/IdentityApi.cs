using Application.Common.Interfaces;
using Application.Common.Models;
using Application.Servicios.AprobarTransferencias;
using Application.Servicios.Common.WsIdentity;
using Domain.Models;
using Infraestructure.Common.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Reflection;

namespace Infraestructure.InterfacesApi.Common
{
    public class IdentityApi : IIdentityApi
    {
        private readonly IHttpService _httpService;
        private readonly Configuracion _config;
        private readonly ILogs _logs;
        private readonly string _clase;
        private static SolicitarServicio _solicitarServicio = new SolicitarServicio();

        public IdentityApi(IHttpService httpService, IOptionsMonitor<Configuracion> config, ILogs logs)
        {
            _httpService = httpService;
            _logs = logs;
            _clase = GetType().Name;
            _config = config.CurrentValue;
            _solicitarServicio.tipoAuth = _config.wsIdentity_type_auth;
            _solicitarServicio.authBasic = _config.wsIdentity_auth;
            _solicitarServicio.nombreServicio = _config.wsIdentity_nombre;
            _solicitarServicio.dcyHeadersAdicionales = new Dictionary<string, object>();
        }

        public async Task<RespuestaTransaccion> GenerateToken(ReqGenerateToken req_generate_token)
        {
            string str_operacion = "LLAMAR_SERVICIO_WS_IDENTITY";
            await _logs.SaveHeaderLogs(req_generate_token, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
            var respuesta = new RespuestaTransaccion();
            try
            {
                _solicitarServicio.tipoMetodo = "POST";
                _solicitarServicio.urlServicio = $"{_config.wsIdentity_url}AUTENTICARSE_INVITADO_INT";
                _solicitarServicio.objSolicitud = req_generate_token;
                _solicitarServicio.dcyHeadersAdicionales = new();

                var str_res_servicio = await _httpService.solicitar_servicio(_solicitarServicio);
                var response = JsonConvert.DeserializeObject<ResGenerateToken>(str_res_servicio.ToString()!)!;

                respuesta.obj_cuerpo = response;
            }
            catch (TaskCanceledException ex)
            {
                await _logs.SaveExecptionLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex);
                throw new ArgumentNullException(ex.Message);
            }
            await _logs.SaveResponseLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
            return respuesta;
        }
    }
}
