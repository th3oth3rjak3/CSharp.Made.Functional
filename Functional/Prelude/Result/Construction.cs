namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Create a new result that represents an Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Ok&lt;string, Exception&gt;("hello, world!");
    /// Assert.IsTrue(result.IsOk);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TOk">The type of the success case.</typeparam>
    /// <typeparam name="TError">The type of the Error case.</typeparam>
    /// <param name="ok">The contents for the success case.</param>
    /// <returns>A new result.</returns>
    public static Result<TOk, TError> Ok<TOk, TError>(TOk ok) =>
        new(ok);

    /// <summary>
    /// Create a new result that represents an Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Ok("hello, world!");
    /// Assert.IsTrue(result.IsOk);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TOk">The type of the success case.</typeparam>
    /// <param name="success">The contents for the success case.</param>
    /// <returns>A new result.</returns>
    public static Result<TOk, Exception> Ok<TOk>(TOk success) =>
        new(success);

    /// <summary>
    /// Create a new result that represents an Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Error&lt;string, Exception&gt;(new Exception("Error!"));
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TOk">The type of the success case.</typeparam>
    /// <typeparam name="TError">The type of the Error case.</typeparam>
    /// <param name="error">The contents for the Error case.</param>
    /// <returns>A new result.</returns>
    public static Result<TOk, TError> Error<TOk, TError>(TError error) =>
        new(error);

    /// <summary>
    /// Create a new result that represents an Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Error&lt;string&gt;(new Exception("Error!"));
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TOk">The type of the success case.</typeparam>
    /// <param name="error">The contents for the Error case.</param>
    /// <returns>A new result.</returns>
    public static Result<TOk, Exception> Error<TOk>(Exception error) =>
        new(error);
}