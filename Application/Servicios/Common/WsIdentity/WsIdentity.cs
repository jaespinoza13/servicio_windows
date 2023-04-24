using System.Reflection;
using Application.Common.Interfaces;

namespace Application.Servicios.Common.WsIdentity
{
    public class WsIdentity : IWsIdentity
    {

        private readonly ILogs _logs;
        private readonly IIdentityApi _identityApi;

        public WsIdentity(IIdentityApi identity, ILogs logs)
        {
            _logs = logs;
            _identityApi = identity;
        }

        public async Task<ResGenerateToken> GenerateToken(ReqGenerateToken req_generate_token)
        {
            string operacion = "GENERAR_TOKEN";
            var resp_token = new ResGenerateToken();

            try
            {
                await _logs.SaveHeaderLogs(req_generate_token, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);

                var res_tran = await _identityApi.GenerateToken(req_generate_token);
                resp_token = (ResGenerateToken)res_tran.obj_cuerpo;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await _logs.SaveExecptionLogs(resp_token, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name, ex);
            }
            await _logs.SaveResponseLogs(resp_token, operacion, MethodBase.GetCurrentMethod()!.Name, GetType().Name);
            return resp_token;
        }
    }
}
