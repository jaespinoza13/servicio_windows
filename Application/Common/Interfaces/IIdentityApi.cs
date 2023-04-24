using Application.Common.Models;
using Application.Servicios.Common.WsIdentity;

namespace Application.Common.Interfaces
{
    public interface IIdentityApi
    {
        Task<RespuestaTransaccion> GenerateToken(ReqGenerateToken req_generate_token);
    }
}
