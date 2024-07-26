using CommandLine;

using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Client.CLI.Grpc.DataStructures.ProgramArguments;

[Verb("map", HelpText = "Generate data for a specific map.")]
internal sealed class MapOptions : ICommandLineOptions
{
    [Option('i', "id", Required = true, HelpText = "Map ID to generate data for. Must be an integer.")]
    public int MapId { get; set; }

    [Option("d2exe", Required = true, HelpText = "Path to the Diablo II executable.")]
    public string D2DirectoryPath { get; set; } = string.Empty;
    
    [Option('s', "seed", Required = true, HelpText = "Seed to use for the random number generator. Must be a positive integer.")]
    public uint Seed { get; set; }

    [Option('d', "difficulty", Required = true, HelpText = "Difficulty to generate the map for. Must be one of 'Normal', 'Nightmare', or 'Hell'.")]
    public D2Difficulty D2Difficulty { get; set; }

    [Option("debug", Required = false, HelpText = "Enable debug logging.")]
    public bool ShouldShowDebugLogs { get; set; }

    public async Task ValidateAsync()
    {
        // TODO: Validate that the requested Map ID is within a valid range. - Comment by M9 on 06/18/2024 @ 19:45:12
        
        if ( !Directory.Exists(D2DirectoryPath) )
        {
            throw new ArgumentException("Path to the Diablo II executable is invalid.");
        }

        await Task.CompletedTask;
    }
}