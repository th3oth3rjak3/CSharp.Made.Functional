using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Validation;

[ExcludeFromCodeCoverage]
public sealed record ValidationResult<T>
{
    private Union<ValidationSuccess<T>, ValidationFailure<T>> Contents { get; init; }
    public ValidationResult(ValidationSuccess<T> success) => Contents = new Union<ValidationSuccess<T>, ValidationFailure<T>>(success);

    public ValidationResult(ValidationFailure<T> failure) => Contents = new Union<ValidationSuccess<T>, ValidationFailure<T>>(failure);

    /// <summary>
    /// Match the ValidationResult to either Success or Failure and provide functions to handle each case.
    /// </summary>
    /// <typeparam name="TInput">The input type of the entity.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="validationResult">The result of validation to be matched.</param>
    /// <param name="whenValid">The fucntion to execute when ValidationSuccess.</param>
    /// <param name="whenInvalid">The function to execute when ValidationFailure.</param>
    /// <returns>The result of the function performed.</returns>
    public TResult Match<TResult>(Func<T, TResult> whenSuccess, Func<ValidationFailure<T>, TResult> whenFailure) =>
        Contents
            .Match(
                success => whenSuccess(success.Contents),
                whenFailure);
}

public static class ValidationResult
{
    /// <summary>
    /// Create a new Success ValidationResult.
    /// </summary>
    /// <typeparam name="TResult">The validation result type.</typeparam>
    /// <param name="contents">The contents to pass along in the success result.</param>
    /// <returns>The validation result.</returns>
    public static ValidationResult<TResult> Success<TResult>(TResult contents) =>
        new(new ValidationSuccess<TResult>(contents));

    /// <summary>
    /// Create a new validation failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the ValidationResult</typeparam>
    /// <param name="message">The failure message to return.</param>
    /// <returns></returns>
    public static ValidationResult<TResult> Failure<TResult>(string message) =>
        new(new ValidationFailure<TResult>(ImmutableList<string>.Empty.Add(message)));

    /// <summary>
    /// Create a new validation failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the failure result.</typeparam>
    /// <param name="messages">A list of messages to add to the validation result.</param>
    /// <returns>A new validation result that represents a failure.</returns>
    public static ValidationResult<TResult> Failure<TResult>(ImmutableList<string> messages) =>
        new(new ValidationFailure<TResult>(messages));

}

[ExcludeFromCodeCoverage]
public record ValidationSuccess<T>(T Contents);

[ExcludeFromCodeCoverage]
public record ValidationFailure<T>(ImmutableList<string> FailureMessages);
