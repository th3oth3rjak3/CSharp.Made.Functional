using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using OneOf;

namespace Functional.Validation;

[GenerateOneOf]
[ExcludeFromCodeCoverage]
public partial class ValidationResult<T> : OneOfBase<ValidationSuccess<T>, ValidationFailure>
{
    public static implicit operator ValidationResult<T>(T success) =>
        new ValidationSuccess<T>(success);
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
        new ValidationSuccess<TResult>(contents);

    /// <summary>
    /// Create a new validation failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the ValidationResult</typeparam>
    /// <param name="message">The failure message to return.</param>
    /// <returns></returns>
    public static ValidationResult<TResult> Failure<TResult>(string message) =>
        new ValidationFailure(ImmutableList<string>.Empty.Add(message));

    /// <summary>
    /// Create a new validation failure.
    /// </summary>
    /// <typeparam name="TResult">The type of the failure result.</typeparam>
    /// <param name="messages">A list of messages to add to the validation result.</param>
    /// <returns>A new validation result that represents a failure.</returns>
    public static ValidationResult<TResult> Failure<TResult>(ImmutableList<string> messages) =>
        new ValidationFailure(messages);

}

[ExcludeFromCodeCoverage]
public record ValidationSuccess<T>(T Contents);

[ExcludeFromCodeCoverage]
public record ValidationFailure(ImmutableList<string> FailureMessages);
