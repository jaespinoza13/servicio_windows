using System;
using Application.Common.Models;
using Microsoft.Extensions.Options;
using Application.Common.Interfaces;
using Infraestructure.Common.Interfaces;

namespace Infraestructure.Services
{
    public class LogsServices : ILogs
    {
        private readonly Configuracion _configuration;
        private readonly IHttpService _httpService;
        public Dictionary<string, object> _logs;
        private readonly SolicitarServicio solicitarServicio = new();

        public LogsServices(IOptionsMonitor<Configuracion> options, IHttpService httpService)
        {
            _configuration = options.CurrentValue;
            _httpService = httpService;
            solicitarServicio.urlServicio = _configuration.url_acceso_logs + "saveLogs/";
            _logs = new Dictionary<string, object>();
            _logs.Add("str_base", _configuration.db_mongo);
            _logs.Add("str_collection", "");
            _logs.Add("tipo_log", "");
            _logs.Add(_configuration.typeAuthAccesoLogs, _configuration.auth_acceso_logs);

            solicitarServicio.dcyHeadersAdicionales = _logs;
        }
        public Task SaveErroresDb(object obj_error)
        {
            solicitarServicio.dcyHeadersAdicionales["str_collection"] = _configuration.errores_db;
            solicitarServicio.dcyHeadersAdicionales["tipo_log"] = _configuration.errores_db;
            solicitarServicio.objSolicitud = obj_error;
            _httpService.solicitar_servicio_async(solicitarServicio);
            return Task.CompletedTask;
        }

        public Task SaveExecptionLogs(dynamic transaction, string str_operacion, string str_metodo, string str_clase, Exception obj_error)
        {
            var objError = new { str_id_transaccion = transaction.str_id_transaccion, error = obj_error.Message };

            solicitarServicio.dcyHeadersAdicionales["str_collection"] = _configuration.errores;
            solicitarServicio.dcyHeadersAdicionales["tipo_log"] = _configuration.errores;
            solicitarServicio.objSolicitud = objError;
            _httpService.solicitar_servicio_async(solicitarServicio);
            return Task.CompletedTask;
        }

        public Task SaveHeaderLogs(dynamic transaccion, string str_operacion, string str_metodo, string str_clase)
        {
            solicitarServicio.dcyHeadersAdicionales["str_collection"] = _configuration.solicitudes;
            solicitarServicio.dcyHeadersAdicionales["tipo_log"] = _configuration.solicitudes;
            solicitarServicio.objSolicitud = transaccion;

            _httpService.solicitar_servicio_async(solicitarServicio);
            return Task.CompletedTask;
        }

        public Task SaveHttpErrorLogs(object obj_solicitud, string str_error, string str_id_transaccion)
        {
            var objError = new { peticion = obj_solicitud, error = str_error };

            solicitarServicio.dcyHeadersAdicionales["str_collection"] = _configuration.errores_http;
            solicitarServicio.dcyHeadersAdicionales["tipo_log"] = _configuration.errores_http;
            solicitarServicio.objSolicitud = objError;
            _httpService.solicitar_servicio_async(solicitarServicio);
            return Task.CompletedTask;

        }

        public Task SaveResponseLogs(dynamic transaction, String str_operacion, String str_metodo, String str_clase)
        {
            solicitarServicio.dcyHeadersAdicionales["str_collection"] = _configuration.respuestas;
            solicitarServicio.dcyHeadersAdicionales["tipo_log"] = _configuration.respuestas;
            solicitarServicio.objSolicitud = transaction;
            _httpService.solicitar_servicio_async(solicitarServicio);
            return Task.CompletedTask;

        }
    }
}
