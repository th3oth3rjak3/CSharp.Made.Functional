﻿namespace Functional;

// TODO: Document this whole file with examples.

/// <summary>
/// A class which represents a success or a failure. 
/// This is the primary way to return error types from a function
/// rather than throwing exceptions.
/// </summary>
/// <typeparam name="TOk">The type of the inner object when Ok.</typeparam>
/// <typeparam name="TError">The type of the inner object when Error.</typeparam>
public sealed record Result<TOk, TError>
{
    /// <summary>
    /// The internal state of the Result.
    /// </summary>
    private enum ResultState
    {
        Ok,
        Error,
    }

    /// <summary>
    /// The internal state of the result.
    /// </summary>
    private readonly ResultState _state;

    /// <summary>
    /// The contents of the result when the result is Ok.
    /// </summary>
    private readonly TOk? _okContents;

    /// <summary>
    /// The contents of the result when the result is an Error.
    /// </summary>
    private readonly TError? _errorContents;

    // TODO: add examples.
    /// <summary>
    /// Construct a new Result.
    /// </summary>
    /// <param name="ok">An instance of the value for when the Result is Ok.</param>
    public Result(TOk ok)
    {
        _okContents = ok;
        _state = ResultState.Ok;
    }

    // TODO: add examples.
    /// <summary>
    /// Construct a new Result.
    /// </summary>
    /// <param name="error">An instance of the value for when the Result is an Error.</param>
    public Result(TError error)
    {
        _errorContents = error;
        _state = ResultState.Error;
    }

    /// <summary>
    /// Determine if the Result is an Ok.
    /// </summary>
    public bool IsOk => _state == ResultState.Ok;

    /// <summary>
    /// Determine if the Result is an Error.
    /// </summary>
    public bool IsError => _state == ResultState.Error;

    /// <summary>
    /// Unwrap is used to get the inner value of a Result when the Result type
    /// is ok. If the result is an error, it will throw an InvalidOperationException.
    /// <br /><br />
    /// For example Result.Error&lt;string, Exception&gt;(new Exception("error")).Unwrap() will throw since
    /// there was no ok value.
    /// <br /><br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Result is ok using 
    /// <see cref="Result{Ok,Error}.IsOk"/> or <see cref="Result{Ok,Error}.IsError"/>.
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result was an error and was unwrapped as ok.</exception>

