namespace IzmPortal.Application.Common;

public class Result
{
    public bool Succeeded { get; }
    public string? Message { get; }
    public string? ErrorCode { get; }

    protected Result(
        bool succeeded,
        string? message,
        string? errorCode = null)
    {
        Succeeded = succeeded;
        Message = message;
        ErrorCode = errorCode;
    }

    public static Result Success(string? message = null)
        => new(true, message);

    public static Result Failure(
        string message,
        string? errorCode = null)
        => new(false, message, errorCode);
}

public class Result<T> : Result
{
    public T? Data { get; }

    protected Result(
        bool succeeded,
        T? data,
        string? message,
        string? errorCode = null)
        : base(succeeded, message, errorCode)
    {
        Data = data;
    }

    public static Result<T> Success(
        T data,
        string? message = null)
        => new(true, data, message);

    public static new Result<T> Failure(
        string message,
        string? errorCode = null)
        => new(false, default, message, errorCode);
}
