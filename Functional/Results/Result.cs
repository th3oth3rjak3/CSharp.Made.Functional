using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Results;

public sealed record Result<T>
{
    private Union<SuccessResult<T>, FailureResult<T>> Contents { get; init; }
    public Result(SuccessResult<T> success) => Contents = new Union<SuccessResult<T>, FailureResult<T>>(success);

    public Result(FailureResult<T> failure) => Contents = new Union<SuccessResult<T>, FailureResult<T>>(failure);

    /// <summary>
    /// Match the result to a success or failure and perform some function on either case.
    /// </summary>
    /// <typeparam name="TInput">The result input type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onSuccess">Perform some function on the success result.</param>
    /// <param name="onFailure">Perform some function on the failure result.</param>
    /// <returns>The result of executing the onSuccess or onFailure function.</returns>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<FailureResult<T>, TResult> onFailure) =>
        Contents
            .Match(
                success => onSuccess(success.Contents),
                onFailure);
}

public static class Result
{
    /// <summary>
    /// A result that indicates success.
    /// </summary>
    /// <typeparam name="TInput">The type of the contents.</typeparam>
    /// <param name="input">The contents to store.</param>
    /// <returns>A result which indicates success that contains contents.</returns>
    public static Result<TInput> Success<TInput>(this TInput input) =>
        new(new SuccessResult<TInput>(input));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the contents if they had been present.</typeparam>
    /// <param name="message">The message to return with the failure.</param>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<TResult> Failure<TResult>(string message) =>
        new(new FailureResult<TResult>(ImmutableList<string>.Empty.Add(message)));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the contents if they had been present.</typeparam>
    /// <param name="messages">The messages to return with the failure.</param>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<TResult> Failure<TResult>(ImmutableList<string> messages) =>
        new(new FailureResult<TResult>(messages));

}

[ExcludeFromCodeCoverage]
public record SuccessResult<T>(T Contents);

[ExcludeFromCodeCoverage]
public record FailureResult<T>(ImmutableList<string> FailureMessages);
