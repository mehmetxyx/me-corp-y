namespace MeCorp.Y.Shared;

public class Result<T>
{
    public T Value { get; set; }
    public bool IsSuccessful { get; set; } = false;
    public string Message { get; set; }
}