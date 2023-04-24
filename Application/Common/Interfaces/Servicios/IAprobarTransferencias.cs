using Application.Servicios.AprobarTransferencias;

namespace Application.Common.Interfaces.Servicios
{
    public interface IAprobarTransferencias
    {
        Task<ResAprobarTransf> AprobTransferencias(ReqAprobarTransf req_aprobar_transf);
    }
}
