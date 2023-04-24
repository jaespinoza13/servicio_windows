using Renci.SshNet;

namespace Infraestructure.Common.Interfaces
{
    public interface ISftpConceccions
    {
        SftpClient Conectar(string host, string usuario, string password);
        void Desconectar(SftpClient sftpClient);
    }
}
