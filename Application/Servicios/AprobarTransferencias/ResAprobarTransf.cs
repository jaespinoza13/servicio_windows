using Application.Common.Models;

namespace Application.Servicios.AprobarTransferencias
{
    public class ResAprobarTransf : ResComun
    {
        public List<TransfProcesada>? transf_procesada { get; set; }
    }
}
