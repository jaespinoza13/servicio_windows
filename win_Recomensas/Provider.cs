using Application.Interfaz;
using Infraestructure.Services;
using Application.Common.Models;
using Application.Common.Interfaces;
using Infraestructure.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Infraestructure.gRPC_Clients.Sybase;
using Infraestructure.InterfacesApi.Common;
using Application.Common.Interfaces.Servicios;
using Application.Servicios.Common.WsIdentity;
using Microsoft.Extensions.DependencyInjection;
using Application.Servicios.AprobarTransferencias;
using Infraestructure.InterfacesApi.Transferencias;
using Application.Servicios.Common.Sistemas.WsSistemas;
using Application.Servicios.Common.Sistemas.MegSistemas;

namespace servicio_windows
{
    internal class Provider
    {
        private readonly ServiceCollection _grpcService;
        private readonly IConfiguration _conf;
        public Provider(ServiceCollection grpcService, IConfiguration configuracion) {
            _grpcService = grpcService;
            _conf = configuracion;
        }
        public ServiceProvider ServiceProvider()
        {
            var serviceProvider = _grpcService.Configure<Configuracion>(_conf!.GetSection("Config:EndPoints"))
                  .Configure<Configuracion>(_conf.GetSection("Config:Settings"))
                  .Configure<Configuracion>(_conf.GetSection("Config:DataBases"))
                  .Configure<Configuracion>(_conf.GetSection("Config:Servicios"))
                  .Configure<Configuracion>(_conf.GetSection("Config:Authorizations"))
                  .Configure<Configuracion>(_conf.GetSection("Config:TypeAuthorizations"))
                  .Configure<Configuracion>(_conf.GetSection("Config:GrpcSettings"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ConfigMongodb"))
                  .AddSingleton<IHttpService, HttpService>()
                  .AddSingleton<ITransferenciasApi, TransferenciasApi>()
                  .AddSingleton<ISistemasApi, SistemasApi>()
                  .AddSingleton<IIdentityApi, IdentityApi>()
                  .AddSingleton<ISistemasDat, SistemasDat>()
                  .AddSingleton<IWsSistemas, WsSistemas>()
                  .AddSingleton<IWsIdentity, WsIdentity>()
                  .AddSingleton<IAprobarTransferencias, AprobarTransferencias>()
                  .AddSingleton<IMegSistemas, MegSistemas>()
                  .AddSingleton<ILogs, LogsServices>()
                  .BuildServiceProvider();
            return serviceProvider;
        }
    }
}
