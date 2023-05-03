using Application.Common.Models;
using Application.Servicios.AprobarTransferencias;

namespace Application.Common.Interfaces.Servicios
{
    public interface IAprobarTransferencias
    {
        Task<ResAprobarTransf> AprobTransferencias(ReqProcesarServicio req_aprobar_transf);
    }
}
