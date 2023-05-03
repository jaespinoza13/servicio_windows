using Application.WsSitemas;

namespace Application.Common.Models
{
    public class ReqProcesarServicio : ResGetTiemposEjecucion
    {
        public string str_mac { get; set; } = String.Empty;
        public string str_ip { get; set; } = String.Empty;
        public int int_sistema { get; set; }
    }
}
