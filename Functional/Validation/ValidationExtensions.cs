using Functional.Monadic;
using Functional.Results;

namespace Functional.Validation;

public static class ValidationExtensions
{
	/// <summary>
	/// Bind a previous validation result to a new validation operation. 
	/// To begin validating an object without a validation result, 
	/// see <seealso cref="AsValidationResult{TInput}(TInput)"/>.
	/// </summary>
	/// <typeparam name="TPrevious">The type of the previous validation result contents (when success).</typeparam>
	/// <typeparam name="TInput">The type of the input object to be validated.</typeparam>
	/// <typeparam name="TResult">The type of the output result.</typeparam>
	/// <param name="inputResult">The previous validation result.</param>
	/// <param name="input">The current object to validate.</param>
	/// <param name="validator">The validator function which returns a ValidationResult.</param>
	/// <returns>The new validation result.</returns>
	public static ValidationResult<TResult> Bind<TPrevious, TInput, TResult>(
		this ValidationResult<TPrevious> inputResult,
		TInput input,
		Func<TInput, ValidationResult<TResult>> validator) =>
			validator(input)
				.Match(
					MatchSuccess<TPrevious, TResult>(inputResult),
					MatchFailure<TPrevious, TResult>(inputResult));

	/// <summary>
	/// When the current validation succeeds, handle previous results with the current success.
	/// </summary>
	/// <typeparam name="TInput">The type of the input.</typeparam>
	/// <typeparam name="TResult">The type of the output.</typeparam>
	/// <param name="inputResult">The previous validation results to be combined with the success result.</param>
	/// <returns>A new validation result which may be a success or a failure.</returns>
	private static Func<TResult, ValidationResult<TResult>> MatchSuccess<TInput, TResult>(ValidationResult<TInput> inputResult) =>
		success =>
			inputResult.Match(
				// If the previous results were success, then just return the new contents.
				// The last call to bind will use this to pass the success result with the final object.
				inputSuccess => ValidationResult.Success(success),
				// If the previous result was a failure, pass the failure results through to the next call to bind.
				// Since this validation was a success, there are no messages to add to the failure message list.
				inputFailure => ValidationResult.Failure<TResult>(inputFailure.FailureMessages));

	/// <summary>
	/// When the current validation fails, handle previous results with the current failure.
	/// </summary>
	/// <typeparam name="TInput">The type of the input.</typeparam>
	/// <typeparam name="TResult">The type of the result.</typeparam>
	/// <param name="inputResult">The previous validation results to be combined with the failure result.</param>
	/// <returns>A new validation result which will always be a failiure..</returns>
	private static Func<ValidationFailure<TResult>, ValidationResult<TResult>> MatchFailure<TInput, TResult>(ValidationResult<TInput> inputResult) =>
		failure =>
			inputResult.Match(
				// If the previous validation was a success, then this is a new failure.
				// The failure messages from the current failure need to be added to a new ValidationFailure.
				inputSuccess =>
					ValidationResult.Failure<TResult>(failure.FailureMessages),
				// If the previous validation failed and the current validation failed, there may be multiple
				// error messages. In this case, they need to be combined to return all validation failures back
				// to the client.
				inputFailure =>
					inputFailure
						.FailureMessages
						.AddRange(failure.FailureMessages)
						.Pipe(ValidationResult.Failure<TResult>));

	/// <summary>
	/// Turn any input type into a Success validation result to begin making calls to bind.
	/// </summary>
	/// <typeparam name="TInput">The input type.</typeparam>
	/// <param name="input">The input to wrap with a success result.</param>
	/// <returns>A success result used to start making calls to bind.</returns>
	public static ValidationResult<TInput> AsValidationResult<TInput>(this TInput input) =>
		ValidationResult.Success(input);

	/// <summary>
	/// Convert a validation result into a result of the same type.
	/// </summary>
	/// <typeparam name="TInput">The input type.</typeparam>
	/// <param name="successResult">The validation success result to map.</param>
	/// <returns>A result.</returns>
	public static Result<TInput> MatchSuccess<TInput>(TInput successResult) =>
		successResult
			.Pipe(Result.Success);

	/// <summary>
	/// Convert a validation failure to a Result of the same type.
	/// </summary>
	/// <typeparam name="TResult">The type of the output result.</typeparam>
	/// <param name="failureResult">The validation failure.</param>
	/// <returns>A new result with the contents of the validation failure.</returns>
	public static Result<TInput> MatchFailure<TInput>(ValidationFailure<TInput> failureResult) =>
		failureResult
			.FailureMessages
			.Pipe(Result.Failure<TInput>);

	/// <summary>
	/// Simplify converting validation results to regular results with default behavior.
	/// </summary>
	/// <typeparam name="TInput">The input type.</typeparam>
	/// <param name="validationResult">The validation result to convert.</param>
	/// <returns>A result containing the validation result contents.</returns>
	public static Result<TInput> MatchDefault<TInput>(this ValidationResult<TInput> validationResult) =>
		validationResult
			.Match(
				Result.Success,
				failure => Result.Failure<TInput>(failure.FailureMessages));

	/// <summary>
	/// Match the ValidationResult to either Success or Failure and provide functions to handle each case.
	/// </summary>
	/// <typeparam name="TInput">The input type of the entity.</typeparam>
	/// <typeparam name="TResult">The output type.</typeparam>
	/// <param name="validationResult">The result of validation to be matched.</param>
	/// <param name="whenValid">The fucntion to execute when ValidationSuccess.</param>
	/// <param name="whenInvalid">The function to execute when ValidationFailure.</param>
	/// <returns>The result of the function performed.</returns>
	/// <exception cref="InvalidOperationException">Thrown when any other type pretends to be an 
	/// ValidationResult type other than ValidationSuccess or ValidationFailure.</exception>
	public static TResult Match<TInput, TResult>(
		this ValidationResult<TInput> validationResult,
		Func<TInput, TResult> whenValid,
		Func<ValidationFailure<TInput>, TResult> whenInvalid) =>
			validationResult switch
			{
				ValidationSuccess<TInput> valid => whenValid(valid.Contents),
				ValidationFailure<TInput> invalid => whenInvalid(invalid),
				_ => throw new InvalidOperationException()
			};

	/// <summary>
	/// Match the ValidationResult to either Success or Failure and provide functions to handle each case.
	/// </summary>
	/// <typeparam name="TInput">The input type of the entity.</typeparam>
	/// <typeparam name="TResult">The output type.</typeparam>
	/// <param name="validationResult">The result of validation to be matched.</param>
	/// <param name="whenValid">The fucntion to execute when ValidationSuccess.</param>
	/// <param name="whenInvalid">The function to execute when ValidationFailure.</param>
	/// <returns>The result of the function performed.</returns>
	/// <exception cref="InvalidOperationException">Thrown when any other type pretends to be an 
	/// ValidationResult type other than ValidationSuccess or ValidationFailure.</exception>
	public static async Task<TResult> MatchAsync<TInput, TResult>(
		this Task<ValidationResult<TInput>> validationResult,
		Func<TInput, TResult> whenValid,
		Func<ValidationFailure<TInput>, TResult> whenInvalid) =>
			(await validationResult)
				.Match(whenValid, whenInvalid);

}