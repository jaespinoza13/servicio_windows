using System.Reflection;
using Application.Interfaz;
using AccesoDatosGrpcAse.Neg;
using Application.Common.Models;
using Application.Common.Interfaces;
using Infrastructure.Common.Funciones;
using static AccesoDatosGrpcAse.Neg.DAL;

namespace Infraestructure.gRPC_Clients.Sybase
{
    public class SistemasDat : ISistemasDat
    {
        public readonly DALClient _objClienteDal;
        private readonly ILogs _logsService;
        public readonly string _str_clase;

        public SistemasDat(DALClient objClienteDal, ILogs logs)
        {
            _str_clase = GetType().Name;
            _objClienteDal = objClienteDal;
            _logsService = logs;
        }

        public async Task<RespuestaTransaccion> GetIdSistema(string str_nombre_sistema)
        {
            string str_operacion = "GET_ID_SISTEMA";
            await _logsService.SaveHeaderLogs(str_nombre_sistema, str_operacion, MethodBase.GetCurrentMethod()!.Name, _str_clase);
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            try
            {
                DatosSolicitud ds = new();
                ds.ListaPEntrada.Add(new ParametroEntrada { StrNameParameter = "@prm_sistema", TipoDato = TipoDato.VarChar, ObjValue = str_nombre_sistema });
                ds.ListaPSalida.Add(new ParametroSalida { StrNameParameter = "@prm_id", TipoDato = TipoDato.Integer });
                ds.NombreSP = "stp_get_id_sistema";
                ds.NombreBD = "meg_sistemas";

                var resultado = _objClienteDal.ExecuteDataSet(ds);
                var lst_valores = new List<ParametroSalidaValores>();

                foreach (var item in resultado.ListaPSalidaValores) lst_valores.Add(item);
                var int_id_sistema = lst_valores.Find(x => x.StrNameParameter == "@prm_id")!.ObjValue;

                respuesta.str_codigo = "0".ToString().Trim().PadLeft(3, '0');
                respuesta.obj_cuerpo = Funciones.ObtenerDatos(resultado);
                respuesta.dcc_variables.Add("str_error", "");
                respuesta.dcc_variables.Add("int_id_sistema", int_id_sistema);
            }
            catch (Exception ex)
            {
                respuesta.str_codigo = "003";
                respuesta.dcc_variables.Add("str_error", ex.ToString());
                await _logsService.SaveExecptionLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _str_clase, ex);
            }
            await _logsService.SaveResponseLogs(respuesta, str_operacion, MethodBase.GetCurrentMethod()!.Name, _str_clase);
            return respuesta;
        }
    }
}
