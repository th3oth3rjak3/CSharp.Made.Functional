using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Results;

public sealed record Result<TSuccess, TError>
{
    private Union<SuccessResult<TSuccess>, FailureResult<TError>> Contents { get; init; }
    public Result(SuccessResult<TSuccess> success) =>
        Contents = new Union<SuccessResult<TSuccess>, FailureResult<TError>>(success);

    public Result(FailureResult<TError> failure) =>
        Contents = new Union<SuccessResult<TSuccess>, FailureResult<TError>>(failure);

    /// <summary>
    /// Match the result to a success or failure and perform some function on either case.
    /// </summary>
    /// <typeparam name="TInput">The result input type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onSuccess">Perform some function on the success result.</param>
    /// <param name="onFailure">Perform some function on the failure result.</param>
    /// <returns>The result of executing the onSuccess or onFailure function.</returns>
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onFailure) =>
        Contents
            .Match(
                success => onSuccess(success.Contents),
                failure => onFailure(failure.Contents));
}

public static class Result
{
    /// <summary>
    /// A result that indicates success.
    /// </summary>
    /// <typeparam name="TSuccess">The type of the contents.</typeparam>
    /// <param name="input">The contents to store.</param>
    /// <returns>A result which indicates success that contains contents.</returns>
    public static Result<TSuccess, TError> Success<TSuccess, TError>(this TSuccess input) =>
        new(new SuccessResult<TSuccess>(input));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the contents if they had been present.</typeparam>
    /// <param name="message">The message to return with the failure.</param>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<TSuccess, TError> Failure<TSuccess, TError>(TError error) =>
        new(new FailureResult<TError>(error));

}

[ExcludeFromCodeCoverage]
public record SuccessResult<T>(T Contents);

[ExcludeFromCodeCoverage]
public record FailureResult<T>(T Contents);
