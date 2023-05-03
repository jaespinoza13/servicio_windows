namespace Application.Common.Models
{
    public class Configuracion
    {
        #region Settings
        public string Ruta_log { get; set; } = string.Empty;
        #endregion

        #region EndPoints
        public string url_acceso_logs { get; set; } = string.Empty;
        #endregion

        #region ServiciosGenerales
            #region wsSistemasRest
            public string sistemas_nombre { get; set; } = string.Empty;
            public string sistemas_url { get; set; } = string.Empty;
            public string sistemas_type_auth { get; set; } = string.Empty;
            public string sistemas_auth { get; set; } = string.Empty;
            public string sistemas_usuario { get; set; } = string.Empty;
            public string sistemas_password { get; set; } = string.Empty;
            public int sistemas_id { get; set; }
            #endregion
            #region wsIdentity
            public string wsIdentity_nombre { get; set; } = string.Empty;
            public string wsIdentity_url { get; set; } = string.Empty;
            public string wsIdentity_type_auth { get; set; } = string.Empty;
            public string wsIdentity_auth { get; set; } = string.Empty;
            #endregion
        #endregion

        #region ServiciosWin
            #region WsTransferencias
            public string wsTransferencias_nombre { get; set; } = string.Empty;
            public string wsTransferencias_recurso { get; set; } = string.Empty;
            public string wsTransferencias_type_auth { get; set; } = string.Empty;
            public string wsTransferencias_auth { get; set; } = string.Empty;
                #region AprobarTransferencias
                public string aprob_transf_nombre { get; set; } = string.Empty;
                public int aprob_transf_frecuencia_ejecucion { get; set; }
                public string aprob_transf_hora_inicio_ejecucion { get; set; } = string.Empty;
                public string aprob_transf_hora_fin_ejecucion { get; set; } = string.Empty;
                #endregion
            #endregion
            #region WsProcesarSms
            public string wsProcesarSms_nombre { get; set; } = string.Empty;
            public string wsProcesarSms_recurso { get; set; } = string.Empty;
            public string wsProcesarSms_type_auth { get; set; } = string.Empty;
            public string wsProcesarSms_auth { get; set; } = string.Empty;
                #region RechazarTransfBloquearCuenta
                public string rechaz_bloquear_nombre { get; set; } = string.Empty;
                public int rechaz_bloquear_frecuencia_ejecucion { get; set; }
                public string rechaz_bloquear_hora_inicio_ejecucion { get; set; } = string.Empty;
                public string rechaz_bloquear_hora_fin_ejecucion { get; set; } = string.Empty;
                #endregion
            #endregion
        #endregion

        #region Authorizations
        public string auth_acceso_logs { get; set; } = string.Empty;
        #endregion

        #region TypesAuthorization
        public string typeAuthAccesoLogs { get; set; } = string.Empty;
        #endregion

        #region GrpcSettings
        public string client_grpc_sybase { get; set; } = string.Empty;
        public string client_grpc_mongo { get; set; } = string.Empty;
        public int timeoutGrpcSybase { get; set; }
        public int delayOutGrpcSybase { get; set; }
        public int timeoutGrpcMongo { get; set; }
        public int delayOutGrpcMongo { get; set; }
        #endregion

        #region ConfigMongodb
        public string nombre_base_mongo { get; set; } = string.Empty;
        public string coll_peticiones { get; set; } = string.Empty;
        public string coll_errores { get; set; } = string.Empty;
        public string coll_amenazas { get; set; } = string.Empty;
        public string coll_respuesta { get; set; } = string.Empty;
        public string coll_peticiones_diarias { get; set; } = string.Empty;
        public string coll_promedio_peticiones_diarias { get; set; } = string.Empty;
        public string coll_errores_db { get; set; } = string.Empty;
        public string coll_errores_http { get; set; } = string.Empty;
        #endregion

        #region DataBases
        public string db_mongo { get; set; } = string.Empty;
        #endregion

        #region Tiempos demonio
        public int min_anulacion { get; set; }
        public int seco_anulacion { get; set; }
        public int mili_anulacion { get; set; }
        public int min_respaldo { get; set; }
        public int seco_respaldo { get; set; }
        public int mili_respaldo { get; set; }
        #endregion

        #region CollectionsMongo
        public string solicitudes { get; set; } = "peticiones";
        public string respuestas { get; set; } = "respuesta";
        public string errores { get; set; } = "errores";
        public string errores_http { get; set; } = "errores_http";
        public string respuestas_http { get; set; } = "respuestas_http";
        public string notificaciones_push { get; set; } = "notificaciones_push";
        public string errores_db { get; set; } = "errores_db";
        #endregion

        #region Variables
        public int length_pin { get; set; } = 0;
        public int tiempo_pin_valido_min { get; set; } = 0;
        #endregion

    }
}
