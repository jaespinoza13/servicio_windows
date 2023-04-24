using Application.Common.Models;

namespace Application.Servicios.AprobarTransferencias
{
    public class ReqAprobarTransf
    {
        public string str_mac { get; set; } = String.Empty;
        public string str_ip { get; set; } = String.Empty;
        public string str_horario_ejecucion { get; set; } = String.Empty;
        public string str_frecuencia_ejecucion { get; set; } = String.Empty;
    }
}
