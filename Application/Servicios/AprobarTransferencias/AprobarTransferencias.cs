using System.Reflection;
using Application.WsSitemas;
using Application.WsSistemasDat;
using Application.Common.Models;
using Application.Common.Funciones;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Servicios;
using Application.Servicios.Common.WsIdentity;

namespace Application.Servicios.AprobarTransferencias
{
    public class AprobarTransferencias : IAprobarTransferencias
    {
        private readonly ILogs _logs;
        private readonly ITransferenciasApi _transf;
        private readonly IMegSistemas sisDat;
        private readonly IWsIdentity _wsIdentity;
        private readonly IWsSistemas _wsSistemas;
        private static TimeSpan hora_actual = DateTime.Now.TimeOfDay;

        public AprobarTransferencias(ITransferenciasApi transf, ILogs logs, IMegSistemas sISDat, IWsIdentity wsIdentity, IWsSistemas wsSistemas)
        {
            _logs = logs;
            _transf = transf;
            sisDat = sISDat;
            _wsIdentity = wsIdentity;
            _wsSistemas = wsSistemas;
        }

        public async Task<ResAprobarTransf> AprobTransferencias(ReqAprobarTransf req_aprobar_transf)
        {
            string operacion = "APROBAR_TRANSFERENCIAS";
            var respuesta = new ResAprobarTransf();
            var res_tran = new RespuestaTransaccion();
            ReqGetIdSistema req_get_id_sistema = new ReqGetIdSistema();
            ReqGetTiemposEjecucion req_tiempos_ejecucion = new ReqGetTiemposEjecucion();
            bool _ejecutando = true;

            try
            {
                await _logs.SaveHeaderLogs(req_aprobar_transf, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

                // Consultar id del sistema para los parametros
                string sistema_id;
                string str_token = "";
                req_get_id_sistema.str_nombre_sistema = "WSPROCESARSMS";
                var res_sistema = await sisDat.GetIdSistema(req_get_id_sistema);
                if (res_sistema.str_res_codigo == "000") sistema_id = res_sistema.str_id_sistema;
                else sistema_id = "4"; // Soporte

                // Generar el token para wsTransferencias
                Header header = Funciones.ConstruirHeader(new Header
                {
                    str_id_servicio = "APROBAR_TRANSFERENCIAS",
                    str_id_msj = "Aprobar transferencias que esten en estado REVISADO",
                    str_id_sistema = sistema_id,
                    str_mac_dispositivo = req_aprobar_transf.str_mac,
                    str_ip_dispositivo = req_aprobar_transf.str_ip,
                    str_login = "USR_SMS"
                });

                // Consulta los parametros para determinar el horario y frecuencia de ejecución
                req_tiempos_ejecucion.int_id_sistema = Convert.ToInt32(sistema_id);
                req_tiempos_ejecucion.str_nemo_horario = req_aprobar_transf.str_horario_ejecucion;
                req_tiempos_ejecucion.str_nemo_frecuencia_ejecucion = req_aprobar_transf.str_frecuencia_ejecucion;

                ResGetTiemposEjecucion response = await _wsSistemas.GetTiemposEjecucion(req_tiempos_ejecucion);

                while (_ejecutando)
                {
                    if (hora_actual >= response.tsp_hora_inicio && hora_actual <= response.tsp_hora_fin)
                    {
                        Console.WriteLine($"Buscando transferencias por aprobar...");

                        ReqGenerateToken rgt = Funciones.ConstruirReqGenerarToken(header);
                        var res_token = await _wsIdentity.GenerateToken(rgt);

                        if (res_token.str_res_codigo == "000") str_token = res_token.str_token;

                        ReqAprobTransfApi req = new ReqAprobTransfApi
                        {
                            consulta = header,
                            str_token = str_token
                        };
                        res_tran = await _transf.AprobarTransferencias(req);
                        respuesta = (ResAprobarTransf?)res_tran.obj_cuerpo;

                        if (respuesta != null)
                        {
                            if (respuesta.transf_procesada != null && respuesta.transf_procesada!.Count > 0)
                            {
                                foreach (var item in respuesta.transf_procesada!)
                                {
                                    Console.WriteLine("CODIGO: " + item.codigo + " MENSAJE: " + item.mensaje);
                                }
                            }
                        }
                    }
                    Thread.Sleep(response.int_frecuencia_ejecucion * 60000);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("CODIGO: " + 500 + " MENSAJE: " + ex.Message);
                Console.ForegroundColor = ConsoleColor.Red;

                Console.WriteLine($"Detalle error: {ex}", Console.ForegroundColor);
                await _logs.SaveExecptionLogs(res_tran, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, ex);
            }
            await _logs.SaveResponseLogs(res_tran, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            return respuesta!;
        }
    }
}
