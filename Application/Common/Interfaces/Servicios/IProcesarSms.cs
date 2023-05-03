using Application.Common.Models;
using Application.Servicios.ProcesarSms;

namespace Application.Common.Interfaces.Servicios
{
    public interface IProcesarSms
    {
        Task<ResProcesarSms> ProcesSms(ReqProcesarServicio req_procesar_sms);
    }
}
