using Application.Interfaz;
using Infraestructure.Services;
using Application.Common.Models;
using Application.Common.Interfaces;
using Infraestructure.Common.Interfaces;
using Application.Servicios.ProcesarSms;
using Infraestructure.InterfacesApi.Sms;
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
            //Configuracion del appsettings. Tambien revisar que tenga la misma estructura en Application > Common > Models > Configuracion.cs
            var serviceProvider = _grpcService.Configure<Configuracion>(_conf!.GetSection("Config:EndPoints"))
                  .Configure<Configuracion>(_conf.GetSection("Config:Settings"))

                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosGenerales"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosGenerales:WsSistemasRest"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosGenerales:WsIdentity"))

                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosWin"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosWin:WsTransferencias"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosWin:WsTransferencias:AprobarTransferencias"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosWin:WsProcesarSms"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ServiciosWin:WsProcesarSms:RechazarTransfBloquearCuenta"))

                  .Configure<Configuracion>(_conf.GetSection("Config:DataBases"))
                  .Configure<Configuracion>(_conf.GetSection("Config:Authorizations"))
                  .Configure<Configuracion>(_conf.GetSection("Config:TypeAuthorizations"))
                  .Configure<Configuracion>(_conf.GetSection("Config:GrpcSettings"))
                  .Configure<Configuracion>(_conf.GetSection("Config:ConfigMongodb"))

                  .AddSingleton<IHttpService, HttpService>()
                  .AddSingleton<ILogs, LogsServices>()
                  .AddSingleton<ISistemasApi, SistemasApi>()
                  .AddSingleton<ISistemasDat, SistemasDat>()
                  .AddSingleton<IMegSistemas, MegSistemas>()
                  .AddSingleton<IIdentityApi, IdentityApi>()
                  .AddSingleton<IWsIdentity, WsIdentity>()
                  .AddSingleton<IWsSistemas, WsSistemas>()

                  //Configuracion de los servicios web (api, servicio). Agregar aca los servicios que se vayan a configurar
                  .AddSingleton<ITransferenciasApi, TransferenciasApi>()
                  .AddSingleton<IProcesarSmsApi, ProcesarSmsApi>()
                  .AddSingleton<IAprobarTransferencias, AprobarTransferencias>()
                  .AddSingleton<IProcesarSms, ProcesarSms>()
                  .BuildServiceProvider();
            return serviceProvider;
        }
    }
}
