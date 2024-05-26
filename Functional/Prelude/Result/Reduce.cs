namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// When the result is Ok, return its contents, otherwise return an alternate value discarding the error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await Ok(42)
    ///     .Async()
    ///     .ReduceAsync(0)
    ///     .EffectAsync(output => Assert.AreEqual(output, 42));
    /// 
    /// await Error&lt;int&gt;("error")
    ///     .Async()
    ///     .ReduceAsync(0)
    ///     .EffectAsync(output => Assert.AreEqual(output, 0));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="result">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Ok alternate) =>
            (await result).Reduce(alternate);
    
    /// <summary>
    /// When the result is Ok, return its contents, otherwise return an alternate value discarding the error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await Ok(42)
    ///     .Async()
    ///     .ReduceAsync(0.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, 42));
    /// 
    /// await Error&lt;int&gt;("error")
    ///     .Async()
    ///     .ReduceAsync(0.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, 0));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="result">The result to unpack.</param>
    /// <param name="alternate">An alternate value.</param>
    /// <returns>When success, the contents, otherwise the alternate.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Task<Ok> alternate)
    {
        var theResult = await result;
        return theResult.IsOk
            ? theResult.Unwrap()
            : await alternate;
    }
    
    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value using the error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await Ok&lt;string, Exception&gt;("hello, world")
    ///     .Async()
    ///     .ReduceAsync(err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "hello, world"));
    ///
    /// await Error&lt;string, Exception&gt;(new Exception("error"))
    ///     .Async()
    ///     .ReduceAsync(err => err.Message)
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="result">The result to unpack.</param>
    /// <param name="alternate">A function which uses an Error 
    /// result to return an alternate.</param>
    /// <returns>When Ok, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Error, Ok> alternate) =>
            (await result).Reduce(alternate);
    
    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value using the error.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await Ok&lt;string, Exception&gt;("hello, world")
    ///     .Async()
    ///     .ReduceAsync(err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "hello, world"));
    ///
    /// await Error&lt;string, Exception&gt;(new Exception("error"))
    ///     .Async()
    ///     .ReduceAsync(err => err.Message.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, "error"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="result">The result to unpack.</param>
    /// <param name="alternate">A function which uses an Error 
    /// result to return an alternate.</param>
    /// <returns>When Ok, the contents, otherwise the return
    /// value of the alternate.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Error, Task<Ok>> alternate)
    {
        var theResult = await result;
        return theResult.IsOk
            ? theResult.Unwrap()
            : await alternate(theResult.UnwrapError());
    }
    
    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value by discarding the error.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await Ok(42)
    ///     .Async()
    ///     .ReduceAsync(() => 0)
    ///     .EffectAsync(output => Assert.AreEqual(output, 42));
    ///
    /// await Error&lt;int&gt;("error")
    ///     .Async()
    ///     .ReduceAsync(() => 0)
    ///     .EffectAsync(output => Assert.AreEqual(output, 0));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="result">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When Ok, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok> alternate) =>
            (await result).Reduce(alternate);
    
    /// <summary>
    /// When the result is Ok, return its contents, 
    /// otherwise execute the function to produce an alternate value by discarding the error.
    /// This method is good for when the alternate function might be 
    /// computationally expensive.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await Ok(42)
    ///     .Async()
    ///     .ReduceAsync(() => 0.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, 42));
    ///
    /// await Error&lt;int&gt;("error")
    ///     .Async()
    ///     .ReduceAsync(() => 0.Async())
    ///     .EffectAsync(output => Assert.AreEqual(output, 0));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The input type.</typeparam>
    /// <typeparam name="Error">The error type.</typeparam>
    /// <param name="result">The result to unpack.</param>
    /// <param name="alternate">A function that takes no inputs, but produces an
    /// alternate value.</param>
    /// <returns>When Ok, the contents, otherwise the return value of
    /// the alternate function.</returns>
    public static async Task<Ok> ReduceAsync<Ok, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task<Ok>> alternate)
    {
        var theResult = await result;
        return theResult.IsOk
            ? theResult.Unwrap()
            : await alternate();
    }
}