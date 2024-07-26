namespace D2MapApi.Server.Grpc.Exceptions.GameData;

public class ApiInteropInitializationException : Exception
{
    public ApiInteropInitializationException() { }
    public ApiInteropInitializationException(string p_message) : base(p_message) { }
    public ApiInteropInitializationException(string p_message, Exception p_inner) : base(p_message, p_inner) { }
}