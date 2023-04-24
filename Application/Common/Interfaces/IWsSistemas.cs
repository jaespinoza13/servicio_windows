using Application.WsSitemas;

namespace Application.Common.Interfaces
{
    public interface IWsSistemas
    {
        Task<ResGetParametro> GetParametros(ReqGetParametro reqGetParametro);
        Task<ResGetTiemposEjecucion> GetTiemposEjecucion(ReqGetTiemposEjecucion req_get_tiempos_ejecucion);
    }
}
