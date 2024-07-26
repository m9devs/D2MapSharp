// 

namespace D2MapApi.Server.Grpc.Global.IO.Directories;

public static class ApplicationDirectories
{
    #if DEBUG
    public static string ServerDataPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "BMBot", "D2MapApi", "Server", "Debug");
    #else
    public static string ServerDataPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                     "BMBot", "D2MapApi", "Server");
    #endif

    public static string LogsDataPath => Path.Combine(ServerDataPath, "Logs");

    public static void CreateRequiredDirectories()
    {
        // Logs data path is automatically created by ILogger. - Comment by M9 on 07/02/2024 @ 10:17:49
        Directory.CreateDirectory(ServerDataPath);
    }
}