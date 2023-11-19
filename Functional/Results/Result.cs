namespace Functional.Results;

/// <summary>
/// A class which represents a success or a failure. 
/// This is the primary way to return error types from a function
/// rather than throwing exceptions.
/// </summary>
/// <typeparam name="Ok">The type of the inner object when Ok.</typeparam>
/// <typeparam name="Error">The type of the inner object when Error.</typeparam>
[ExcludeFromCodeCoverage]
public sealed record Result<Ok, Error>
{
    private Union<OkResult<Ok>, ErrorResult<Error>> Contents { get; init; }

    /// <summary>
    /// Construct a new Result from an Ok.
    /// </summary>
    /// <param name="ok">A result which is Ok.</param>
    public Result(OkResult<Ok> ok) =>
        Contents = new Union<OkResult<Ok>, ErrorResult<Error>>(ok);

    /// <summary>
    /// Construct a new Result from an Error.
    /// </summary>
    /// <param name="error">A result which is an Error.</param>
    public Result(ErrorResult<Error> error) =>
        Contents = new Union<OkResult<Ok>, ErrorResult<Error>>(error);

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenOk">Perform some function on the Ok result.</param>
    /// <param name="whenError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the whenOk or whenError function.</returns>
    public TResult Match<TResult>(Func<Ok, TResult> whenOk, Func<Error, TResult> whenError) =>
        Contents
            .Match(
                ok => whenOk(ok.Contents),
                error => whenError(error.Contents));

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public void Effect(Action<Ok> doWhenOk, Action<Error> doWhenError) =>
        Contents
            .Effect(
                ok => doWhenOk(ok.Contents),
                error => doWhenError(error.Contents));

}

/// <summary>
/// A class which represents a success or a failure. 
/// This is the primary way to return error types from a function
/// rather than throwing exceptions.
/// </summary>
public static class Result
{
    /// <summary>
    /// A result that indicates that the value is Ok.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents.</typeparam>
    /// <typeparam name="Error">The type if it had been an error.</typeparam>
    /// <param name="input">The contents to store.</param>
    /// <returns>A result which indicates that the contents are Ok.</returns>
    public static Result<Ok, Error> Ok<Ok, Error>(this Ok input) =>
        new(new OkResult<Ok>(input));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<Ok, Error> Error<Ok, Error>(Error error) =>
        new(new ErrorResult<Error>(error));

}

/// <summary>
/// A result indicating that the value is Ok.
/// </summary>
/// <typeparam name="Ok">The type of the inner value of the result.</typeparam>
/// <param name="Contents">The wrapped inner value of the Ok.</param>
[ExcludeFromCodeCoverage]
public record OkResult<Ok>(Ok Contents);

/// <summary>
/// A result indicating an Error.
/// </summary>
/// <typeparam name="Error">The type of the inner value of the Error.</typeparam>
/// <param name="Contents">The wrapped inner value of the Error.</param>
[ExcludeFromCodeCoverage]
public record ErrorResult<Error>(Error Contents);
