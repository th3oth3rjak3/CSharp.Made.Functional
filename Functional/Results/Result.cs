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
    private Ok? OkContents { get; }
    private Error? ErrorContents { get; }
    private Union<OkResult<Ok>, ErrorResult<Error>> Union { get; }

    /// <summary>
    /// Construct a new Result from an Ok.
    /// </summary>
    /// <param name="ok">A result which is Ok.</param>
    public Result(OkResult<Ok> ok)
    {
        OkContents = ok.Contents;
        Union = new Union<OkResult<Ok>, ErrorResult<Error>>(ok);
    }

    /// <summary>
    /// Construct a new Result from an Error.
    /// </summary>
    /// <param name="error">A result which is an Error.</param>
    public Result(ErrorResult<Error> error)
    {
        ErrorContents = error.Contents;
        Union = new Union<OkResult<Ok>, ErrorResult<Error>>(error);
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenOk">Perform some function on the Ok result.</param>
    /// <param name="whenError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the whenOk or whenError function.</returns>
    public TResult Match<TResult>(Func<Ok, TResult> whenOk, Func<Error, TResult> whenError) =>
        Union
            .Match(
                ok => whenOk(ok.Contents),
                error => whenError(error.Contents));

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public void Effect(Action<Ok> doWhenOk, Action<Error> doWhenError) =>
        Union
            .Effect(
                ok => doWhenOk(ok.Contents),
                error => doWhenError(error.Contents));

    
    /// <summary>
    /// Determine if the result is ok or an error. True when Ok, otherwise false.
    /// </summary>
    public bool IsOk =>
        Union.Match(_ => true, _ => false);

    /// <summary>
    /// Determine if the result is ok or an error. True when Error, otherwise false.
    /// </summary>
    public bool IsError =>
        Union.Match(_ => false, _ => true);

    /// <summary>
    /// Unwrap is used to get the inner value of a Result when the Result type
    /// is ok. If the result is an error, it will return the default value for the ok type.
    /// <br /><br />
    /// This means that the value will be null for reference types or the standard default value for 
    /// primitive types. For example Result.Error&lt;string, int&gt;(1).Unwrap() will return null since
    /// there was no ok value.
    /// <br /><br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Result is ok using 
    /// <see cref="Result{Ok,Error}.IsOk"/> or <see cref="Result{Ok,Error}.IsError"/>.
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    public Ok? Unwrap() =>
        OkContents;

    /// <summary>
    /// UnwrapError is used to get the inner value of a Result when the Result type
    /// is an error. If the result is ok, it will return the default value for the error type.
    /// <br /><br />
    /// This means that the value will be null for reference types or the standard default value for 
    /// primitive types. For example Result.Ok&lt;string, int&gt;("hello").Unwrap() will return 0 since
    /// there was no error value.
    /// <br /><br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Result is an error using 
    /// <see cref="Result{Ok,Error}.IsOk"/> or <see cref="Result{Ok,Error}.IsError"/>.
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    public Error? UnwrapError() =>
        ErrorContents;
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
    public static Result<Ok, Error> Error<Ok, Error>(this Error error) =>
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
