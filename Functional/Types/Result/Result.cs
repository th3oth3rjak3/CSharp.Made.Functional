namespace Functional;

/// <summary>
/// A class which represents a success or a failure. 
/// This is the primary way to return error types from a function
/// rather than throwing exceptions.
/// </summary>
/// <typeparam name="TSuccess">The type of the inner object when Success.</typeparam>
/// <typeparam name="TFailure">The type of the inner object when Failure.</typeparam>
public sealed record Result<TSuccess, TFailure>
{
    /// <summary>
    /// The internal state of the Result.
    /// </summary>
    private enum ResultState
    {
        Success,
        Failure,
    }

    /// <summary>
    /// The internal state of the result.
    /// </summary>
    private readonly ResultState state = default!;

    /// <summary>
    /// The contents of the result when the result is Success.
    /// </summary>
    private readonly TSuccess successContents = default!;

    /// <summary>
    /// The contents of the result when the result is Failure.
    /// </summary>
    private readonly TFailure failureContents;

    /// <summary>
    /// Construct a new Result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = new Result&lt;string, Exception&gt;("hello, world!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="success">An instance of the value for when the Result is Success.</param>
    public Result(TSuccess success)
    {
        successContents = success;
        failureContents = default!;
        state = ResultState.Success;
    }

    /// <summary>
    /// Construct a new Result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = new Result&lt;string, Exception&gt;(new Exception("failure"));
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="failure">An instance of the value for when the Result is Failure.</param>
    public Result(TFailure failure)
    {
        successContents = default!;
        failureContents = failure;
        state = ResultState.Failure;
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
    /// <param name="success">The success to be converted into a Result.</param>
    public static implicit operator Result<TSuccess, TFailure>(TSuccess success) =>
        new(success);

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
    /// <param name="failure">The failure to be converted into a Result.</param>
    public static implicit operator Result<TSuccess, TFailure>(TFailure failure) =>
        new(failure);

    /// <summary>
    /// Determine if the Result is Success.
    /// </summary>
    public bool IsSuccess => state == ResultState.Success;

    /// <summary>
    /// Determine if the Result is Failure.
    /// </summary>
    public bool IsFailure => state == ResultState.Failure;

    /// <summary>
    /// Unwrap the success contents from the result when it is known to be Success. 
    /// This will throw an exception if the inner contents are a Failure. 
    /// Be sure to check the inner type before unwrapping.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Failure&lt;string&gt;(new Exception("something bad happened"));
    /// if (result.IsSuccess)
    /// {
    ///     // Do something with the contents here.
    ///     string contents = result.Unwrap();
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result was Failure and was unwrapped as Success.</exception>
    public TSuccess Unwrap()
    {
        if (IsSuccess) return successContents;

        throw new InvalidOperationException("Tried to unwrap Success when it was Failure.");
    }

    /// <summary>
    /// Unwrap the failure contents from the result when it is known to be a Failure. 
    /// This will throw an InvalidOperationException if the inner contents are a Success. 
    /// Be sure to check the inner type before unwrapping.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; result = Success("successful!");
    /// if (result.IsFailure)
    /// {
    ///     // Do something with the contents here.
    ///     string errorContents = result.UnwrapFailure().Message;
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <returns>The inner value of the result.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the result was Success and was unwrapped as Failure.</exception>
    public TFailure UnwrapFailure()
    {
        if (IsFailure) return failureContents;

        throw new InvalidOperationException("Tried to unwrap Failure when it was Success.");
    }

    /// <summary>
    /// Match the result to a Success or Failure and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Success&lt;int, int&gt;(1).Match(value => value + 1, err => err - 1);
    /// Assert.AreEqual(value, 2);
    /// 
    /// value = Failure&lt;int, int&gt;(0).Match(value => value + 1, err => err - 1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSuccess">Perform some function on the Success result.</param>
    /// <param name="whenFailure">Perform some function on the Failure result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<TSuccess, TResult> whenSuccess, Func<TFailure, TResult> whenFailure) =>
        IsSuccess
            ? whenSuccess(Unwrap())
            : whenFailure(UnwrapFailure());

    /// <summary>
    /// Match the result to a Success or Failure and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Success&lt;int, int&gt;(1).Match(() => 42, err => err - 1);
    /// Assert.AreEqual(value, 42);
    /// 
    /// value = Failure&lt;int, int&gt;(0).Match(() => 42, err => err - 1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSuccess">Perform some function on the Success result.</param>
    /// <param name="whenFailure">Perform some function on the Failure result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<TResult> whenSuccess, Func<TFailure, TResult> whenFailure) =>
        IsSuccess
            ? whenSuccess()
            : whenFailure(UnwrapFailure());

    /// <summary>
    /// Match the result to a Success or Failure and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Success&lt;int, int&gt;(1).Match(value => value + 1, () => -1);
    /// Assert.AreEqual(value, 2);
    /// 
    /// value = Failure&lt;int, int&gt;(0).Match(value => value + 1, () => -1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSuccess">Perform some function on the Success result.</param>
    /// <param name="whenFailure">Perform some function on the Failure result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<TSuccess, TResult> whenSuccess, Func<TResult> whenFailure) =>
        IsSuccess
            ? whenSuccess(Unwrap())
            : whenFailure();

    /// <summary>
    /// Match the result to a Success or Failure and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int value = Success&lt;int, int&gt;(1).Match(() => 42, () => -1);
    /// Assert.AreEqual(value, 42);
    /// 
    /// value = Failure&lt;int, int&gt;(0).Match(() => 42, () => -1);
    /// Assert.AreEqual(value, -1);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="whenSuccess">Perform some function on the Success result.</param>
    /// <param name="whenFailure">Perform some function on the Failure result.</param>
    /// <returns>The result of executing the appropriate mapping function.</returns>
    public TResult Match<TResult>(Func<TResult> whenSuccess, Func<TResult> whenFailure) =>
        IsSuccess
            ? whenSuccess()
            : whenFailure();

    /// <summary>
    /// Map a Success result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Success(42).Map(value => value.ToString());
    /// Assert.AreEqual(mapped.Unwrap(), "42");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedSuccess">The type of the converted input.</typeparam>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public Result<TMappedSuccess, TFailure> Map<TMappedSuccess>(Func<TSuccess, TMappedSuccess> mapper) =>
        IsSuccess
            ? new Result<TMappedSuccess, TFailure>(mapper(Unwrap()))
            : new Result<TMappedSuccess, TFailure>(UnwrapFailure());

    /// <summary>
    /// Map a Success result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Success(42).Map(() => "some alternate value");
    /// Assert.AreEqual(mapped.Unwrap(), "some alternate value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedSuccess">The type of the converted input.</typeparam>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public Result<TMappedSuccess, TFailure> Map<TMappedSuccess>(Func<TMappedSuccess> mapper) =>
        IsSuccess
            ? new Result<TMappedSuccess, TFailure>(mapper())
            : new Result<TMappedSuccess, TFailure>(UnwrapFailure());

    /// <summary>
    /// Map a result with one failure type to another.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Failure&lt;int, string&gt;("error").MapFailure(msg => new Exception(msg));
    /// Assert.AreEqual(mapped.UnwrapFailure().Message, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="failureMapper">A function to transform one failure to another.</param>
    /// <typeparam name="TMappedFailure">The type for the new failure.</typeparam>
    /// <returns>A result with a mapped failure.</returns>
    public Result<TSuccess, TMappedFailure> MapFailure<TMappedFailure>(Func<TFailure, TMappedFailure> failureMapper) =>
        IsSuccess
            ? new Result<TSuccess, TMappedFailure>(Unwrap())
            : new Result<TSuccess, TMappedFailure>(failureMapper(UnwrapFailure()));

    /// <summary>
    /// Map a result with one failure type to another.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = Failure&lt;int, string&gt;("error").MapFailure(() => new Exception("unknown error occurred"));
    /// Assert.AreEqual(mapped.UnwrapFailure().Message, "unknown error occurred");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="failureMapper">A function to transform one failure to another.</param>
    /// <typeparam name="TMappedFailure">The type for the new failure.</typeparam>
    /// <returns>A result with a mapped failure.</returns>
    public Result<TSuccess, TMappedFailure> MapFailure<TMappedFailure>(Func<TMappedFailure> failureMapper) =>
        IsSuccess
            ? new Result<TSuccess, TMappedFailure>(Unwrap())
            : new Result<TSuccess, TMappedFailure>(failureMapper());

    /// <summary>
    /// Perform work on a previous result. When the result is Success,
    /// perform work on the result by providing a function.
    /// On failure, the previous failure will be returned as the new result type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, string&gt; bindingFunction(int input) => 
    ///     input &gt; 10 
    ///         ? Success&lt;int, string&gt;(input)
    ///         : Failure&lt;int, string&gt;("the value was too low");
    ///         
    /// var result = Success&lt;int, string&gt;(42).Bind(bindingFunction);
    /// Assert.IsTrue(result.IsSuccess);
    /// Assert.AreEqual(result.Unwrap(), 42);
    /// 
    /// result = Success&lt;int, string&gt;(5).Bind(bindingFunction);
    /// Assert.IsTrue(result.IsFailure);
    /// Assert.AreEqual(result.UnwrapFailure(), "the value was too low");
    ///     
    /// result = Failure("failure happened before binding").Bind(bindingFunction);
    /// Assert.IsTrue(result.IsFailure);
    /// Assert.AreEqual(result.UnwrapFailure(), "failure happened before binding");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedSuccess">The type of the result after performing 
    /// the binding function.</typeparam>
    /// <param name="binder">The function to perform when the 
    /// previous result is Success.</param>
    /// <returns>The result of the bind operation.</returns>
    public Result<TMappedSuccess, TFailure> Bind<TMappedSuccess>(Func<TSuccess, Result<TMappedSuccess, TFailure>> binder) =>
        IsSuccess
            ? binder(Unwrap())
            : new Result<TMappedSuccess, TFailure>(UnwrapFailure());

    /// <summary>
    /// Perform work on a previous result. When the result is Success,
    /// perform work on the result by providing a function.
    /// On failure, the previous failure will be returned as the new result type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, string&gt; bindingFunction() => 
    ///     new Random().Next(100) &gt; 10 
    ///         ? Success&lt;int, string&gt;(input)
    ///         : Failure&lt;int, string&gt;("the value was too low");
    ///         
    /// // This may be success or failure depending on the Random number generator.
    /// var result = Success&lt;int, string&gt;(42).Bind(bindingFunction);
    ///     
    /// result = Failure("failure happened before binding").Bind(bindingFunction);
    /// Assert.IsTrue(result.IsFailure);
    /// Assert.AreEqual(result.UnwrapFailure(), "failure happened before binding");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TMappedSuccess">The type of the result after performing 
    /// the binding function.</typeparam>
    /// <param name="binder">The function to perform when the 
    /// previous result is Success.</param>
    /// <returns>The result of the bind operation.</returns>
    public Result<TMappedSuccess, TFailure> Bind<TMappedSuccess>(Func<Result<TMappedSuccess, TFailure>> binder) =>
        IsSuccess
            ? binder()
            : new Result<TMappedSuccess, TFailure>(UnwrapFailure());

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         success => successResult = success, 
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Effect(
    ///         success => successResult = success,
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action<TSuccess> onSuccess, Action<TFailure> onFailure)
    {
        if (IsSuccess) onSuccess(Unwrap());
        if (IsFailure) onFailure(UnwrapFailure());
        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         () => successResult = "success", 
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Effect(
    ///         () => successResult = "success",
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action onSuccess, Action<TFailure> onFailure)
    {
        if (IsSuccess) onSuccess();
        if (IsFailure) onFailure(UnwrapFailure());

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         success => successResult = success, 
    ///         () => failureResult = "failure");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Effect(
    ///         success => successResult = success,
    ///         () => failureResult = "failure");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action<TSuccess> onSuccess, Action onFailure)
    {
        if (IsSuccess) onSuccess(Unwrap());
        if (IsFailure) onFailure();

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Effect(
    ///         () => successResult = "success", 
    ///         () => failureResult = "failure");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Effect(
    ///         () => successResult = "success", 
    ///         () => failureResult = "failure");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>Unit.</returns>
    public Unit Effect(Action onSuccess, Action onFailure)
    {
        if (IsSuccess) onSuccess();
        if (IsFailure) onFailure();

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .EffectSuccess(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectSuccess(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <returns>Unit.</returns>
    public Unit EffectSuccess(Action<TSuccess> onSuccess)
    {
        if (IsSuccess) onSuccess(Unwrap());

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .EffectSuccess(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectSuccess(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <returns>Unit.</returns>
    public Unit EffectSuccess(Action onSuccess)
    {
        if (IsSuccess) onSuccess();

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Failure.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectFailure(failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .EffectFailure(failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(failureResult, "failure!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>Unit.</returns>
    public Unit EffectFailure(Action<TFailure> onFailure)
    {
        if (IsFailure) onFailure(UnwrapFailure());

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Failure.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// new Result&lt;string, Exception&gt;("hello, world!")
    ///     .EffectFailure(() => failureResult = "error");
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .EffectFailure(() => failureResult = "error"));
    ///         
    /// Assert.AreEqual(failureResult, "error");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>Unit.</returns>
    public Unit EffectFailure(Action onFailure)
    {
        if (IsFailure) onFailure();

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             success => successResult = success, 
    ///             failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .Tap(
    ///             success => successResult = success,
    ///             failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> Tap(Action<TSuccess> onSuccess, Action<TFailure> onFailure)
    {
        if (IsSuccess) onSuccess(Unwrap());
        if (IsFailure) onFailure(UnwrapFailure());

        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             () => successResult = "success", 
    ///             failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .Tap(
    ///             () => successResult = "success",
    ///             failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> Tap(Action onSuccess, Action<TFailure> onFailure)
    {
        if (IsSuccess) onSuccess();
        if (IsFailure) onFailure(UnwrapFailure());

        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             success => successResult = success, 
    ///             () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .Tap(
    ///             success => successResult = success,
    ///             () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> Tap(Action<TSuccess> onSuccess, Action onFailure)
    {
        if (IsSuccess) onSuccess(Unwrap());
        if (IsFailure) onFailure();

        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .Tap(
    ///             () => successResult = "success", 
    ///             () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .Tap(
    ///             () => successResult = "success", 
    ///             () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> Tap(Action onSuccess, Action onFailure)
    {
        if (IsSuccess) onSuccess();
        if (IsFailure) onFailure();
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapSuccess(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// successResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .TapSuccess(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> TapSuccess(params Action<TSuccess>[] onSuccess)
    {
        if (IsFailure) return this;
        var contents = Unwrap();
        onSuccess.ToList().ForEach(action => action(contents));
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapSuccess(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// successResult = string.Empty;
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .TapSuccess(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> TapSuccess(params Action[] onSuccess)
    {
        if (IsFailure) return this;
        onSuccess.ToList().ForEach(action => action());
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Failure.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapFailure(exn => failureResult = exn.Message);
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .TapFailure(exn => failureResult = exn.Message);
    ///         
    /// Assert.AreEqual(failureResult, "failure!");
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> TapFailure(params Action<TFailure>[] onFailure)
    {
        if (IsSuccess) return this;
        var contents = UnwrapFailure();
        onFailure.ToList().ForEach(action => action(contents));
        return this;
    }

    /// <summary>
    /// Perform a side effect on a result type without consuming the result when the result is Failure.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// Result&lt;string, Exception&gt; result = 
    ///     new Result&lt;string, Exception&gt;("hello, world!")
    ///         .TapFailure(() => failureResult = "fail");
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    /// Assert.IsTrue(result.IsSuccess);
    /// 
    /// result = 
    ///     new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///         .TapFailure(() => failureResult = "fail");
    ///         
    /// Assert.AreEqual(failureResult, "fail");
    /// Assert.IsTrue(result.IsFailure);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <returns>The input result.</returns>
    public Result<TSuccess, TFailure> TapFailure(params Action[] onFailure)
    {
        if (IsSuccess) return this;
        onFailure.ToList().ForEach(action => action());
        return this;
    }
}