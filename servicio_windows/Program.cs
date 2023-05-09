using servicio_windows;
using Application.Interfaz;
using Application.WsSitemas;
using Application.WsSistemasDat;
using Application.Common.Models;
using Application.Common.Interfaces;
using static AccesoDatosGrpcAse.Neg.DAL;
using Infraestructure.Common.Interfaces;
using Application.Servicios.ProcesarSms;
using Microsoft.Extensions.Configuration;
using Application.Servicios.Common.WsIdentity;
using Microsoft.Extensions.DependencyInjection;
using Application.Servicios.AprobarTransferencias;
using Application.Servicios.Common.Sistemas.WsSistemas;
using Application.Servicios.Common.Sistemas.MegSistemas;

namespace servicioWindows
{
    public static class AppServicioWindows
    {
        private static ITransferenciasApi? wsProcTransf;
        private static IProcesarSmsApi? wsProcSms;

        private static ILogs? logs;
        private static ISistemasApi? wsSistemas;
        private static IIdentityApi? wsIdentity;
        private static ISistemasDat? wsSisDat;
        private static string str_servicio = "";

        private static string str_nombre_sistema = "";
        private static string str_frecuencia_ejecucion = "";
        private static string str_horario_ejecucion = "";
        private static string str_ip = "";
        private static string str_mac = "";

        static async Task Main(string[] args)
        {
            Console.WriteLine(" ************ SERVICIO WINDOWS GENERAL ************");

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
            Utils utils = new Utils();

            var serviceProvider = servProvider.ServiceProvider();

            wsSisDat = serviceProvider.GetService<ISistemasDat>();
            wsSistemas = serviceProvider.GetService<ISistemasApi>();
            wsIdentity = serviceProvider.GetService<IIdentityApi>();

            //Agregar aqui los nuevos servicios web
            wsProcTransf = serviceProvider.GetService<ITransferenciasApi>();
            wsProcSms = serviceProvider.GetService<IProcesarSmsApi>();

            logs = serviceProvider.GetService<ILogs>();
            serviceProvider.GetService<IWsSistemas>();
            serviceProvider.GetService<IWsIdentity>();
            serviceProvider.GetService<IHttpService>();
            str_ip = utils.getIp();
            str_mac = utils.GetMacAddress();

            // string[] argss = new string[] { "IEAT", "FEAT", "WSPROCESARSMS", "APROBAR_TRANSFERENCIAS" };

            var services_thread = new Thread(() => OnStart(args));
            services_thread.Start();
        }

        static async void OnStart(string[] args)
        {
            try
            {
                WsSistemas sistemas = new WsSistemas(wsSistemas!, logs!);
                WsIdentity identity = new WsIdentity(wsIdentity!, logs!);
                MegSistemas sisDat = new MegSistemas(wsSisDat!, logs!);

                //Lamar aca los servicios web
                AprobarTransferencias transf = new AprobarTransferencias(wsProcTransf!, logs!, identity);
                ProcesarSms proces = new ProcesarSms(wsProcSms!, logs!);

                //Verificar los parametros que se enviaran desde pm2
                if (args.Length > 0) str_horario_ejecucion = args[0];
                if (args.Length > 1) str_frecuencia_ejecucion = args[1];
                if (args.Length > 2) str_nombre_sistema = args[2];
                if (args.Length > 3)
                {
                    str_servicio = args[3];

                    ReqGetIdSistema req_get_id_sistema = new ReqGetIdSistema();
                    ReqGetTiemposEjecucion req_tiempos_ejecucion = new ReqGetTiemposEjecucion();
                    string sistema_id = "4"; //Soporte

                    // Consultar id del sistema para los parametros
                    req_get_id_sistema.str_nombre_sistema = str_nombre_sistema;
                    var res_sistema = await sisDat.GetIdSistema(req_get_id_sistema);
                    if (res_sistema.str_res_codigo == "000") sistema_id = res_sistema.str_id_sistema;

                    // Consulta los parametros para determinar el horario y frecuencia de ejecución
                    req_tiempos_ejecucion.int_id_sistema = Convert.ToInt32(sistema_id);
                    req_tiempos_ejecucion.str_nemo_horario = str_horario_ejecucion;
                    req_tiempos_ejecucion.str_nemo_frecuencia_ejecucion = str_frecuencia_ejecucion;

                    ResGetTiemposEjecucion response = await sistemas.GetTiemposEjecucion(req_tiempos_ejecucion);

                    ReqProcesarServicio req = new ReqProcesarServicio
                    {
                        str_mac = str_mac,
                        str_ip = str_ip,
                        int_sistema = Convert.ToInt32(sistema_id),
                        tsp_hora_inicio = response.tsp_hora_inicio,
                        tsp_hora_fin = response.tsp_hora_fin,
                        int_frecuencia_ejecucion = response.int_frecuencia_ejecucion
                    };

                    switch (str_servicio)
                    {
                        case "APROBAR_TRANSFERENCIAS":
                            Console.WriteLine(">> Servicio windows para aprobar transferencias");

                            var resp_aprob_transf = await transf.AprobTransferencias(req);
                            await logs!.SaveResponseLogs(resp_aprob_transf, "APROBAR_TRANSFERENCIAS", "OnStart", "AppServicioWindows");
                            break;
                        case "PROCESAR_SMS":
                            Console.WriteLine(">> Servicio windows para procesar SMS enviados por Eclipsoft");

                            var resp_procesar_sms = await proces.ProcesSms(req);
                            await logs!.SaveResponseLogs(resp_procesar_sms, "PROCESAR_SMS", "OnStart", "AppServicioWindows");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Parámetro con el nombre del servicio web indefinido");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
