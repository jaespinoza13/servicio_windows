using Application.Common.Models;

namespace Application.Servicios.AprobarTransferencias
{
    public class ReqAprobTransfApi
    {
        public Header? consulta { get; set; }
        public string str_token { get; set; } = String.Empty;
    }
}
