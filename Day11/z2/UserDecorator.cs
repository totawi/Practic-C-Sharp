public class UserDecorator : LoggerDecorator
{
    private string _user;
    public UserDecorator(ILogger logger, string user) : base(logger) => _user = user;
    public override string Log(string message)
        => $" (User: {_user}) " + base.Log(message);
}
