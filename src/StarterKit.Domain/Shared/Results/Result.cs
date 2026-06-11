using StarterKit.Domain.Constants;

namespace StarterKit.Domain.Shared.Results;

public interface IResult { bool IsFailure { get; } Error Error { get; } }
public record Error(string Code, string Message);
public class Result<T> : IResult 
{
    public T? Value { get; }
    public bool IsSuccess { get; }
    public string Message { get; }
    public bool IsFailure => !IsSuccess;
    public Error? Error { get; }

    protected Result(T? value, string message,  bool isSuccess, Error? error)
    {
        Value = value;
        IsSuccess = isSuccess;
        Message = message;
        Error = error;
    }

    public static Result<T> Success(T value, string message ="") => new(value, message, true, null);
    public static Result<T> Failure(Error error) => new(default, "", false, error);

    public static implicit operator Result<T>(T value) => Success(value);
    public static implicit operator Result<T>(Error error) => Failure(error);
}

// Factory using the Constants
public static class Errors
{
    public static Error NotFound(string msg) => new(ErrorCodes.NotFound, msg);
    public static Error Validation(string msg) => new(ErrorCodes.Validation, msg);
    public static Error Failure(string msg) => new(ErrorCodes.Failure, msg);
}
