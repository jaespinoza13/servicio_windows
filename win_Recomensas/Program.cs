using System.Net;
using servicio_windows;
using Application.Interfaz;
using System.Net.NetworkInformation;
using Application.Common.Interfaces;
using static AccesoDatosGrpcAse.Neg.DAL;
using Infraestructure.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Application.Servicios.Common.WsIdentity;
using Microsoft.Extensions.DependencyInjection;
using Application.Servicios.AprobarTransferencias;
using Application.Servicios.Common.Sistemas.WsSistemas;
using Application.Servicios.Common.Sistemas.MegSistemas;

namespace servicioWindows
{
    class AppServicioWindows
    {
        private static ISistemasApi? wsSistemas;
        private static IIdentityApi? wsIdentity;
        private static ITransferenciasApi? wsProcTransf;
        private static ISistemasDat? wsSisDat;
        private static ILogs? logs;

        private static string str_frecuencia_ejecucion = "";
        private static string str_horario_ejecucion = "";
        private static string str_servicio = "";
        private static string str_ip = "";
        private static string str_mac = "";

        static async Task Main(string[] args)
        {
            Console.WriteLine("Servicio windows general");

            IConfiguration configuracion = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            IConfiguration conf = configuracion!;
            var grpcService = new ServiceCollection();
            grpcService.AddGrpcClient<DALClient>(o =>
            {
                o.Address = new Uri(conf["Config:GrpcSettings:client_grpc_sybase"]!);
            }).ConfigureChannel(c =>
            {
                c.HttpHandler = new SocketsHttpHandler
                {
                    PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                    KeepAlivePingDelay = TimeSpan.FromSeconds(20),
                    KeepAlivePingTimeout = TimeSpan.FromSeconds(60),
                    EnableMultipleHttp2Connections = true
                };
            });

            Provider servProvider = new Provider(grpcService, conf);

            var serviceProvider = servProvider.ServiceProvider();

            wsSisDat = serviceProvider.GetService<ISistemasDat>();
            wsSistemas = serviceProvider.GetService<ISistemasApi>();
            wsIdentity = serviceProvider.GetService<IIdentityApi>();
            wsProcTransf = serviceProvider.GetService<ITransferenciasApi>();
            logs = serviceProvider.GetService<ILogs>();
            serviceProvider.GetService<IWsSistemas>();
            serviceProvider.GetService<IWsIdentity>();
            serviceProvider.GetService<IHttpService>();
            str_ip = getIp();
            str_mac = GetMacAddress();

            string[] argss = new string[] { "IEAT", "FEAT", "APROBAR_TRANSFERENCIAS" };

            var services_thread = new Thread(() => OnStart(argss));
            services_thread.Start();
        }

        static async void OnStart(string[] args)
        {
            try
            {
                WsSistemas sistemas = new WsSistemas(wsSistemas!, logs!);
                WsIdentity identity = new WsIdentity(wsIdentity!, logs!);
                MegSistemas sisDat = new MegSistemas(wsSisDat!, logs!);

                AprobarTransferencias transf = new AprobarTransferencias(wsProcTransf!, logs!, sisDat, identity, sistemas);

                if (args.Length > 0) str_horario_ejecucion = args[0];
                if (args.Length > 1) str_frecuencia_ejecucion = args[1];
                if (args.Length > 2)
                {
                    str_servicio = args[2];
                    switch (str_servicio)
                    {
                        case "APROBAR_TRANSFERENCIAS":
                            Console.WriteLine("Servicio windows para aprobar transferencias");

                            ReqAprobarTransf req = new ReqAprobarTransf
                            {
                                str_mac = str_mac,
                                str_ip = str_ip,
                                str_frecuencia_ejecucion = str_frecuencia_ejecucion,
                                str_horario_ejecucion = str_horario_ejecucion
                            };
                            var respuesta = await transf.AprobTransferencias(req);
                            await logs!.SaveResponseLogs(respuesta, "APROBAR_TRANSFERENCIAS", "OnStart", "AppServicioWindows");
                            break;
                    }
                } else
                {
                    Console.WriteLine("Parámetro con el nombre del servicio web indefinido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            string value = Console.ReadLine()!;
        }

        static string getIp()
        {
            try
            {
                string strHostName = Dns.GetHostName();
                IPHostEntry ipEntry = Dns.GetHostEntry(strHostName);
                IPAddress[] addr = ipEntry.AddressList;
                return addr[addr.Length - 1].ToString();
            }
            catch (Exception)
            {
                return "";
            }
        }

        static string GetMacAddress()
        {
            string macAddresses = string.Empty;

            foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (nic.OperationalStatus == OperationalStatus.Up)
                {
                    macAddresses += nic.GetPhysicalAddress().ToString();
                    break;
                }
            }

            return macAddresses;
        }
    }
}
