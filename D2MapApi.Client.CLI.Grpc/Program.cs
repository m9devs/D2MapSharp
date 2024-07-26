using CommandLine;

using D2MapApi.Client.CLI.Grpc.DataStructures.ProgramArguments;
using D2MapApi.Client.CLI.Grpc.Global.IO.Files;
using D2MapApi.Client.CLI.Grpc.Services;
using D2MapApi.Common.Exceptions.Runtime;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;
using Serilog.Events;

namespace D2MapApi.Client.CLI.Grpc;

internal static class Program
{
    public static async Task<int> Main(string[] p_args)
    {
        var parseResult = Parser.Default.ParseArguments<ActOptions, MapOptions>(p_args);

        if ( parseResult.Value is not ICommandLineOptions options )
        {
            // Help message handled by ParseArguments above, no need to log here. - Comment by M9 on 06/19/2024 @ 09:24:54
            return 1;
        }

        var serviceCollection = ConfigureServices();

        serviceCollection.AddLogging(p_builder => ConfigureLogging(p_builder, options.ShouldShowDebugLogs));

        var serviceProvider = serviceCollection.BuildServiceProvider();

        var application = serviceProvider.GetRequiredService<MapperApplicationService>();

        try
        {
            await application.RunAsync(parseResult);
        }
        catch ( RuntimeException exception )
        {
            // Error log messages should be handled by inner exception handlers.
            // Just return an error code for external use here. - Comment by M9 on 06/19/2024 @ 09:25:53
            return (int)exception.ErrorCode;
        }

        return 0;
    }

    private static ServiceCollection ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddSingleton<MapperApplicationService>();

        return serviceCollection;
    }

    private static void ConfigureLogging(ILoggingBuilder p_builder, bool p_showDebug)
    {
        p_builder.ClearProviders();

        Log.Logger = new LoggerConfiguration()
                     .MinimumLevel
                     .Is(p_showDebug ? LogEventLevel.Debug : LogEventLevel.Information)
                     .WriteTo.Console()
                     .WriteTo.File(ApplicationFiles.LogsFilePath,
                                   rollingInterval: RollingInterval.Day,
                                   retainedFileCountLimit: 31,
                                   fileSizeLimitBytes: 1024 * 1024 * 10,
                                   rollOnFileSizeLimit: true)
                     .CreateLogger();

        p_builder.AddSerilog(Log.Logger);
    }
}