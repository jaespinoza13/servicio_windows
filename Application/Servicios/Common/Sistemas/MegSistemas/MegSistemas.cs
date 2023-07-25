using System.Reflection;
using Application.Interfaz;
using Application.Common.Models;
using Application.WsSistemasDat;
using Application.Common.Interfaces;

namespace Application.Servicios.Common.Sistemas.MegSistemas
{
    public class MegSistemas : IMegSistemas
    {
        private readonly ILogs _logs;
        private readonly ISistemasDat _sistemas;
        private readonly string _clase;

        public MegSistemas(ISistemasDat sistemas, ILogs logs)
        {
            _logs = logs;
            _sistemas = sistemas;
            _clase = GetType().Name;
        }

        public async Task<ResGetIdSistema> GetIdSistema(ReqGetIdSistema req_get_id_sistema)
        {
            string str_operacion = "GET_ID_SISTEMA";
            var response = new ResGetIdSistema();
            try
            {
                await _logs.SaveHeaderLogs(req_get_id_sistema, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
                RespuestaTransaccion res = await _sistemas.GetIdSistema(req_get_id_sistema.str_nombre_sistema);
                response.str_res_estado_transaccion = (res.str_codigo.Equals("000")) ? "OK" : "ERR";
                response.str_res_codigo = res.str_codigo;
                response.str_id_sistema = res.dcc_variables["int_id_sistema"].ToString()!;
                await _logs.SaveResponseLogs(response, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase);
                return response;
            }
            catch (TaskCanceledException ex)
            {
                await _logs.SaveExecptionLogs(response, str_operacion, MethodBase.GetCurrentMethod()!.Name, _clase, ex);
                throw new ArgumentNullException(ex.Message);
            }
        }
    }
}
