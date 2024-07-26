// 

namespace D2MapApi.Client.CLI.Grpc.Global.IO.Directories;

public static class ApplicationDirectories
{
    #if DEBUG
    public static string ClientDataPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "BMBot", "D2MapApi", "Client", "gRPC", "Debug");
    #else
    public static string ClientDataPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "BMBot", "D2MapApi", "Client", "gRPC");
    #endif

    public static string LogsDataPath => Path.Combine(ClientDataPath, "Logs");

    public static void CreateRequiredDirectories()
    {
        // Logs data path is automatically created by ILogger. - Comment by M9 on 07/02/2024 @ 10:17:49
        Directory.CreateDirectory(ClientDataPath);
    }
}