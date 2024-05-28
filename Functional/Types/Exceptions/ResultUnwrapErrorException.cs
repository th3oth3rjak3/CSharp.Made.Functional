namespace Functional;

/// <summary>
/// An exception that is thrown when a Result is unwrapped as an Error, but it's actually Ok.
/// </summary>
public class ResultUnwrapErrorException : Exception
{
    /// <summary>
    /// Create a new ResultUnwrapErrorException.
    /// </summary>
    public ResultUnwrapErrorException() { }

    /// <inheritdoc/>
    public override string Message => "A result was unwrapped as an error when the value was Ok. Be sure to check the result first with 'IsError'.";
}
