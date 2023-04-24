using Domain.Models;
using System.Reflection;
using Application.WsSitemas;
using Application.Common.Models;
using Application.Common.Interfaces;

namespace Application.Servicios.Common.Sistemas.WsSistemas
{
    public class WsSistemas : IWsSistemas
    {
        private readonly ILogs _logs;
        private readonly ISistemasApi _sistemasApi;

        public WsSistemas(ISistemasApi sistemas, ILogs logs)
        {
            _logs = logs;
            _sistemasApi = sistemas;
        }
        
        public async Task<ResGetParametro> GetParametros(ReqGetParametro reqGetParametro)
        {
            string operacion = "GET_PARAMETROS";
            var respuesta = new ResGetParametro();
            var res_tran = new RespuestaTransaccion();

            try
            {
                await _logs.SaveHeaderLogs(reqGetParametro, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

                res_tran = await _sistemasApi.GetParametros(reqGetParametro);
                respuesta.lst_parametro = (List<Parametro>?)res_tran.obj_cuerpo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _logs.SaveExecptionLogs(res_tran, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, ex);
            }
            await _logs.SaveResponseLogs(res_tran, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            return respuesta;
        }

        public async Task<ResGetTiemposEjecucion> GetTiemposEjecucion(ReqGetTiemposEjecucion req_get_tiempos_ejecucion)
        {
            string operacion = "GET_TIEMPOS_DE_EJECUCION";

            var response = new ResGetTiemposEjecucion();
            var res_horario = new RespuestaTransaccion();
            var res_frecuencia = new RespuestaTransaccion();

            ReqGetParametro req_param_horario = new ReqGetParametro();
            ReqGetParametro req_param_frecuencia = new ReqGetParametro();

            try
            {
                await _logs.SaveHeaderLogs(req_get_tiempos_ejecucion, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

                req_param_horario.int_sistema = req_get_tiempos_ejecucion.int_id_sistema;
                req_param_horario.str_nemonico = req_get_tiempos_ejecucion.str_nemo_horario;
                res_horario = await _sistemasApi.GetParametros(req_param_horario);
                var horario = (List<Parametro>?)res_horario.obj_cuerpo;

                if (horario!.Count > 0)
                {
                    response.tsp_hora_inicio = TimeSpan.Parse(horario[0].valorIni.ToString());
                    response.tsp_hora_fin = TimeSpan.Parse(horario[0].valorFin!.ToString());
                }
                else
                {
                    response.tsp_hora_inicio = TimeSpan.Parse("00:00:00"); //Tiempos por defecto
                    response.tsp_hora_fin = TimeSpan.Parse("23:59:00");
                }

                req_param_frecuencia.int_sistema = req_get_tiempos_ejecucion.int_id_sistema;
                req_param_frecuencia.str_nemonico = req_get_tiempos_ejecucion.str_nemo_frecuencia_ejecucion;
                res_frecuencia = await _sistemasApi.GetParametros(req_param_frecuencia);
                var frecuencia = (List<Parametro>?)res_frecuencia.obj_cuerpo;

                if (frecuencia!.Count > 0)
                {
                    response.int_frecuencia_ejecucion = Convert.ToInt32(frecuencia[0].valorIni.ToString());
                }
                else
                {
                    response.int_frecuencia_ejecucion = Convert.ToInt32(2); //Frecuencia por defecto
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _logs.SaveExecptionLogs(response, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, ex);
            }
            await _logs.SaveResponseLogs(response, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            return response;
        }
    }
}
