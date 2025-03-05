namespace api.CQS;

public interface IResult
{
    bool IsSuccess { get; }

    string? ErrorMessage { get; }
    Exception? Exception { get; }
}