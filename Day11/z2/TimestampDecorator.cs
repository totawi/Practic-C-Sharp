public class TimestampDecorator : LoggerDecorator
{
    public TimestampDecorator(ILogger logger) : base(logger) { }
    public override string Log(string message)
        => $"[{DateTime.Now:HH:mm:ss}] " + base.Log(message);
}
