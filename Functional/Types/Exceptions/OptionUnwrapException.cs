namespace Functional;

/// <summary>
/// An exception that is thrown when a None is unwrapped.
/// </summary>
public sealed class OptionUnwrapException : Exception
{
    /// <summary>
    /// Create a new OptionUnwrapException.
    /// </summary>
    public OptionUnwrapException() { }

    /// <inheritdoc />
    public override string Message => "An option was unwrapped when the value was None. Be sure to check the option first with 'IsSome'.";
}
