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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
        Action<Error> onError) =>
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
        Func<Error, Task> onError)
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action onOk,
        Action<Error> onError) =>
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action onOk,
        Func<Error, Task> onError)
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
        Action<Error> onError)
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
        Func<Error, Task> onError)
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task> onOk,
        Action<Error> onError)
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task> onOk,
        Func<Error, Task> onError)
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action<Ok>[] onOk) =>
            (await result)
                .EffectOk(onOk);

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">Processing order, parallel or sequential.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action<Ok>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk),
                () => RunSequential(theResult.Unwrap(), onOk));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action<Ok>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await RunSequential(theResult.Unwrap(), onOk, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         success => successResult = success);
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">Processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action<Ok>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk, cancellationToken),
                () => RunSequential(theResult.Unwrap(), onOk, cancellationToken));
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action[] onOk) =>
            (await result)
                .EffectOk(onOk);

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">Processing order, parallel or sequential.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk),
                () => RunSequential(onOk));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         () => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         () => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await RunSequential(onOk, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => successResult = "success");
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">Processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk, cancellationToken),
                () => RunSequential(onOk, cancellationToken));
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await RunSequential(theResult.Unwrap(), onOk)
            : Unit();
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">Processing order, parallel or sequential.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk),
                () => RunSequential(theResult.Unwrap(), onOk));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await RunSequential(theResult.Unwrap(), onOk, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         success => Task.Run(() => successResult = success));
    ///         
    /// Assert.AreEqual(successResult, "hello, world!");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">Processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk, cancellationToken),
                () => RunSequential(theResult.Unwrap(), onOk, cancellationToken));
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await RunSequential(onOk)
            : Unit();
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">Processing order, parallel or sequential.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk),
                () => RunSequential(onOk));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await RunSequential(onOk, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string successResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, string.Empty);
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => successResult = "success"));
    ///         
    /// Assert.AreEqual(successResult, "success");
    /// 
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">Perform this action when the value is Ok.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk, cancellationToken),
                () => RunSequential(onOk, cancellationToken));
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action<Error>[] onError) =>
            (await result)
                .EffectError(onError);

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action<Error>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError),
                () => RunSequential(theResult.UnwrapError(), onError));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action<Error>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await RunSequential(theResult.UnwrapError(), onError, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         Error => ErrorResult = Error.Message);
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action<Error>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError, cancellationToken),
                () => RunSequential(theResult.UnwrapError(), onError, cancellationToken));
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action[] onError) =>
            (await result)
                .EffectError(onError);

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError),
                () => RunSequential(onError));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await RunSequential(onError, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => ErrorResult = "fail");
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError, cancellationToken),
                () => RunSequential(onError, cancellationToken));
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        return theResult.IsError
            ? await RunSequential(onError)
            : Unit();
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError),
                () => RunSequential(onError));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await RunSequential(onError, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => ErrorResult = "fail"));
    ///         
    /// Assert.AreEqual(ErrorResult, "fail");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError, cancellationToken),
                () => RunSequential(onError, cancellationToken));
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
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        return theResult.IsError
            ? await RunSequential(theResult.UnwrapError(), onError)
            : Unit();
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError),
                () => RunSequential(theResult.UnwrapError(), onError));
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         CancellationToken.None,
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await RunSequential(theResult.UnwrapError(), onError, cancellationToken);
    }

    // todo: tests
    /// <summary>
    /// Perform a side effect on a result type and consume the result when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string ErrorResult = string.Empty;
    /// 
    /// await new Result&lt;string, Exception&gt;("hello, world!")
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, string.Empty);
    ///
    /// await new Result&lt;string, Exception&gt;(new Exception("Error!"))
    ///     .Async()
    ///     .EffectErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         Error => Task.Run(() => ErrorResult = Error.Message));
    ///         
    /// Assert.AreEqual(ErrorResult, "Error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to perform the effect on.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">Perform this action when the value is Error.</param>
    /// <typeparam name="Ok">The type when the result is Ok.</typeparam>
    /// <typeparam name="Error">The type when the result is Error.</typeparam>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return Unit();
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError, cancellationToken),
                () => RunSequential(theResult.UnwrapError(), onError, cancellationToken));
    }
}