using Application.WsSitemas;
using Application.Common.Models;

namespace Application.Common.Interfaces
{
    public interface ISistemasApi
    {
        Task<RespuestaTransaccion> GetParametros(ReqGetParametro reqGetParametro);
    }
}
