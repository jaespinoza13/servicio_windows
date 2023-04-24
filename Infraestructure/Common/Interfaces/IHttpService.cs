using Application.Common.Models;

namespace Infraestructure.Common.Interfaces
{
    public interface IHttpService
    {
        Task<object> solicitar_servicio(SolicitarServicio solicitarServicio);
        object solicitar_servicio_async(SolicitarServicio solicitarServicio);
    }
}
