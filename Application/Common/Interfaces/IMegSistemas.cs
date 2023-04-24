using Application.WsSistemasDat;

namespace Application.Common.Interfaces
{
    public interface IMegSistemas
    {
        Task<ResGetIdSistema> GetIdSistema(ReqGetIdSistema req_get_id_sistema);
    }
}
