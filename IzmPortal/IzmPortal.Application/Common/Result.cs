namespace IzmPortal.Application.Common;

public class Result
{
    public bool Succeeded { get; }
    public string? Message { get; }

    protected Result(bool succeeded, string? message)
    {
        Succeeded = succeeded;
        Message = message;
    }

    public static Result Success(string? message = null)
        => new(true, message);

    public static Result Failure(string message)
        => new(false, message);
}

public class Result<T> : Result
{
    public T? Data { get; }

    protected Result(bool succeeded, T? data, string? message)
        : base(succeeded, message)
    {
        Data = data;
    }

    public static Result<T> Success(T data, string? message = null)
        => new(true, data, message);

    public static new Result<T> Failure(string message)
        => new(false, default, message);
}
