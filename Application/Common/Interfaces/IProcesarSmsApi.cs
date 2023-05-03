using Application.Common.Models;
using Application.Servicios.ProcesarSms;

namespace Application.Common.Interfaces
{
    public interface IProcesarSmsApi
    {
        Task<RespuestaTransaccion> ProcesSms(ReqProcesarSmsApi req_procesar_sms_api);
    }
}
