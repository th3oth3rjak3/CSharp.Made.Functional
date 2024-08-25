namespace Functional;

/// <summary>
/// A class which represents a success or failure. 
/// This is the primary way to return error types from a function
/// rather than throwing exceptions.
/// </summary>
/// <typeparam name="Ok">The type of the inner object when Ok.</typeparam>
/// <typeparam name="Error">The type of the inner object when Error.</typeparam>
public sealed record Result<Ok, Error>
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
    private readonly ResultState state = default!;

    /// <summary>
    /// The contents of the result when the result is Ok.
    /// </summary>
    private readonly Ok okContents;

    /// <summary>
    /// The contents of the result when the result is Error.
    /// </summary>
    private readonly Error errorContents;

    /// <summary>
    /// Construct a new Result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = new Result&lt;string, Exception&gt;("hello, world!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="ok">An instance of the value for when the Result is Ok.</param>
    public Result(Ok ok)
    {
        okContents = ok;
        errorContents = default!;
        state = ResultState.Ok;
    }

    /// <summary>
    /// Construct a new Result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = new Result&lt;string, Exception&gt;(new Exception("Error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="error">An instance of the value for when the Result is Error.</param>
    public Result(Error error)
    {
        okContents = default!;
        errorContents = error;
        state = ResultState.Error;
    }

    /// <summary>
    /// Allow returns to implicitly be converted to result types.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; DoWork(int input)
    /// {
    ///     if (input &lt; 20) return input.ToString();
    ///     
    ///     return new Exception("Number too large");
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="ok">The value to be converted into a Result.</param>
    public static implicit operator Result<Ok, Error>(Ok ok) =>
        new(ok);

    /// <summary>
    /// Allow returns to implicitly be converted to result types.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; DoWork(int input)
    /// {
    ///     if (input &lt; 20) return input.ToString();
    ///     
    ///     return new Exception("Number too large");
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="error">The Error to be converted into a Result.</param>
    public static implicit operator Result<Ok, Error>(Error error) =>
        new(error);

    /// <summary>
    /// Determine if the Result is Ok.
    /// </summary>
    public bool IsOk => state == ResultState.Ok;

    /// <summary>
    /// Determine if the Result is Error.
    /// </summary>
    public bool IsError => state == ResultState.Error;

    /// <summary>
    /// Unwrap the success contents from the result when it is known to be Ok. 
    /// This will throw an exception if the inner contents are an Error. 
    /// Be sure to check the inner type before unwrapping.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Error&lt;string&gt;(new Exception("something bad happened"));
    /// if (result.IsOk)
    /// {
    ///     // Do something with the contents here.
    ///     string contents = result.Unwrap();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="ResultUnwrapException">Thrown when the result was Error and was unwrapped as Ok.</exception>
    public Ok Unwrap()
    {
        if (IsOk) return okContents;
        throw new ResultUnwrapException();
    }

    /// <summary>
    /// Unwrap the Error contents from the result when it is known to be an Error. 
    /// This will throw an exception if the inner contents are an Ok. 
    /// Be sure to check the inner type before unwrapping.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Ok("successful!");
    /// if (result.IsError)
    /// {
    ///     // Do something with the contents here.
    ///     string errorContents = result.UnwrapError().Message;
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="ResultUnwrapErrorException">Thrown when the result was Ok and was unwrapped as Error.</exception>
    public Error UnwrapError() =>
        IsError
        ? errorContents
        : throw new ResultUnwrapErrorException();

    /// <summary>
    /// Match the result to a Ok or Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Ok&lt;int, int&gt;(1).Match(value => value + 1, err => err - 1);
    /// Assert.AreEqual(value, 2);
    /// 
    /// value = Error&lt;int, int&gt;(0).Match(value => value + 1, err => err - 1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<Ok, TResult> onOk, Func<Error, TResult> onError) =>
        IsOk
            ? onOk(Unwrap())
            : onError(UnwrapError());

    /// <summary>
    /// Match the result to a Ok or Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Ok&lt;int, int&gt;(1).Match(() => 42, err => err - 1);
    /// Assert.AreEqual(value, 42);
    /// 
    /// value = Error&lt;int, int&gt;(0).Match(() => 42, err => err - 1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<TResult> onOk, Func<Error, TResult> onError) =>
        IsOk
            ? onOk()
            : onError(UnwrapError());

    /// <summary>
    /// Match the result to a Ok or Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Ok&lt;int, int&gt;(1).Match(value => value + 1, () => -1);
    /// Assert.AreEqual(value, 2);
    /// 
    /// value = Error&lt;int, int&gt;(0).Match(value => value + 1, () => -1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<Ok, TResult> onOk, Func<TResult> onError) =>
        IsOk
            ? onOk(Unwrap())
            : onError();

    /// <summary>
    /// Match the result to Ok or Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Ok&lt;int, int&gt;(1).Match(() => 42, () => -1);
    /// Assert.AreEqual(value, 42);
    /// 
    /// value = Error&lt;int, int&gt;(0).Match(() => 42, () => -1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<TResult> onOk, Func<TResult> onError) =>
        IsOk
            ? onOk()
            : onError();

    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Ok(42).Map(value => value.ToString());
    /// Assert.AreEqual(mapped.Unwrap(), "42");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public Result<TMappedOk, Error> Map<TMappedOk>(Func<Ok, TMappedOk> mapper) =>
        IsOk
            ? new Result<TMappedOk, Error>(mapper(Unwrap()))
            : new Result<TMappedOk, Error>(UnwrapError());

    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Ok(42).Map(() => "some alternate value");
    /// Assert.AreEqual(mapped.Unwrap(), "some alternate value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public Result<TMappedOk, Error> Map<TMappedOk>(Func<TMappedOk> mapper) =>
        IsOk
            ? new Result<TMappedOk, Error>(mapper())
            : new Result<TMappedOk, Error>(UnwrapError());

    /// <summary>
    /// Map a result with one Error type to another.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Error&lt;int, string&gt;("error").MapError(msg => new Exception(msg));
    /// Assert.AreEqual(mapped.UnwrapError().Message, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="errorMapper">A function to transform one Error to another.</param>
    /// <typeparam name="TMappedError">The type for the new Error.</typeparam>
    /// <returns>A result with a mapped Error.</returns>
    public Result<Ok, TMappedError> MapError<TMappedError>(Func<Error, TMappedError> errorMapper) =>
        IsOk
            ? new Result<Ok, TMappedError>(Unwrap())
            : new Result<Ok, TMappedError>(errorMapper(UnwrapError()));

    /// <summary>
    /// Map a result with one Error type to another.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Error&lt;int, string&gt;("error").MapError(() => new Exception("unknown error occurred"));
    /// Assert.AreEqual(mapped.UnwrapError().Message, "unknown error occurred");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="errorMapper">A function to transform one Error to another.</param>
    /// <typeparam name="TMappedError">The type for the new Error.</typeparam>
    /// <returns>A result with a mapped Error.</returns>
    public Result<Ok, TMappedError> MapError<TMappedError>(Func<TMappedError> errorMapper) =>
        IsOk
            ? new Result<Ok, TMappedError>(Unwrap())
            : new Result<Ok, TMappedError>(errorMapper());

    /// <summary>
    /// When the result is Ok, return its contents, otherwise return an alternate value discarding the error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Reduce(0)
    ///     .Effect(output => Assert.AreEqual(output, 42));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Reduce(0)
    ///     .Effect(output => Assert.AreEqual(output, 0);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public Ok Reduce(Ok alternate) =>
        IsOk
            ? Unwrap()
            : alternate;

    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value discarding the error.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Reduce(() => 0)
    ///     .Effect(output => Assert.AreEqual(output, 42));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Reduce(() => 0)
    ///     .Effect(output => Assert.AreEqual(output, 0));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When Ok, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public Ok Reduce(Func<Ok> alternate) =>
        IsOk
            ? Unwrap()
            : alternate();

    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value using the error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok&lt;string, Exception&gt;("hello, world")
    ///     .Reduce(err => err.Message)
    ///     .Effect(output => Assert.AreEqual(output, "hello, world"));
    ///
    /// Error&lt;string, Exception&gt;(new Exception("error"))
    ///     .Reduce(err => err.Message)
    ///     .Effect(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="alternate">A function which uses an Error 
    /// result to return an alternate.</param>
    /// <returns>When Ok, the contents, otherwise the return
    /// value of the alternate.</returns>
    public Ok Reduce(Func<Error, Ok> alternate) =>
        IsOk
            ? Unwrap()
            : alternate(UnwrapError());

    /// <summary>
    /// Perform work on a previous result. When the result is Ok,
    /// perform work on the result by providing a function.
    /// On Error, the previous Error will be returned as the new result type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, string&gt; bindingFunction(int input) => 
    ///     input &gt; 10 
    ///         ? Ok&lt;int, string&gt;(input)
    ///         : Error&lt;int, string&gt;("the value was too low");
    ///         
    /// var result = Ok&lt;int, string&gt;(42).Bind(bindingFunction);
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(result.Unwrap(), 42);
    /// 
    /// result = Ok&lt;int, string&gt;(5).Bind(bindingFunction);
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(result.UnwrapError(), "the value was too low");
    ///     
    /// result = Error("Error happened before binding").Bind(bindingFunction);
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(result.UnwrapError(), "Error happened before binding");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedOk">The type of the result after performing 
    /// the binding function.</typeparam>
    /// <param name="binder">The function to perform when the 
    /// previous result is Ok.</param>
    /// <returns>The result of the bind operation.</returns>
    public Result<TMappedOk, Error> Bind<TMappedOk>(Func<Ok, Result<TMappedOk, Error>> binder) =>
        IsOk
            ? binder(Unwrap())
            : new Result<TMappedOk, Error>(UnwrapError());

    /// <summary>
    /// Perform work on a previous result. When the result is Ok,
    /// perform work on the result by providing a function.
    /// On Error, the previous Error will be returned as the new result type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, string&gt; bindingFunction() => 
    ///     new Random().Next(100) &gt; 10 
    ///         ? Ok&lt;int, string&gt;(input)
    ///         : Error&lt;int, string&gt;("the value was too low");
    ///         
    /// // This may be success or Error depending on the Random number generator.
    /// var result = Ok&lt;int, string&gt;(42).Bind(bindingFunction);
    ///     
    /// result = Error("Error happened before binding").Bind(bindingFunction);
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(result.UnwrapError(), "Error happened before binding");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedOk">The type of the result after performing 
    /// the binding function.</typeparam>
    /// <param name="binder">The function to perform when the 
    /// previous result is Ok.</param>
    /// <returns>The result of the bind operation.</returns>
    public Result<TMappedOk, Error> Bind<TMappedOk>(Func<Result<TMappedOk, Error>> binder) =>
        IsOk
            ? binder()
            : new Result<TMappedOk, Error>(UnwrapError());

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         success => successResult = success, 
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Effect(
    ///         success => successResult = success,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action<Ok> onOk, Action<Error> onError)
    {
        if (IsOk) onOk(Unwrap());
        if (IsError) onError(UnwrapError());
        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         () => successResult = "success", 
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Effect(
    ///         () => successResult = "success",
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action onOk, Action<Error> onError)
    {
        if (IsOk) onOk();
        if (IsError) onError(UnwrapError());

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         success => successResult = success, 
    ///         () => ErrorResult = "Error");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Effect(
    ///         success => successResult = success,
    ///         () => ErrorResult = "Error");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action<Ok> onOk, Action onError)
    {
        if (IsOk) onOk(Unwrap());
        if (IsError) onError();

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         () => successResult = "success", 
    ///         () => ErrorResult = "Error");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Effect(
    ///         () => successResult = "success", 
    ///         () => ErrorResult = "Error");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action onOk, Action onError)
    {
        if (IsOk) onOk();
        if (IsError) onError();

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .EffectOk(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectOk(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <returns>Unit.</returns>
    public Unit EffectOk(params Action<Ok>[] onOk)
    {
        if (IsError) return Unit.Default;
        var contents = Unwrap();
        onOk.ToList().ForEach(action => action(contents));
        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .EffectOk(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectOk(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <returns>Unit.</returns>
    public Unit EffectOk(params Action[] onOk)
    {
        if (IsOk) onOk.ToList().ForEach(action => action());
        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectError(Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .EffectError(Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>Unit.</returns>
    public Unit EffectError(params Action<Error>[] onError)
    {
        if (IsOk) return Unit.Default;
        var contents = UnwrapError();
        onError.ToList().ForEach(action => action(contents));
        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectError(() => ErrorResult = "error");
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .EffectError(() => ErrorResult = "error"));
    ///         
    /// Assert.AreEqual(ErrorResult, "error");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>Unit.</returns>
    public Unit EffectError(params Action[] onError)
    {
        if (IsError) onError.ToList().ForEach(action => action());
        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             success => successResult = success, 
    ///             Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .Tap(
    ///             success => successResult = success,
    ///             Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> Tap(Action<Ok> onOk, Action<Error> onError)
    {
        if (IsOk) onOk(Unwrap());
        if (IsError) onError(UnwrapError());

        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             () => successResult = "success", 
    ///             Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .Tap(
    ///             () => successResult = "success",
    ///             Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> Tap(Action onOk, Action<Error> onError)
    {
        if (IsOk) onOk();
        if (IsError) onError(UnwrapError());

        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             success => successResult = success, 
    ///             () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .Tap(
    ///             success => successResult = success,
    ///             () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> Tap(Action<Ok> onOk, Action onError)
    {
        if (IsOk) onOk(Unwrap());
        if (IsError) onError();

        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             () => successResult = "success", 
    ///             () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .Tap(
    ///             () => successResult = "success", 
    ///             () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> Tap(Action onOk, Action onError)
    {
        if (IsOk) onOk();
        if (IsError) onError();
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapOk(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// successResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .TapOk(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> TapOk(params Action<Ok>[] onOk)
    {
        if (IsError) return this;
        var contents = Unwrap();
        onOk.ToList().ForEach(action => action(contents));
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapOk(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// successResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .TapOk(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> TapOk(params Action[] onOk)
    {
        if (IsError) return this;
        onOk.ToList().ForEach(action => action());
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapError(exn => ErrorResult = exn.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .TapError(exn => ErrorResult = exn.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> TapError(params Action<Error>[] onError)
    {
        if (IsOk) return this;
        var contents = UnwrapError();
        onError.ToList().ForEach(action => action(contents));
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapError(() => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// Assert.IsTrue(result.IsOk);
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///         .TapError(() => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// Assert.IsTrue(result.IsError);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <returns>The input result.</returns>
    public Result<Ok, Error> TapError(params Action[] onError)
    {
        if (IsOk) return this;
        onError.ToList().ForEach(action => action());
        return this;
    }
}