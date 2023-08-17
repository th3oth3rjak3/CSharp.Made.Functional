using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using OneOf;

namespace Functional.Results;

[GenerateOneOf]
[ExcludeFromCodeCoverage]
public partial class Result<T> : OneOfBase<SuccessResult<T>, FailureResult>
{

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
        new SuccessResult<TInput>(input);

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the contents if they had been present.</typeparam>
    /// <param name="message">The message to return with the failure.</param>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<TResult> Failure<TResult>(string message) =>
        new FailureResult(ImmutableList<string>.Empty.Add(message));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the contents if they had been present.</typeparam>
    /// <param name="messages">The messages to return with the failure.</param>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<TResult> Failure<TResult>(ImmutableList<string> messages) =>
        new FailureResult(messages);

}

[ExcludeFromCodeCoverage]
public record SuccessResult<T>(T Contents);

[ExcludeFromCodeCoverage]
public record FailureResult(ImmutableList<string> FailureMessages);
