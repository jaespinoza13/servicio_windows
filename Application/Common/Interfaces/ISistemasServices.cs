using Domain.Models;

namespace Application.Common.Interfaces
{
    public interface ISistemasServices
    {
        Task<Parametro> GetParametroWsSistemasRestAsync(int idSistema, string strNemonico);
    }
}
