using D2MapApi.Common.Exceptions.Runtime;
using D2MapApi.Core;
using D2MapApi.Server.Grpc.Enumerations.Errors;
using D2MapApi.Server.Grpc.Exceptions.GameData;
using D2MapApi.Server.Grpc.Services.GameData;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.Startup;

internal class ApplicationInitializationService(ILogger<ApplicationInitializationService> i_logger,
                                                D2GameDataService                         i_d2GameDataService,
                                                IMapService                               i_mapService)
{
    internal async Task InitializeApplicationAsync(string p_d2DirectoryPath)
    {
        i_logger.LogInformation("Initializing required application services");

        if ( string.IsNullOrWhiteSpace(p_d2DirectoryPath) )
        {
            i_logger.LogError("'Diablo2Directory' is not properly configured in appsettings.json");
            throw new RuntimeException(RuntimeErrorCode.DIRECTORY_NOT_CONFIGURED);
        }
        
        try
        {
            await InitializeD2GameDataService(p_d2DirectoryPath);
            await i_mapService.InitializeAsync(p_d2DirectoryPath);
        }
        catch ( DirectoryNotFoundException )
        {
            i_logger.LogError("The specified directory '{D2Directory}' does not exist", p_d2DirectoryPath);
            throw new RuntimeException(RuntimeErrorCode.DIRECTORY_DOES_NOT_EXIST);
        }
        catch ( GameEditionNotDetectedException exception )
        {
            i_logger.LogError("Failed to detect game edition: {Message}", exception.Message);
            throw new RuntimeException(RuntimeErrorCode.GAME_EDITION_NOT_DETECTED);
        }
        catch ( GameVersionNotDetectedException exception )
        {
            i_logger.LogError("Failed to detect game version: {Message}", exception.Message);
            throw new RuntimeException(RuntimeErrorCode.GAME_VERSION_NOT_DETECTED);
        }
    }

    private async Task InitializeD2GameDataService(string p_d2DirectoryPath)
    {
        await i_d2GameDataService.InitializeAsync(p_d2DirectoryPath);
        await i_d2GameDataService.ValidateAsync();
    }
}