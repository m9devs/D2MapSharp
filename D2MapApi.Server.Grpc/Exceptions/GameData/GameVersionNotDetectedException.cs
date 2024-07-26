namespace D2MapApi.Server.Grpc.Exceptions.GameData;

public class GameVersionNotDetectedException : Exception
{
    public GameVersionNotDetectedException() { }
    public GameVersionNotDetectedException(string p_message) : base(p_message) { }
    public GameVersionNotDetectedException(string p_message, Exception p_inner) : base(p_message, p_inner) { }
}