using Application.Common.Models;

namespace Application.Servicios.Common.WsIdentity
{
    public class ReqGenerateToken : Header
    {
        public string? str_documento { get; set; } = String.Empty;
        public string? str_identificacion_ordenante { get; set; } = String.Empty;
        public string? str_tipo_documento { get; set; } = String.Empty;
        public string? str_tipo_socilitud { get; set; } = String.Empty;
        public string? str_tipo_transferencia { get; set; } = String.Empty;
    }
}
