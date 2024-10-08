﻿namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
        Action<Error> onError) =>
            (await result).Tap(onOk, onError);

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
        Action onError) =>
            (await result).Tap(onOk, onError);

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk(theResult.Unwrap());
        if (theResult.IsError) await onError();

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => output = ok,
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action<Ok> onOk,
        Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk(theResult.Unwrap());
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action onOk,
        Action<Error> onError) =>
            (await result).Tap(onOk, onError);

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action onOk,
        Action onError) =>
            (await result).Tap(onOk, onError);

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk();
        if (theResult.IsError) await onError();

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => output = "ok",
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Action onOk,
        Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) onOk();
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task> onOk,
        Action<Error> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) onError(theResult.UnwrapError());

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task> onOk,
        Action onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) onError();

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task> onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) await onError();

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "ok");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         () => Task.Run(() => output = "ok"),
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task> onOk,
        Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk();
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         err => output = err.Message);
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
        Action<Error> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) onError(theResult.UnwrapError());

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         () => output = "err");
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
        Action onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) onError();

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         () => Task.Run(() => output = "err"));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "err");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
        Func<Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) await onError();

        return theResult;
    }

    /// <summary>
    /// Tap into a result to perform a side effect without consuming the result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output = string.Empty;
    /// var result = await Ok("hello, world")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsOk);
    /// Assert.AreEqual(output, "hello, world");
    ///
    /// result = await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapAsync(
    ///         ok => Task.Run(() => output = ok),
    ///         err => Task.Run(() => output = err.Message));
    ///
    /// Assert.IsTrue(result.IsError);
    /// Assert.AreEqual(output, "error");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the result when ok.</typeparam>
    /// <typeparam name="Error">The type of the result when error.</typeparam>
    /// <param name="result">The result to tap.</param>
    /// <param name="onOk">An action to perform when ok.</param>
    /// <param name="onError">An action to perform when error.</param>
    /// <returns>The input result.</returns>
    public static async Task<Result<Ok, Error>> TapAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task> onOk,
        Func<Error, Task> onError)
    {
        var theResult = await result;
        if (theResult.IsOk) await onOk(theResult.Unwrap());
        if (theResult.IsError) await onError(theResult.UnwrapError());

        return theResult;
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action<Ok>[] onOk) =>
            (await result).TapOk(onOk);

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, sequential or parallel.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action<Ok>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk),
                () => RunSequential(theResult.Unwrap(), onOk))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action<Ok>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await RunSequential(theResult.Unwrap(), onOk, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         ok => effects.Add(ok),
    ///         ok => effects.Add(ok + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, sequential or parallel.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action<Ok>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk, cancellationToken),
                () => RunSequential(theResult.Unwrap(), onOk, cancellationToken))
            .PipeAsync(theResult);
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action[] onOk) =>
        (await result).TapOk(onOk);

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, either sequential or parallel.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk),
                () => RunSequential(onOk))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await RunSequential(onOk, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => effects.Add("ok"),
    ///         () => effects.Add("ok!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, either sequential or parallel.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk, cancellationToken),
                () => RunSequential(onOk, cancellationToken))
            .PipeAsync(theResult);
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsOk) await RunSequential(onOk);
        return theResult;
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, sequential or parallel.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk),
                () => RunSequential(onOk))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await RunSequential(onOk, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("ok")),
    ///         () => Task.Run(() => effects.Add("ok!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "ok");
    /// Assert.AreEqual(effects[1], "ok!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, sequential or parallel.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onOk, cancellationToken),
                () => RunSequential(onOk, cancellationToken))
            .PipeAsync(theResult);
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsOk) await RunSequential(theResult.Unwrap(), onOk);
        return theResult;
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, either sequential or parallel.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk),
                () => RunSequential(theResult.Unwrap(), onOk))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         CancellationToken.None,
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await RunSequential(theResult.Unwrap(), onOk, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Ok.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Ok("input")
    ///     .Async()
    ///     .TapOkAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         ok => Task.Run(() => effects.Add(ok)),
    ///         ok => Task.Run(() => effects.Add(ok + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "input");
    /// Assert.AreEqual(effects[1], "input!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, either sequential or parallel.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onOk">The action to perform when the value is ok.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapOkAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Ok, Task>[] onOk)
    {
        var theResult = await result;
        if (theResult.IsError) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.Unwrap(), onOk, cancellationToken),
                () => RunSequential(theResult.Unwrap(), onOk, cancellationToken))
            .PipeAsync(theResult);
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action<Error>[] onError) =>
            (await result).TapError(onError);

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action<Error>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError),
                () => RunSequential(theResult.UnwrapError(), onError))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action<Error>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await RunSequential(theResult.UnwrapError(), onError, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         err => effects.Add(err.Message),
    ///         err => effects.Add(err.Message + "!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action<Error>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError, cancellationToken),
                () => RunSequential(theResult.UnwrapError(), onError, cancellationToken))
            .PipeAsync(theResult);
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Action[] onError) =>
            (await result).TapError(onError);

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Action[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError),
                () => RunSequential(onError))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Action[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await RunSequential(onError, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: examples
    // todo: docs
    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => effects.Add("err"),
    ///         () => effects.Add("err!"));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError, cancellationToken),
                () => RunSequential(onError, cancellationToken))
            .PipeAsync(theResult);
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsError) await RunSequential(onError);
        return theResult;
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>

    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError),
                () => RunSequential(onError))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>

    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await RunSequential(onError, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         () => Task.Run(() => effects.Add("err")),
    ///         () => Task.Run(() => effects.Add("err!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "err");
    /// Assert.AreEqual(effects[1], "err!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>

    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(onError, cancellationToken),
                () => RunSequential(onError, cancellationToken))
            .PipeAsync(theResult);
    }

    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <param name="result">The result to tap into.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsError) await RunSequential(theResult.UnwrapError(), onError);
        return theResult;
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError),
                () => RunSequential(theResult.UnwrapError(), onError))
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         CancellationToken.None,
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        CancellationToken cancellationToken,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await RunSequential(theResult.UnwrapError(), onError, cancellationToken)
            .PipeAsync(theResult);
    }

    // todo: tests
    /// <summary>
    /// Tap into the result and perform an action when the result is Error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var effects = new List&lt;string&gt;();
    ///
    /// await Ok("input")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.rSequential,
    ///         CancellationToken.None,
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 0);
    /// 
    /// await Error&lt;string&gt;("error")
    ///     .Async()
    ///     .TapErrorAsync(
    ///         ProcessingOrder.Sequential,
    ///         CancellationToken.None,
    ///         err => Task.Run(() => effects.Add(err.Message)),
    ///         err => Task.Run(() => effects.Add(err.Message + "!")));
    ///
    /// Assert.AreEqual(effects.Count, 2);
    /// Assert.AreEqual(effects[0], "error");
    /// Assert.AreEqual(effects[1], "error!");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="result">The result to tap into.</param>
    /// <param name="processingOrder">The processing order, parallel or sequential.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="onError">The action to perform when the value is error.</param>
    /// <typeparam name="Error">The type when the result is an error.</typeparam>
    /// <typeparam name="Ok">The type when the result is ok.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<Result<Ok, Error>> TapErrorAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Error, Task>[] onError)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult;
        return await (processingOrder == ProcessingOrder.Parallel)
            .Async()
            .MatchAsync(
                () => RunParallel(theResult.UnwrapError(), onError, cancellationToken),
                () => RunSequential(theResult.UnwrapError(), onError, cancellationToken))
            .PipeAsync(theResult);
    }
}