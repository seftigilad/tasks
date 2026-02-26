namespace TaskCore.Common;

public record Result(bool IsSuccess, string? Error = null)
{
    public static Result Ok() => new(true);
    public static Result Fail(string error) => new(false, error);
}

public record Result<T>(bool IsSuccess, T? Data, string? Error = null)
{
    public static Result<T> Ok(T data) => new(true, data);
    public static Result<T> Fail(string error) => new(false, default, error);
}
