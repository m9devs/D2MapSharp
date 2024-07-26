using System.Security.Cryptography;

using Microsoft.Extensions.Logging;

namespace D2MapApi.Server.Grpc.Services.Cryptography.Hashing;

internal class HashingService(ILogger<HashingService> i_logger)
{
    internal async Task<string> GetFileHashAsync(string p_filePath)
    {
        i_logger.LogDebug("Getting hash for file '{FilePath}'", p_filePath);
        
        await using var fileStream = File.OpenRead(p_filePath);
        
        var hash       = await SHA256.HashDataAsync(fileStream);
        var hashString = BitConverter.ToString(hash).Replace("-", string.Empty);
        return hashString;
    }
}