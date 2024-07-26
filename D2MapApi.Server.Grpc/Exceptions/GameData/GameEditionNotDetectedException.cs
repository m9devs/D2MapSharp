namespace D2MapApi.Server.Grpc.Exceptions.GameData;

public class GameEditionNotDetectedException : Exception
{
    public GameEditionNotDetectedException() { }
    public GameEditionNotDetectedException(string p_message) : base(p_message) { }
    public GameEditionNotDetectedException(string p_message, Exception p_inner) : base(p_message, p_inner) { }
}