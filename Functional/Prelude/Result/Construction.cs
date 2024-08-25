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
    /// <typeparam name="Ok">The type of the success case.</typeparam>
    /// <typeparam name="Error">The type of the Error case.</typeparam>
    /// <param name="ok">The contents for the success case.</param>
    /// <returns>A new result.</returns>
    public static Result<Ok, Error> Ok<Ok, Error>(Ok ok) =>
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
    /// <typeparam name="Ok">The type of the success case.</typeparam>
    /// <param name="success">The contents for the success case.</param>
    /// <returns>A new result.</returns>
    public static Result<Ok, Exception> Ok<Ok>(Ok success) =>
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
    /// <typeparam name="Ok">The type of the success case.</typeparam>
    /// <typeparam name="Error">The type of the Error case.</typeparam>
    /// <param name="error">The contents for the Error case.</param>
    /// <returns>A new result.</returns>
    public static Result<Ok, Error> Error<Ok, Error>(Error error) =>
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
    /// <typeparam name="Ok">The type of the success case.</typeparam>
    /// <param name="error">The contents for the Error case.</param>
    /// <returns>A new result.</returns>
    public static Result<Ok, Exception> Error<Ok>(Exception error) =>
        new(error);

    /// <summary>
    /// Create a new result that represents an Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// // This creates a Result with an Exception using the provided error message.
    /// Result&lt;int, Exception&gt; result = Error&lt;int&gt;("Error!");
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(result.UnwrapError().Message, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the success case.</typeparam>
    /// <param name="errorMessage">The message to use in the Exception.</param>
    /// <returns>A new result.</returns>
    public static Result<Ok, Exception> Error<Ok>(string errorMessage) =>
        new Exception(errorMessage);
}