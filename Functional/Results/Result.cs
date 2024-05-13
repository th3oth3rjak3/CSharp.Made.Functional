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
    public Unit Effect(Action<Ok> doWhenOk, Action<Error> doWhenError) =>
        Union
            .Effect(
                ok => doWhenOk(ok.Contents),
                error => doWhenError(error.Contents));
    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public Unit Effect(Action doWhenOk, Action<Error> doWhenError) =>
        Union
            .Effect(
                _ => doWhenOk(),
                error => doWhenError(error.Contents));
    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public Unit Effect(Action<Ok> doWhenOk, Action doWhenError) =>
        Union
            .Effect(
                ok => doWhenOk(ok.Contents),
                _ => doWhenError());
    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public Unit Effect(Action doWhenOk, Action doWhenError) =>
        Union
            .Effect(
                _ => doWhenOk(),
                _ => doWhenError());

    /// <summary>
    /// Perform a side effect on a result type when the type is Ok.
    /// </summary>
    /// <param name="doWhenOk">An action to perform on an Ok result.</param>
    public Unit EffectOk(Action<Ok> doWhenOk) =>
        Union
            .Effect(
                ok => doWhenOk(ok.Contents),
                _ => { });

    /// <summary>
    /// Perform a side effect on a result type when the type is an error.
    /// </summary>
    /// <param name="doWhenError">An action to perform on an Error result.</param>
    public Unit EffectError(Action<Error> doWhenError) =>
        Union
            .Effect(
                _ => { },
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

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<Ok, Error> Tap(Action<Ok> whenOk, Action<Error> whenError)
    {
        if (IsOk)
        {
            whenOk(OkContents!);
        }
        else
        {
            whenError(ErrorContents!);
        }

        return this;
    }

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<Ok, Error> Tap(Action whenOk, Action<Error> whenError) =>
        Tap(_ => whenOk(), whenError);

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<Ok, Error> Tap(Action<Ok> whenOk, Action whenError) =>
        Tap(whenOk, _ => whenError());

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<Ok, Error> Tap(Action whenOk, Action whenError) =>
        Tap(_ => whenOk(), _ => whenError());

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <returns>The input value.</returns>
    public Result<Ok, Error> TapOk(Action<Ok> whenOk) =>
        Tap(whenOk, _ => { });

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <returns>The input value.</returns>
    public Result<Ok, Error> TapOk(Action whenOk) =>
        TapOk(_ => whenOk());

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is an error.</param>
    /// <returns>The input value.</returns>
    public Result<Ok, Error> TapError(Action<Error> whenError) =>
        Tap(_ => { }, whenError);

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is an error.</param>
    /// <returns>The input value.</returns>
    public Result<Ok, Error> TapError(Action whenError) =>
        TapError(_ => whenError());
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
    /// A result that indicates that the value is Ok.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents.</typeparam>
    /// <param name="input">The contents to store.</param>
    /// <returns>A result which indicates that the contents are Ok.</returns>
    public static Result<Ok, Exception> Ok<Ok>(this Ok input) =>
        new(new OkResult<Ok>(input));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents when ok.</typeparam>
    /// <typeparam name="Error">The type of the error.</typeparam>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<Ok, Error> Error<Ok, Error>(this Error error) =>
        new(new ErrorResult<Error>(error));

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="Ok">The type of the contents when ok.</typeparam>
    /// <param name="error">The error.</param>
    /// <returns>A result that indicates a failure.</returns>
    public static Result<Ok, Exception> Error<Ok>(this Exception error) =>
        new(new ErrorResult<Exception>(error));

    /// <summary>
    /// A result that constructs an exception on your behalf.
    /// </summary>
    /// <typeparam name="Ok">The type of result if it had been ok.</typeparam>
    /// <param name="errorMessage">A message to use in construction of the exception.</param>
    /// <returns>A result that is an error.</returns>
    public static Result<Ok, Exception> Exception<Ok>(this string errorMessage) =>
        errorMessage
            .Pipe(err => new Exception(err))
            .Pipe(Result.Error<Ok>);


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
