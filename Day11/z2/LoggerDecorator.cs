
// 3. Базовый декоратор
public abstract class LoggerDecorator : ILogger
{
    protected ILogger _logger;
    public LoggerDecorator(ILogger logger) => _logger = logger;
    public virtual string Log(string message) => _logger.Log(message);
}
