namespace FieldCollectionRegister.Core.Dtos;

public class ServiceResult<T>
{
    public bool Success { get; private init; }
    public T? Value { get; private init; }
    public string? Error { get; private init; }

    public static ServiceResult<T> Ok(T value) => new() { Success = true, Value = value };

    public static ServiceResult<T> Fail(string error) => new() { Success = false, Error = error };
}
