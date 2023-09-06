using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Exceptions;

[ExcludeFromCodeCoverage]
public sealed record TryResult<T>
{
    private Union<TrySuccess<T>, TryFailure<T>> Contents { get; init; }
    public TryResult(TrySuccess<T> success) => Contents = new Union<TrySuccess<T>, TryFailure<T>>(success);

    public TryResult(TryFailure<T> failure) => Contents = new Union<TrySuccess<T>, TryFailure<T>>(failure);

    /// <summary>
    /// Match the TryResult to either Success or Failure and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="tryResult">The result of trying something to be matched.</param>
    /// <param name="onSuccess">The fucntion to execute when TrySuccess.</param>
    /// <param name="onFailure">The function to execute when TryFailure.</param>
    /// <returns>The result of the function performed.</returns>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Exception, TResult> onFailure) =>
        Contents
            .Match(
                success => onSuccess(success.Contents),
                failure => onFailure(failure.Exception));
}

public static class TryResult
{
    public static TryResult<T> Success<T>(T input) =>
        new(new TrySuccess<T>(input));

    public static TryResult<T> Failure<T>(Exception exception) =>
        new(new TryFailure<T>(exception));
}

[ExcludeFromCodeCoverage]
public record TrySuccess<T>(T Contents);

[ExcludeFromCodeCoverage]
public record TryFailure<T>(Exception Exception);