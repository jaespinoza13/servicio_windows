using Application.Servicios.Common.WsIdentity;

namespace Application.Common.Interfaces
{
    public interface IWsIdentity
    {
        Task<ResGenerateToken> GenerateToken(ReqGenerateToken req_generate_token);
    }
}
