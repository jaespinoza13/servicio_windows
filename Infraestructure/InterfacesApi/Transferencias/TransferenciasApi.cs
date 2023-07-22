using Newtonsoft.Json;
using System.Reflection;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Application.Common.Interfaces;
using Infraestructure.Common.Interfaces;
using Application.Servicios.AprobarTransferencias;

namespace Infraestructure.InterfacesApi.Transferencias
{
    public class TransferenciasApi : ITransferenciasApi
    {
        private readonly IHttpService _httpService;
        private readonly Configuracion _config;
        private readonly ILogs _logs;
        private readonly string _clase;
        private static SolicitarServicio _solicitarServicio = new SolicitarServicio();

        public TransferenciasApi(IHttpService httpService, IOptionsMonitor<Configuracion> config, ILogs logs)
        {
            _httpService = httpService;
            _logs = logs;
            _clase = GetType().Name;
            _config = config.CurrentValue;
            _solicitarServicio.tipoAuth = _config.wsTransferencias_type_auth;
            _solicitarServicio.authBasic = _config.wsTransferencias_auth;
            _solicitarServicio.nombreServicio = _config.wsTransferencias_nombre;
            _solicitarServicio.dcyHeadersAdicionales = new Dictionary<string, object>();
        }

        public async Task<RespuestaTransaccion> AprobarTransferencias(ReqAprobTransfApi req_aprobar_transf)
        {
            string str_operacion = "LLAMAR_SERVICIO_WS_TRANSFERENCIAS";
            await _logs.SaveHeaderLogs(req_aprobar_transf, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
            var respuesta = new RespuestaTransaccion();
            try
            {
                if (req_aprobar_transf.consulta != null) //JESPINOZA
                {
                     _solicitarServicio.objSolicitud = req_aprobar_transf.consulta;
                     _solicitarServicio.tipoMetodo = "POST";
                     _solicitarServicio.urlServicio = $"{_config.wsTransferencias_recurso}APROBAR_TRANSFERENCIAS";
                     _solicitarServicio.dcyHeadersAdicionales = new Dictionary<string, object>
                     {
                        { "Authorization", $"Bearer {req_aprobar_transf.str_token}" },
                        { "int_estado", -1 } // Se envia el token y un estado con valor -1 para la sesion de wsTransferencias
                     };

                     var str_res_servicio = await _httpService.solicitar_servicio(_solicitarServicio);
                     var response = JsonConvert.DeserializeObject<ResAprobarTransf>(str_res_servicio.ToString()!)!;
                     respuesta.obj_cuerpo = response;

                }
                else
                {
                    return respuesta;
                }
                
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
