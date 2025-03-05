namespace api.CQS;

public interface ICommandDefinition
{
}

public class CommandResult : ICommandResult
{
    internal CommandResult(bool isSuccess, string? errorMessage, Exception? exception)
    {
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        Exception = exception;
    }

    public bool IsSuccess { get; } 


    public string? ErrorMessage { get; } 

    public Exception? Exception { get; } 

}

public interface ICommandResult : IResult
{
    static CommandResult Success()
    {
        return new CommandResult(true, "", null);
    }

    static CommandResult Failure(string errorMessage, Exception? exception = null)
    {
        return new CommandResult(false, errorMessage, exception);
    }
}

public interface ICommandHandler<T> where T : ICommandDefinition
{
    CommandResult Execute(T command);
}