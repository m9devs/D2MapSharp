using D2MapApi.Client.CLI.Grpc.Global.IO.Directories;

namespace D2MapApi.Client.CLI.Grpc.Global.IO.Files;

public static class ApplicationFiles
{
    public static string LogsFilePath => Path.Combine(ApplicationDirectories.LogsDataPath, "activity.log");
}