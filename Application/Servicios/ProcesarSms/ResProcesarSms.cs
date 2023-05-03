using Application.Common.Models;

namespace Application.Servicios.ProcesarSms
{
    public class ResProcesarSms : ResComun
    {
        public List<RespProceso>? sms_procesados { get; set; }
    }
}
