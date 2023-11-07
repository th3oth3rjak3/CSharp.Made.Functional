namespace Functional.Exceptions;

/// <summary>
/// The result of trying an operation which could throw an exception.
/// </summary>
/// <typeparam name="T">The inner result of trying the operation.</typeparam>
[ExcludeFromCodeCoverage]
public sealed record TryResult<T>
{
    private Union<TrySuccess<T>, TryFailure<T>> Contents { get; init; }

    /// <summary>
    /// Construct a new TryResult from a TrySuccess.
    /// </summary>
    /// <param name="success">The success result after trying an exceptional operation.</param>
    public TryResult(TrySuccess<T> success) => Contents = new Union<TrySuccess<T>, TryFailure<T>>(success);

    /// <summary>
    /// Construct a new TryResult from a TryFailure.
    /// </summary>
    /// <param name="failure">The failure result after trying an exceptional operation.</param>
    public TryResult(TryFailure<T> failure) => Contents = new Union<TrySuccess<T>, TryFailure<T>>(failure);

    /// <summary>
    /// Match the TryResult to either Success or Failure and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="onSuccess">The fucntion to execute when TrySuccess.</param>
    /// <param name="onFailure">The function to execute when TryFailure.</param>
    /// <returns>The result of the function performed.</returns>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Exception, TResult> onFailure) =>
        Contents
            .Match(
                success => onSuccess(success.Contents),
                failure => onFailure(failure.Exception));

    /// <summary>
    /// Perform a side-effect on a TryResult type.
    /// </summary>
    /// <param name="doWhenSuccess">Perform this action when the value is Success.</param>
    /// <param name="doWhenFailure">Perform this action when the value is Failure.</param>
    public void Effect(Action<T> doWhenSuccess, Action<Exception> doWhenFailure) =>
        this.Contents
            .Effect(
                success => doWhenSuccess(success.Contents),
                failure => doWhenFailure(failure.Exception));

}

/// <summary>
/// The result of trying an operation which could throw an exception.
/// </summary>
public static class TryResult
{
    /// <summary>
    /// Create a new TryResult which indicates success.
    /// </summary>
    /// <typeparam name="T">The type of the inner contents.</typeparam>
    /// <param name="input">The input to wrap with a TryResult.</param>
    /// <returns>A TryResult to be handled by the Catch method.</returns>
    public static TryResult<T> Success<T>(T input) =>
        new(new TrySuccess<T>(input));

    /// <summary>
    /// Create a new TryResult which indicates failure.
    /// </summary>
    /// <typeparam name="T">The type of the inner contents.</typeparam>
    /// <param name="exception">The exception that was thrown during the Try operation.</param>
    /// <returns>A TryResult to be handled by the Catch method.</returns>
    public static TryResult<T> Failure<T>(Exception exception) =>
        new(new TryFailure<T>(exception));
}

/// <summary>
/// The result of a Try operation which indicates success.
/// </summary>
/// <typeparam name="T">The return type from the Try operation.</typeparam>
/// <param name="Contents">The actual successful result of the Try operation.</param>
[ExcludeFromCodeCoverage]
public record TrySuccess<T>(T Contents);

/// <summary>
/// The result of a Try operation which indicates failure.
/// </summary>
/// <typeparam name="T">The return type from the Try operation if it would have succeeded.</typeparam>
/// <param name="Exception">The Exception that was thrown during the Try operation.</param>
[ExcludeFromCodeCoverage]
public record TryFailure<T>(Exception Exception);