using Domain.Models;
using Newtonsoft.Json;
using System.Reflection;
using Application.WsSitemas;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Application.Common.Interfaces;
using Infraestructure.Common.Interfaces;

namespace Infraestructure.InterfacesApi.Common
{
    public class SistemasApi : ISistemasApi
    {
        private readonly IHttpService _httpService;
        private readonly Configuracion _config;
        private readonly ILogs _logs;
        private readonly string _clase;
        private static SolicitarServicio _solicitarServicio = new SolicitarServicio();

        public SistemasApi(IHttpService httpService, IOptionsMonitor<Configuracion> config, ILogs logs)
        {
            _httpService = httpService;
            _logs = logs;
            _clase = GetType().Name;
            _config = config.CurrentValue;
            _solicitarServicio.tipoAuth = _config.sistemas_type_auth;
            _solicitarServicio.authBasic = _config.sistemas_auth;
            _solicitarServicio.nombreServicio = _config.sistemas_nombre;
            _solicitarServicio.dcyHeadersAdicionales = new Dictionary<string, object>();
        }

        public async Task<RespuestaTransaccion> GetParametros(ReqGetParametro reqGetParametro)
        {
            string str_operacion = "LLAMAR_SERVICIO_WS_SISTEMAS_REST";
            await _logs.SaveHeaderLogs(reqGetParametro, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
            var respuesta = new RespuestaTransaccion();
            try
            {
                _solicitarServicio.tipoMetodo = "GET";
                _solicitarServicio.urlServicio = $"{_config.sistemas_url}parametros/parametros_nemo?id_sistema={reqGetParametro.int_sistema}&nemonico={reqGetParametro.str_nemonico}";
                _solicitarServicio.dcyHeadersAdicionales = new();

                var str_res_servicio = await _httpService.solicitar_servicio(_solicitarServicio);
                List<Parametro> response = JsonConvert.DeserializeObject<List<Parametro>>(str_res_servicio.ToString()!)!;

                respuesta.obj_cuerpo = response;
            }
            catch (TaskCanceledException ex)
            {
                await _logs.SaveExecptionLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex);
                throw new Exception(ex.Message);
            }
            await _logs.SaveResponseLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
            return respuesta;
        }
    }
}
