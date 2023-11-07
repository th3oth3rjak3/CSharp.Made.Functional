namespace Functional.Results;

/// <summary>
/// A class which represents a success or a failure. 
/// This is the primary way to return error types from a function
/// rather than throwing exceptions.
/// </summary>
/// <typeparam name="TSuccess">The type of the inner object when successful.</typeparam>
/// <typeparam name="TError">The type of the inner object when not successful.</typeparam>
public sealed record Result<TSuccess, TError>
{
    private Union<SuccessResult<TSuccess>, FailureResult<TError>> Contents { get; init; }

    /// <summary>
    /// Construct a new Result from a SuccessResult.
    /// </summary>
    /// <param name="success">A result which is a success.</param>
    public Result(SuccessResult<TSuccess> success) =>
        Contents = new Union<SuccessResult<TSuccess>, FailureResult<TError>>(success);

    /// <summary>
    /// Construct a new Result from a FailureResult.
    /// </summary>
    /// <param name="failure">A result which is a failure.</param>
    public Result(FailureResult<TError> failure) =>
        Contents = new Union<SuccessResult<TSuccess>, FailureResult<TError>>(failure);

    /// <summary>
    /// Match the result to a success or failure and perform some function on either case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="onSuccess">Perform some function on the success result.</param>
    /// <param name="onFailure">Perform some function on the failure result.</param>
    /// <returns>The result of executing the onSuccess or onFailure function.</returns>
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onFailure) =>
        Contents
            .Match(
                success => onSuccess(success.Contents),
                failure => onFailure(failure.Contents));

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenSuccess">Perform this action when the value is Success.</param>
    /// <param name="doWhenFailure">Perform this action when the value is Failure.</param>
    public void Effect(Action<TSuccess> doWhenSuccess, Action<TError> doWhenFailure) =>
        this.Contents
            .Effect(
                success => doWhenSuccess(success.Contents),
                failure => doWhenFailure(failure.Contents));

}

/// <summary>
/// A class which represents a success or a failure. 
/// This is the primary way to return error types from a function
/// rather than throwing exceptions.
/// </summary>
public static class Result
{
    /// <summary>
    /// A result that indicates success.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the contents.</typeparam>
    /// <typeparam name="TError">The type if it had been an error.</typeparam>
    /// <param name="input">The contents to store.</param>
    /// <returns>A result which indicates success that contains contents.</returns>
    public static Result<TSuccess, TError> Success<TSuccess, TError>(this TSuccess input) =>
        new(new SuccessResult<TSuccess>(input));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<TSuccess, TError> Failure<TSuccess, TError>(TError error) =>
        new(new FailureResult<TError>(error));

}

/// <summary>
/// A result inidicating success.
/// </summary>
/// <typeparam name="T">The type of the inner value of the result.</typeparam>
/// <param name="Contents">The wrapped inner value of the success.</param>
[ExcludeFromCodeCoverage]
public record SuccessResult<T>(T Contents);

/// <summary>
/// A result indicating failure.
/// </summary>
/// <typeparam name="T">The type of the inner value of the failure.</typeparam>
/// <param name="Contents">The wrapped inner value of the failure.</param>
[ExcludeFromCodeCoverage]
public record FailureResult<T>(T Contents);
