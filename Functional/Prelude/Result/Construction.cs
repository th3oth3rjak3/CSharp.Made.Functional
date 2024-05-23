namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Create a new result that represents a Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Success&lt;string, Exception&gt;("hello, world!");
    /// Assert.IsTrue(result.IsSuccess);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success case.</typeparam>
    /// <typeparam name="TFailure">The type of the failure case.</typeparam>
    /// <param name="success">The contents for the success case.</param>
    /// <returns>A new result.</returns>
    public static Result<TSuccess, TFailure> Success<TSuccess, TFailure>(TSuccess success) =>
        new Result<TSuccess, TFailure>(success);

    /// <summary>
    /// Create a new result that represents a Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Success("hello, world!");
    /// Assert.IsTrue(result.IsSuccess);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success case.</typeparam>
    /// <param name="success">The contents for the success case.</param>
    /// <returns>A new result.</returns>
    public static Result<TSuccess, Exception> Success<TSuccess>(TSuccess success) =>
        new Result<TSuccess, Exception>(success);

    /// <summary>
    /// Create a new result that represents a Failure.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Failure&lt;string, Exception&gt;(new Exception("failure!"));
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success case.</typeparam>
    /// <typeparam name="TFailure">The type of the failure case.</typeparam>
    /// <param name="failure">The contents for the failure case.</param>
    /// <returns>A new result.</returns>
    public static Result<TSuccess, TFailure> Failure<TSuccess, TFailure>(TFailure failure) =>
        new Result<TSuccess, TFailure>(failure);

    /// <summary>
    /// Create a new result that represents a Failure.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Failure&lt;string&gt;(new Exception("failure!"));
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the success case.</typeparam>
    /// <param name="failure">The contents for the failure case.</param>
    /// <returns>A new result.</returns>
    public static Result<TSuccess, Exception> Failure<TSuccess>(Exception failure) =>
        new Result<TSuccess, Exception>(failure);
}