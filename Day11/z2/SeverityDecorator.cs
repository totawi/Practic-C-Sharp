public class SeverityDecorator : LoggerDecorator
{
    private string _severity;
    public SeverityDecorator(ILogger logger, string severity) : base(logger) => _severity = severity;
    public override string Log(string message)
        => $"{_severity}: " + base.Log(message);
}
