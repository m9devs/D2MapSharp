// 

using Microsoft.Extensions.Logging;

using Serilog.Events;

namespace D2MapApi.Server.Grpc.Global.Utilities.Logging;

internal static class LogLevelUtilities
{
    internal static LogLevel GetLogLevel(string? p_level)
    {
        return p_level?.ToUpper() switch
               {
                   #if DEBUG
                   "TRACE" => LogLevel.Trace,
                   "DEBUG" => LogLevel.Debug,
                   #else
                   "TRACE"       => LogLevel.Information,
                   "DEBUG"       => LogLevel.Information,
                   #endif
                   "INFORMATION" => LogLevel.Information,
                   "WARNING"     => LogLevel.Warning,
                   "ERROR"       => LogLevel.Error,
                   "CRITICAL"    => LogLevel.Critical,
                   _             => throw new ArgumentOutOfRangeException(nameof(p_level))
               };
    }

    internal static LogEventLevel GetSerilogLogLevel(LogLevel p_logLevel)
    {
        return p_logLevel switch
               {
                   LogLevel.Trace       => LogEventLevel.Verbose,
                   LogLevel.Debug       => LogEventLevel.Debug,
                   LogLevel.Information => LogEventLevel.Information,
                   LogLevel.Warning     => LogEventLevel.Warning,
                   LogLevel.Error       => LogEventLevel.Error,
                   LogLevel.Critical    => LogEventLevel.Fatal,
                   LogLevel.None        => LogEventLevel.Fatal,
                   _                    => throw new ArgumentOutOfRangeException(nameof(p_logLevel), p_logLevel, null)
               };
    }
}