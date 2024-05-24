namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success,
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Action<TSuccess> onSuccess, 
        Action<TFailure> onFailure) =>
            (await result)
                .Effect(onSuccess, onFailure);

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         () => failureResult = "failure");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Action<TSuccess> onSuccess, 
        Action onFailure) =>
            (await result)
                .Effect(onSuccess, onFailure);

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success,
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Action<TSuccess> onSuccess,
        Func<TFailure, Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) onSuccess(theResult.Unwrap());
        if (theResult.IsFailure) await onFailure(theResult.UnwrapFailure());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success,
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Action<TSuccess> onSuccess,
        Func<Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) onSuccess(theResult.Unwrap());
        if (theResult.IsFailure) await onFailure();
        return Unit();
    }
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Action onSuccess, 
        Action<TFailure> onFailure) =>
            (await result)
                .Effect(onSuccess, onFailure);
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Action onSuccess, 
        Action onFailure) =>
            (await result)
                .Effect(onSuccess, onFailure);
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Action onSuccess,
        Func<TFailure, Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) onSuccess();
        if (theResult.IsFailure) await onFailure(theResult.UnwrapFailure());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Action onSuccess,
        Func<Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) onSuccess();
        if (theResult.IsFailure) await onFailure();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TSuccess, Task> onSuccess,
        Action<TFailure> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess(theResult.Unwrap());
        if (theResult.IsFailure) onFailure(theResult.UnwrapFailure());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => failureResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TSuccess, Task> onSuccess,
        Action onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess(theResult.Unwrap());
        if (theResult.IsFailure) onFailure();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TSuccess, Task> onSuccess,
        Func<TFailure, Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess(theResult.Unwrap());
        if (theResult.IsFailure) await onFailure(theResult.UnwrapFailure());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TSuccess, Task> onSuccess,
        Func<Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess(theResult.Unwrap());
        if (theResult.IsFailure) await onFailure();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         failure => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         failure => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<Task> onSuccess,
        Action<TFailure> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess();
        if (theResult.IsFailure) onFailure(theResult.UnwrapFailure());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<Task> onSuccess,
        Action onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess();
        if (theResult.IsFailure) onFailure();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<Task> onSuccess,
        Func<TFailure, Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess();
        if (theResult.IsFailure) await onFailure(theResult.UnwrapFailure());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(failureResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<Task> onSuccess,
        Func<Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess();
        if (theResult.IsFailure) await onFailure();
        return Unit();
    }
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectSuccessAsync(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectSuccessAsync(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Action<TSuccess> onSuccess) =>
            (await result)
                .EffectSuccess(onSuccess);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectSuccessAsync(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectSuccessAsync(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Action onSuccess) =>
            (await result)
                .EffectSuccess(onSuccess);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectSuccessAsync(success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectSuccessAsync(success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TSuccess, Task> onSuccess)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess(theResult.Unwrap());
        return Unit();
    }
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectSuccessAsync(() => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectSuccessAsync(() => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onSuccess">Perform this action when the value is Success.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectSuccessAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Func<Task> onSuccess)
    {
        var theResult = await result;
        if (theResult.IsSuccess) await onSuccess();

        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectFailureAsync(failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectFailureAsync(failure => failureResult = failure.Message);
    ///         
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Action<TFailure> onFailure) =>
            (await result)
                .EffectFailure(onFailure);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectFailureAsync(() => failureResult = "fail");
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectFailureAsync(() => failureResult = "fail");
    ///         
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Action onFailure) =>
            (await result)
                .EffectFailure(onFailure);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectFailureAsync(() => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectFailureAsync(() => Task.Run(() => failureResult = "fail"));
    ///         
    /// Assert.AreEqual(failureResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result, 
        Func<Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsFailure) await onFailure();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Success.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string failureResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectFailureAsync(failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(failureResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("failure!"))
    ///     .Async()
    ///     .EffectFailureAsync(failure => Task.Run(() => failureResult = failure.Message));
    ///         
    /// Assert.AreEqual(failureResult, "failure!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onFailure">Perform this action when the value is Failure.</param>
    /// <typeparam name="TSuccess">The type when the result is Success.</typeparam>
    /// <typeparam name="TFailure">The type when the result is Failure.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectFailureAsync<TSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TFailure, Task> onFailure)
    {
        var theResult = await result;
        if (theResult.IsFailure) await onFailure(theResult.UnwrapFailure());
        return Unit();
    }
}