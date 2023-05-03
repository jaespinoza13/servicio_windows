using System.Reflection;
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
        private readonly IWsIdentity _wsIdentity;
        private static TimeSpan hora_actual = DateTime.Now.TimeOfDay;

        public AprobarTransferencias(ITransferenciasApi transf, ILogs logs, IWsIdentity wsIdentity)
        {
            _logs = logs;
            _transf = transf;
            _wsIdentity = wsIdentity;
        }

        public async Task<ResAprobarTransf> AprobTransferencias(ReqProcesarServicio req_aprobar_transf)
        {
            string operacion = "APROBAR_TRANSFERENCIAS";
            var respuesta = new ResAprobarTransf();
            var res_tran = new RespuestaTransaccion();
            bool _ejecutando = true;

            try
            {
                await _logs.SaveHeaderLogs(req_aprobar_transf, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
                string str_token = "";
               
                Header header = Funciones.ConstruirHeader(new Header
                {
                    str_id_servicio = "APROBAR_TRANSFERENCIAS",
                    str_id_msj = "Aprobar transferencias que esten en estado REVISADO",
                    str_id_sistema = req_aprobar_transf.int_sistema.ToString(),
                    str_mac_dispositivo = req_aprobar_transf.str_mac,
                    str_ip_dispositivo = req_aprobar_transf.str_ip,
                    str_login = "USR_SMS"
                });

                while (_ejecutando)
                {
                    if (hora_actual >= req_aprobar_transf.tsp_hora_inicio && hora_actual <= req_aprobar_transf.tsp_hora_fin)
                    {
                        Console.WriteLine($"Buscando transferencias por aprobar...");

                        // Generar el token para wsTransferencias
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
                    Thread.Sleep(req_aprobar_transf.int_frecuencia_ejecucion * 60000);
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
