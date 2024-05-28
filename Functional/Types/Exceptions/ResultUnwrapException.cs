namespace Functional;

/// <summary>
/// An exception that is thrown when a Result is unwrapped as an Ok value, but it's actually an Error.
/// </summary>
public sealed class ResultUnwrapException : Exception
{
    /// <summary>
    /// Create a new ResultUnwrapException.
    /// </summary>
    public ResultUnwrapException() { }

    /// <inheritdoc />
    public override string Message => "A result was unwrapped when the value was an Error. Be sure to check the result first with 'IsOk'.";
}
