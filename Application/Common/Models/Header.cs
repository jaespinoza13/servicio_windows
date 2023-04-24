namespace Application.Common.Models
{
    public class Header
    {
        /// <summary>
        /// Id de log
        /// </summary>
        public string str_id_transaccion { get; set; } = String.Empty;
        /// <summary>
        /// Ente
        /// </summary>
        public string str_ente { get; set; } = String.Empty;
        /// <summary>
        /// Nemonico del canal Ejm: CANBEEBOT
        /// </summary>
        public string str_nemonico_canal { get; set; } = String.Empty;
        /// <summary>
        /// Id del sistema Ejm: 74
        /// </summary>
        /// 
        public string str_id_sistema { get; set; } = String.Empty;
        /// <summary>
        /// Nombre de la app Ejm: MEGONLINE
        /// </summary>
        /// 
        public string str_app { get; set; } = String.Empty;
        /// <summary>
        /// Id del servicio web Ejm: REQ_VALIDAR_USUARIO
        /// </summary>
        /// 
        public string str_id_servicio { get; set; } = String.Empty;
        /// <summary>
        /// Versión del servicio web Ejm: 1.0
        /// </summary>
        /// 
        public string str_version_servicio { get; set; } = String.Empty;
        /// <summary>
        /// Id del usuario para ley protección de datos.
        /// </summary>
        /// 
        public string str_id_usuario { get; set; } = String.Empty;
        /// <summary>
        /// Dirección física
        /// </summary>
        /// 
        public string str_mac_dispositivo { get; set; } = String.Empty;
        /// <summary>
        /// Dirección Ip
        /// </summary>
        /// 
        public string str_ip_dispositivo { get; set; } = String.Empty;
        /// <summary>
        /// Remitente Ejm: RED_SOCIAL_FACEBOOK
        /// </summary>
        /// 
        public string str_remitente { get; set; } = String.Empty;
        /// <summary>
        /// Receptor Ejm: COOPMEGO
        /// </summary>
        /// 
        public string str_receptor { get; set; } = String.Empty;
        /// <summary>
        /// Tipo de petición REQ o RES
        /// </summary>
        /// 
        public string str_tipo_peticion { get; set; } = String.Empty;
        /// <summary>
        /// Id mensaje formato yyMMddHHmmssffff
        /// </summary>
        public string str_id_msj { get; set; } = String.Empty;
        /// <summary>
        /// Fecha formato yyyy-MM-dd HH:mm:ss
        /// </summary>
        public DateTime dt_fecha_operacion { get; set; } = DateTime.Now;
        /// <summary>
        /// Posible duplicado false o true
        /// </summary>
        public bool bl_posible_duplicado { get; set; } = false;
        /// <summary>
        /// Prioridad
        /// </summary>
        public string str_prioridad { get; set; } = String.Empty;
        /// <summary>
        /// Login de usuario
        /// </summary>
        /// 
        public string str_login { get; set; } = String.Empty;
        /// <summary>
        /// Latitud
        /// </summary>
        /// 
        public string str_latitud { get; set; } = String.Empty;
        /// <summary>
        /// Longitud
        /// </summary>
        /// 
        public string str_longitud { get; set; } = String.Empty;
        /// <summary>
        /// Firma digital
        /// </summary>
        public string str_firma_digital { get; set; } = String.Empty;
        /// <summary>
        /// Num sim
        /// </summary>
        public string str_num_sim { get; set; } = String.Empty;
        /// <summary>
        /// Clave secreta
        /// </summary>
        public string str_clave_secreta { get; set; } = String.Empty;
        /// <summary>
        /// País
        /// </summary>
        /// 
        public string str_pais { get; set; } = String.Empty;
        /// <summary>
        /// Sesión
        /// </summary>
        /// 
        public string str_sesion { get; set; } = String.Empty;
        /// <summary>
        /// Id de Oficina
        /// </summary>
        /// 
        public string str_id_oficina { get; set; } = String.Empty;
        /// <summary>
        /// Id de Perfil
        /// </summary>
        public string str_id_perfil { get; set; } = String.Empty;
    }
}
