namespace Application.Common.Models
{
    public class SolicitarServicio
    {
        public string authBasic { get; set; } = null!;
        public string tipoAuth { get; set; } = "Authorization-Mego";
        public string contentType { get; set; } = "application/json";
        public object objSolicitud { get; set; } = new object();
        public Dictionary<string, object> dcyHeadersAdicionales { get; set; } = new Dictionary<string, object>();
        public string urlServicio { get; set; } = string.Empty;
        public string idTransaccion { get; set; } = string.Empty;
        public string tipoMetodo { get; set; } = "POST";
        public string nombreServicio { get; set; } = string.Empty;
    }
}
