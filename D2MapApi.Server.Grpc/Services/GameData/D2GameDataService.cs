using D2MapApi.Common.Enumerations.Extensions;
using D2MapApi.Common.Enumerations.GameData;
using D2MapApi.Server.Grpc.Exceptions.GameData;
using D2MapApi.Server.Grpc.Services.Cryptography.Hashing;
using D2MapApi.Server.Grpc.Services.GameData.Edition;
using D2MapApi.Server.Grpc.Services.GameData.Version;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.GameData;

internal class D2GameDataService(ILogger<D2GameDataService> i_logger,
                                 D2EditionService           i_d2EditionService,
                                 D2VersionService           i_d2VersionService,
                                 HashingService             i_hashingService)
{
    internal string D2ExecutablePath => Path.Combine(D2DirectoryPath, "Game.exe");
    internal string D2DirectoryPath  { get; private set; } = string.Empty;
    
    internal D2GameEdition D2GameEdition      { get; private set; } = D2GameEdition.UNKNOWN;
    internal D2GameVersion D2GameVersion      { get; private set; } = D2GameVersion.UNKNOWN;
    
    internal async Task InitializeAsync(string p_d2GameDirectoryPath)
    {
        i_logger.LogInformation("Initializing game data");
        i_logger.LogDebug("Initializing with game executable path '{GameExecutablePath}'", p_d2GameDirectoryPath);
        
        if ( !Directory.Exists(p_d2GameDirectoryPath) )
        {
            throw new DirectoryNotFoundException();
        }
        
        D2DirectoryPath = p_d2GameDirectoryPath;

        await DetectGameData(D2ExecutablePath);
        
        await Task.CompletedTask;
    }

    internal async Task ValidateAsync()
    {
        if (D2GameEdition == D2GameEdition.UNKNOWN)
        {
            throw new GameEditionNotDetectedException();
        }
        
        if (D2GameVersion == D2GameVersion.UNKNOWN)
        {
            throw new GameVersionNotDetectedException();
        }

        // TODO: Throw exception for incompatible editions/versions. - Comment by M9 on 06/19/2024 @ 11:27:44
        
        await Task.CompletedTask;
    }

    private async Task DetectGameData(string p_gameExecutablePath)
    {
        i_logger.LogDebug("Detecting executable data for '{GameExecutablePath}'", p_gameExecutablePath);
        
        var hashString = await i_hashingService.GetFileHashAsync(p_gameExecutablePath);

        D2GameEdition = i_d2EditionService.GetGameEdition(hashString);
        D2GameVersion = i_d2VersionService.GetGameVersion(hashString);
        
        i_logger.LogInformation("Detected game edition as '{GameEdition}'", D2GameEdition.ToFriendlyString());
        i_logger.LogInformation("Detected game version as '{GameVersion}'", D2GameVersion);
    }
}