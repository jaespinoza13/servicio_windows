using Application.Common.Models;
using Application.Servicios.AprobarTransferencias;

namespace Application.Common.Interfaces
{
    public interface ITransferenciasApi
    {
        Task<RespuestaTransaccion> AprobarTransferencias(ReqAprobTransfApi req_aprobar_transf);
    }
}
