namespace Application.WsSitemas
{
    public class ReqGetTiemposEjecucion
    {
        public int int_id_sistema { get; set; }
        public string str_nemo_horario { get; set; } = String.Empty;
        public string str_nemo_frecuencia_ejecucion { get; set; } = String.Empty;
    }
}
