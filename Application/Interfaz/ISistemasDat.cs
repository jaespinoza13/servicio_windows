using Application.Common.Models;

namespace Application.Interfaz
{
    public interface ISistemasDat
    {
        Task<RespuestaTransaccion> GetIdSistema(string str_nombre_sistema);
    }
}
