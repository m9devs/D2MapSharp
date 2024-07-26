using System.Runtime.InteropServices;

using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Common.Exceptions.Runtime;
using D2MapApi.Core;
using D2MapApi.Core.Models;
using D2MapApi.Server.Grpc.Global.IO.Directories;
using D2MapApi.Server.Grpc.Global.IO.Files;
using D2MapApi.Server.Grpc.Global.Utilities.Logging;
using D2MapApi.Server.Grpc.Services.Cryptography.Hashing;
using D2MapApi.Server.Grpc.Services.GameData;
using D2MapApi.Server.Grpc.Services.GameData.Edition;
using D2MapApi.Server.Grpc.Services.GameData.Version;
using D2MapApi.Server.Grpc.Services.GrpcServiceImplementations;
using D2MapApi.Server.Grpc.Services.MapData;
using D2MapApi.Server.Grpc.Services.ObjectTranslation;
using D2MapApi.Server.Grpc.Services.Startup;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Serilog;

namespace D2MapApi.Server.Grpc
{
    internal static class Program
    {
        public static async Task<int> Main(string[] p_args)
        {
            ApplicationDirectories.CreateRequiredDirectories();

            var appBuilder = WebApplication.CreateBuilder(p_args);

            ConfigureLogging(appBuilder);

            ConfigureServices(appBuilder);

            if ( RuntimeInformation.IsOSPlatform(OSPlatform.Windows) )
            {
                appBuilder.Host.UseWindowsService();
            }
            else if ( RuntimeInformation.IsOSPlatform(OSPlatform.Linux) )
            {
                appBuilder.Host.UseSystemd();
            }

            appBuilder.Services.AddGrpc();

            var app = appBuilder.Build();

            var appInitializationService = app.Services.GetRequiredService<ApplicationInitializationService>();

            try
            {
                await appInitializationService.InitializeApplicationAsync(app.Configuration["Diablo2Directory"] ?? string.Empty);
            }
            catch ( RuntimeException exception )
            {
                return (int)exception.ErrorCode;
            }
            
            MapGrpcEndpointServices(app);

            await app.RunAsync();

            return 0;
        }

        private static void ConfigureServices(WebApplicationBuilder p_appBuilder)
        {
            p_appBuilder.Services.AddMemoryCache();

            p_appBuilder.Services.AddTransient<HashingService>();

            p_appBuilder.Services.AddTransient<ApplicationInitializationService>();
            
            p_appBuilder.Services.AddSingleton<IMapService, MapService>();
            p_appBuilder.Services.AddTransient<IMapDataService, MapDataService>();
            p_appBuilder.Services.AddSingleton<D2GameDataService>();
            p_appBuilder.Services.AddSingleton<D2VersionService>();
            p_appBuilder.Services.AddSingleton<D2EditionService>();
            p_appBuilder.Services.AddTransient<MapObjectTranslationService>();
        }
        
        private static void ConfigureLogging(WebApplicationBuilder p_appBuilder)
        {
            var configuredLogLevel = LogLevelUtilities.GetLogLevel(p_appBuilder.Configuration["Logging:LogLevel:Default"]);

            p_appBuilder.Logging.ClearProviders();
            
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel
                         .Is(LogLevelUtilities.GetSerilogLogLevel(configuredLogLevel))
                         .WriteTo.Console()
                         .WriteTo.File(ApplicationFiles.LogsFilePath,
                                       rollingInterval: RollingInterval.Day,
                                       retainedFileCountLimit: 31,
                                       fileSizeLimitBytes: 1024 * 1024 * 10,
                                       rollOnFileSizeLimit: true)
                         .WriteTo.Debug()
                         .CreateLogger();
            
            p_appBuilder.Logging.AddSerilog(Log.Logger);
        }

        private static void MapGrpcEndpointServices(WebApplication p_application)
        {
            p_application.MapGrpcService<ConnectivityServiceImplementation>();
            p_application.MapGrpcService<MapServiceImplementation>();
            
            p_application.MapGet("/", () => "Communication with the D2MapApi gRPC server must be made through a gRPC client.");
        }
    }
    
}