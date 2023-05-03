using System.Reflection;
using Application.Common.Models;
using Application.Common.Funciones;
using Application.Common.Interfaces;
using Application.Common.Interfaces.Servicios;

namespace Application.Servicios.ProcesarSms
{
    public class ProcesarSms : IProcesarSms
    {
        private readonly ILogs _logs;
        private readonly IProcesarSmsApi _sms;
        private static TimeSpan hora_actual = DateTime.Now.TimeOfDay;

        public ProcesarSms(IProcesarSmsApi sms, ILogs logs)
        {
            _logs = logs;
            _sms = sms;
        }

        public async Task<ResProcesarSms> ProcesSms(ReqProcesarServicio req_procesar_sms)
        {
            string operacion = "PROCESAR_SMS";
            var respuesta = new ResProcesarSms();
            var res_tran = new RespuestaTransaccion();
            bool _ejecutando = true;

            try
            {
                await _logs.SaveHeaderLogs(req_procesar_sms, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

                Header header = Funciones.ConstruirHeader(new Header
                {
                    str_id_servicio = "PROCESAR_SMS",
                    str_id_msj = "Rechazar tranferencias y bloquear cuentas contra retiro mediante SMS",
                    str_id_sistema = req_procesar_sms.int_sistema.ToString(),
                    str_mac_dispositivo = req_procesar_sms.str_mac,
                    str_ip_dispositivo = req_procesar_sms.str_ip,
                    str_login = "USR_SMS"
                });

                while (_ejecutando)
                {
                    if (hora_actual >= req_procesar_sms.tsp_hora_inicio && hora_actual <= req_procesar_sms.tsp_hora_fin)
                    {
                        Console.WriteLine($"Buscando sms por procesar...");

                        ReqProcesarSmsApi req = new ReqProcesarSmsApi();
                        req.header = header;

                        res_tran = await _sms.ProcesSms(req);
                        respuesta = (ResProcesarSms?)res_tran.obj_cuerpo;

                        if (respuesta != null)
                        {
                            if (respuesta.sms_procesados != null && respuesta.sms_procesados!.Count > 0)
                            {
                                foreach (var item in respuesta.sms_procesados!)
                                {
                                    Console.WriteLine("CODIGO: " + item.codigo + " MENSAJE: " + item.mensaje);
                                }
                            }
                        }
                    }
                    Thread.Sleep(req_procesar_sms.int_frecuencia_ejecucion * 60000);
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
