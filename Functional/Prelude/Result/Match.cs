namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> onOk,
        Func<Error, Output> onError)
    {
        var theResult = await result;
        return theResult
            .Match(onOk, onError);
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> onOk,
        Func<Error, Output> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk(theResult.Unwrap())
            : onError(theResult.UnwrapError());
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> onOk,
        Func<Error, Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? onOk(theResult.Unwrap())
            : await onError(theResult.UnwrapError());
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> onOk,
        Func<Error, Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk(theResult.Unwrap())
            : await onError(theResult.UnwrapError());
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Output> onOk,
        Func<Error, Output> onError)
    {
        var theResult = await result;
        return theResult
            .Match(onOk, onError);
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Output> onOk,
        Func<Error, Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? onOk()
            : await onError(theResult.UnwrapError());
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task<Output>> onOk,
        Func<Error, Output> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk()
            : onError(theResult.UnwrapError());
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task<Output>> onOk,
        Func<Error, Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk()
            : await onError(theResult.UnwrapError());
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> onOk,
        Func<Output> onError)
    {
        var theResult = await result;
        return theResult
            .Match(onOk, onError);
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString(),
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Output> onOk,
        Func<Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? onOk(theResult.Unwrap())
            : await onError();
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> onOk,
        Func<Output> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk(theResult.Unwrap())
            : onError();
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "42"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         ok => ok.ToString().Async(),
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Output>> onOk,
        Func<Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk(theResult.Unwrap())
            : await onError();
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Output> onOk,
        Func<Output> onError)
    {
        var theResult = await result;
        return theResult
            .Match(onOk, onError);
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         () => "err")
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task<Output>> onOk,
        Func<Output> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk()
            : onError();
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok",
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Output> onOk,
        Func<Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? onOk()
            : await onError();
    }

    /// <summary>
    /// Match the result to an Ok or an Error and perform some function on either case.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Ok(42)
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "ok"));
    ///
    /// Error&lt;int&gt;("error")
    ///     .Async()
    ///     .MatchAsync(
    ///         () => "ok".Async(),
    ///         () => "err".Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "err"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The result input type.</typeparam>
    /// <typeparam name="Output">The output type.</typeparam>
    /// <typeparam name="Error">The type of the expected error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="onOk">Perform some function on the Ok result.</param>
    /// <param name="onError">Perform some function on the Error result.</param>
    /// <returns>The result of executing the onOk or onError function.</returns>
    public static async Task<Output> MatchAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task<Output>> onOk,
        Func<Task<Output>> onError)
    {
        var theResult = await result;
        return theResult.IsOk
            ? await onOk()
            : await onError();
    }
}