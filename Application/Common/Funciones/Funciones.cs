using Application.Common.Models;
using Application.Servicios.Common.WsIdentity;

namespace Application.Common.Funciones
{
    public static class Funciones
    {
        public static Header ConstruirHeader(dynamic header)
        {
            Header req = new Header();

            req.str_id_transaccion = "";
            req.str_id_servicio = header.str_id_servicio;
            req.str_version_servicio = "1.0.0";
            req.str_tipo_peticion = "REQ";
            req.str_id_msj = header.str_id_msj;
            req.bl_posible_duplicado = false;
            req.str_prioridad = "";
            req.str_ente = "0000";
            req.str_clave_secreta = "";
            req.str_nemonico_canal = "CANVEN";
            req.str_id_sistema = header.str_id_sistema;
            req.str_app = "SERV_WINDOWS";
            req.str_mac_dispositivo = header.str_mac_dispositivo;
            req.str_ip_dispositivo = header.str_ip_dispositivo;
            req.str_remitente = "CONSOLA";
            req.str_receptor = "COOPMEGO";
            req.dt_fecha_operacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
            req.str_login = header.str_login;
            req.str_latitud = "-0.2143";
            req.str_longitud = "-78.5017";
            req.str_pais = "Ecuador";
            req.str_sesion = "-1";
            req.str_id_oficina = "1";
            req.str_id_perfil = "1";
            req.str_id_usuario = "1";

            return req;
        }

        public static ReqGenerateToken ConstruirReqGenerarToken(Header header)
        {
            ReqGenerateToken rgt = new ReqGenerateToken();

            rgt.str_id_transaccion = header.str_id_transaccion;
            rgt.str_id_servicio = header.str_id_servicio;
            rgt.str_ente = header.str_ente;
            rgt.str_version_servicio = header.str_version_servicio;
            rgt.str_tipo_peticion = header.str_tipo_peticion;
            rgt.str_id_msj = header.str_id_msj;
            rgt.bl_posible_duplicado = header.bl_posible_duplicado;
            rgt.str_prioridad = header.str_prioridad;
            rgt.str_clave_secreta = header.str_clave_secreta;
            rgt.str_nemonico_canal = header.str_nemonico_canal;
            rgt.str_id_sistema = header.str_id_sistema;
            rgt.str_app = header.str_app;
            rgt.str_mac_dispositivo = header.str_mac_dispositivo;
            rgt.str_ip_dispositivo = header.str_ip_dispositivo;
            rgt.str_remitente = header.str_remitente;
            rgt.str_receptor = header.str_receptor;
            rgt.dt_fecha_operacion = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "yyyy-MM-dd HH:mm:ss", null);
            rgt.str_login = header.str_login;
            rgt.str_latitud = header.str_latitud;
            rgt.str_longitud = header.str_longitud;
            rgt.str_pais = header.str_pais;
            rgt.str_sesion = header.str_sesion;
            rgt.str_id_oficina = header.str_id_oficina;
            rgt.str_id_perfil = header.str_id_perfil;
            rgt.str_id_usuario = header.str_id_usuario;
            rgt.str_documento = "0000000000";
            rgt.str_identificacion_ordenante = "0000000000";
            rgt.str_tipo_documento = "CT";
            rgt.str_tipo_socilitud = "O";
            rgt.str_tipo_transferencia = "Transferencias";

            return rgt;
        }
    }
}
