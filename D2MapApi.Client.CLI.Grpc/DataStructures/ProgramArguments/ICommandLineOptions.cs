using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Client.CLI.Grpc.DataStructures.ProgramArguments;

internal interface ICommandLineOptions
{
    string D2DirectoryPath { get; set; }
    
    uint Seed { get; set; }

    D2Difficulty D2Difficulty { get; set; }

    bool ShouldShowDebugLogs { get; set; }
    
    Task ValidateAsync();
}