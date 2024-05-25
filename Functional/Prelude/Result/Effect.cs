namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        Action<TOk> onOk, 
        Action<TError> onError) =>
            (await result)
                .Effect(onOk, onError);

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         () => ErrorResult = "Error");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        Action<TOk> onOk, 
        Action onError) =>
            (await result)
                .Effect(onOk, onError);

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success,
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Action<TOk> onOk,
        Func<TError, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk(theResult.Unwrap());
        if (theResult.IsError) await onError(theResult.UnwrapError());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success, 
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => successResult = success,
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Action<TOk> onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk(theResult.Unwrap());
        if (theResult.IsError) await onError();
        return Unit();
    }
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        Action onOk, 
        Action<TError> onError) =>
            (await result)
                .Effect(onOk, onError);
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        Action onOk, 
        Action onError) =>
            (await result)
                .Effect(onOk, onError);
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Action onOk,
        Func<TError, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk();
        if (theResult.IsError) await onError(theResult.UnwrapError());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => successResult = "success", 
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Action onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk();
        if (theResult.IsError) await onError();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<TOk, Task> onOk,
        Action<TError> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) onError(theResult.UnwrapError());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<TOk, Task> onOk,
        Action onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) onError();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<TOk, Task> onOk,
        Func<TError, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) await onError(theResult.UnwrapError());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         success => Task.Run(() => successResult = success), 
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<TOk, Task> onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) await onError();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         Error => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         Error => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<Task> onOk,
        Action<TError> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) onError(theResult.UnwrapError());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<Task> onOk,
        Action onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) onError();
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<Task> onOk,
        Func<TError, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) await onError(theResult.UnwrapError());
        return Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// Assert.AreEqual(ErrorResult, string.Empty);
    /// 
    /// successResult = string.Empty;
    /// ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectAsync(
    ///         () => Task.Run(() => successResult = "success"), 
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect with.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<Task> onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) await onError();
        return Unit();
    }
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        params Action<TOk>[] onOk) =>
            (await result)
                .EffectOk(onOk);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(() => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        params Action[] onOk) =>
            (await result)
                .EffectOk(onOk);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        params Func<TOk, Task>[] onOk)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await RunSequential(theResult.Unwrap(), onOk)
            : Unit();
    }
    
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(() => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(() => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await RunSequential(onOk)
            : Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        params Action<TError>[] onError) =>
            (await result)
                .EffectError(onError);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(() => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(() => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        params Action[] onError) =>
            (await result)
                .EffectError(onError);

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(() => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(() => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result, 
        params Func<Task>[] onError)
    {
        var theResult = await result;
        return theResult.IsError
            ? await RunSequential(onError)
            : Unit();
    }

    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="TOk">The type when the result is Ok.</typeparam>
    /// <typeparam name="TError">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<TOk, TError>(
        this Task<Result<TOk, TError>> result,
        params Func<TError, Task>[] onError)
    {
        var theResult = await result;
        return theResult.IsError
            ? await RunSequential(theResult.UnwrapError(), onError)
            : Unit();
    }
}