namespace Application.Common.Models
{
    public class RespuestaTransaccion
    {
        public object obj_cuerpo { get; set; } = new object();
        public string str_codigo { get; set; } = String.Empty;
        public string str_mensaje { get; set; } = String.Empty;
        public string str_id_transaccion { get; set; } = String.Empty;
        public Dictionary<string, object> dcc_variables { get; set; } = new Dictionary<string, object>();
    }
}
