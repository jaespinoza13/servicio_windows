namespace Domain.Models
{
    public class Parametro
    {
        public string nombre { get; set; } = string.Empty;
        public string nemonico { get; set; } = string.Empty;
        public string valorIni { get; set; } = string.Empty;
        public string? valorFin { get; set; }
        public string descripcion { get; set; } = string.Empty;
    }
}
