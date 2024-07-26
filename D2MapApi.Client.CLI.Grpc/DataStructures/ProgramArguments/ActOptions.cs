using CommandLine;

using D2MapApi.Common.Enumerations.GameData;

namespace D2MapApi.Client.CLI.Grpc.DataStructures.ProgramArguments;

[Verb("act", HelpText = "Generate data for a specific act.")]
internal sealed class ActOptions : ICommandLineOptions
{
    [Option('n', "number", Required = true, HelpText = "Act number to generate data for. Must be an integer between 1 and 5.")]
    public int ActNumber { get; set; }

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
        if (ActNumber is < 1 or > 5)
        {
            throw new ArgumentException("Act number must be an integer between 1 and 5.");
        }

        if ( !Directory.Exists(D2DirectoryPath) ||
             !File.Exists(Path.Combine(D2DirectoryPath, "Game.exe")) )
        {
            throw new ArgumentException("Path to the Diablo II executable is invalid.");
        }

        await Task.CompletedTask;
    }
}