    public TOk Unwrap()
    {
        if (this.IsOk && _okContents is not null)
        {
            return _okContents;
        }

        throw new InvalidOperationException("Tried to unwrap an Ok when it was not Ok.");
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenOk">Perform some function on the Ok result.</param>
    /// <param name="whenError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the whenOk or whenError function.</returns>
    public TResult Match<TResult>(Func<TOk, TResult> whenOk, Func<TError, TResult> whenError) =>
        IsOk.Match(
            () => whenOk(Unwrap()),
            () => whenError(UnwrapError()));

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public Unit Effect(Action<TOk> doWhenOk, Action<TError> doWhenError)
    {
        if (this.IsOk) doWhenOk(this.Unwrap());
        if (this.IsError) doWhenError(this.UnwrapError());
        return Unit.Default;
    }

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public Unit Effect(Action doWhenOk, Action<TError> doWhenError) =>
        Effect(_ => doWhenOk(), doWhenError);

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public Unit Effect(Action<TOk> doWhenOk, Action doWhenError) =>
        Effect(doWhenOk, _ => doWhenError());

    /// <summary>
    /// Perform a side-effect on a result type.
    /// </summary>
    /// <param name="doWhenOk">Perform this action when the value is Ok.</param>
    /// <param name="doWhenError">Perform this action when the value is Error.</param>
    public Unit Effect(Action doWhenOk, Action doWhenError) =>
        Effect(_ => doWhenOk(), _ => doWhenError());

    /// <summary>
    /// Perform a side effect on a result type when the type is Ok.
    /// </summary>
    /// <param name="doWhenOk">An action to perform on an Ok result.</param>
    public Unit EffectOk(Action<TOk> doWhenOk) =>
        Effect(doWhenOk, _ => { });

    /// <summary>
    /// Perform a side effect on a result type when the type is an error.
    /// </summary>
    /// <param name="doWhenError">An action to perform on an Error result.</param>
    public Unit EffectError(Action<TError> doWhenError) =>
        Effect(_ => { }, doWhenError);


    /// <summary>
    /// UnwrapError is used to get the inner value of a Result when the Result type
    /// is an error. If the result is ok, it will throw an InvalidOperationException.
    /// <br /><br />
    /// For example Result.Ok&lt;string, int&gt;("hello").UnwrapError() will throw since
    /// there was no error value.
    /// <br /><br />
    /// In order to use this safely, it is recommended to first
    /// check to see if the Result is an error using 
    /// <see cref="Result{Ok,Error}.IsOk"/> or <see cref="Result{Ok,Error}.IsError"/>.
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result was an ok value and unwrapped as an error.</exception>
    public TError UnwrapError()
    {
        if (IsError && _errorContents is not null) return _errorContents;

        throw new InvalidOperationException("Tried to unwrap an Error when it was not an Error.");
    }

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<TOk, TError> Tap(Action<TOk> whenOk, Action<TError> whenError)
    {
        if (IsOk && _okContents is not null)
        {
            whenOk(_okContents);
        }

        if (IsError && _errorContents is not null)
        {
            whenError(_errorContents);
        }

        return this;
    }

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<TOk, TError> Tap(Action whenOk, Action<TError> whenError) =>
        Tap(_ => whenOk(), whenError);

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<TOk, TError> Tap(Action<TOk> whenOk, Action whenError) =>
        Tap(whenOk, _ => whenError());

    /// <summary>
    /// Tap into the value while returning it. Perform a side effect with it when it's ok or an error.
    /// </summary>
    /// <param name="whenOk">An action to perform when the value is ok.</param>
    /// <param name="whenError">An action to perform when the value is an error.</param>
    /// <returns>The input.</returns>
    public Result<TOk, TError> Tap(Action whenOk, Action whenError) =>
        Tap(_ => whenOk(), _ => whenError());

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <returns>The input value.</returns>
    public Result<TOk, TError> TapOk(params Action<TOk>[] whenOk)
    {
        if (this.IsOk)
        {
            var contents = this.Unwrap();
            whenOk.ToList().ForEach(action => action(contents));
        }

        return this;
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// </summary>
    /// <param name="whenOk">The action to perform when the value is ok.</param>
    /// <returns>The input value.</returns>
    public Result<TOk, TError> TapOk(params Action[] whenOk)
    {
        if (this.IsOk)
        {
            whenOk.ToList().ForEach(action => action());
        }

        return this;
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is an error.</param>
    /// <returns>The input value.</returns>
    public Result<TOk, TError> TapError(params Action<TError>[] whenError)
    {
        if (this.IsError)
        {
            var contents = this.UnwrapError();
            whenError.ToList().ForEach(action => action(contents));
        }

        return this;
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// </summary>
    /// <param name="whenError">The action to perform when the value is an error.</param>
    /// <returns>The input value.</returns>
    public Result<TOk, TError> TapError(params Action[] whenError)
    {
        if (this.IsError)
        {
            whenError.ToList().ForEach(action => action());
        }

        return this;
    }
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
    /// <typeparam name="TOk">The type of the contents.</typeparam>
    /// <typeparam name="TError">The type if it had been an error.</typeparam>
    /// <param name="input">The contents to store.</param>
    /// <returns>A result which indicates that the contents are Ok.</returns>
    public static Result<TOk, TError> Ok<TOk, TError>(this TOk input) =>
        new(input);

    /// <summary>
    /// A result that indicates that the value is Ok.
    /// </summary>
    /// <typeparam name="TOk">The type of the contents.</typeparam>
    /// <param name="input">The contents to store.</param>
    /// <returns>A result which indicates that the contents are Ok.</returns>
    public static Result<TOk, Exception> Ok<TOk>(this TOk input) =>
        new(input);

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="TOk">The type of the contents when ok.</typeparam>
    /// <typeparam name="TError">The type of the error.</typeparam>
    /// <returns>A result which indicates a failure which contains messages about the failure.</returns>
    public static Result<TOk, TError> Error<TOk, TError>(this TError error) =>
        new(error);

    /// <summary>
    /// A result that indicates failure.
    /// </summary>
    /// <typeparam name="TOk">The type of the contents when ok.</typeparam>
    /// <param name="error">The error.</param>
    /// <returns>A result that indicates a failure.</returns>
    public static Result<TOk, Exception> Error<TOk>(this Exception error) =>
        new(error);

    /// <summary>
    /// A result that constructs an exception on your behalf.
    /// </summary>
    /// <typeparam name="TOk">The type of result if it had been ok.</typeparam>
    /// <param name="errorMessage">A message to use in construction of the exception.</param>
    /// <returns>A result that is an error.</returns>
    public static Result<TOk, Exception> Exception<TOk>(this string errorMessage) =>
        errorMessage
            .Pipe(err => new Exception(err))
            .Pipe(Error<TOk>);


}