namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Perform work on a previous result. When the result is Ok, 
    /// perform work on the result by providing a binding function.
    /// On Error, the previous result will be returned as the new result type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; GetSmallNumber(int input) =>
    ///     input &lt; 10
    ///         ? Ok(input)
    ///         : Error&lt;int&gt;("the number was too big");
    ///
    /// Ok(5)
    ///     .Async()
    ///     .BindAsync(GetSmallNumber)
    ///     .EffectAsync(output => Assert.IsTrue(output.IsOk));
    ///
    /// Ok(20)
    ///     .Async()
    ///     .BindAsync(GetSmallNumber)
    ///     .EffectAsync(
    ///         output => Assert.IsTrue(output.IsError),
    ///         output => Asser.AreEqual(output.UnwrapError().Message, "the number was too big"));
    ///
    /// Error&lt;int&gt;("error occurred")
    ///     .Async()
    ///     .BindAsync(GetSmallNumber)
    ///     .EffectAsync(
    ///         output => Assert.IsTrue(output.IsError),
    ///         output => Assert.AreEqual(output.UnwrapError().Message, "error occurred"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the input.</typeparam>
    /// <typeparam name="Output">The type of the result after performing 
    /// the onOk function.</typeparam>
    /// <typeparam name="Error">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is Ok.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<Output, Error>> BindAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Result<Output, Error>> binder)
    {
        var theResult = await result;
        return theResult.IsError
            ? theResult.UnwrapError()
            : binder(theResult.Unwrap());
    }

    /// <summary>
    /// Perform work on a previous result. When the result is Ok, 
    /// perform work on the result by providing a binding function.
    /// On Error, the previous result will be returned as the new result type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;Result&lt;int, Exception&gt;&gt; GetSmallNumber(int input) =>
    ///     input &lt; 10
    ///         ? Ok(input).Async()
    ///         : Error&lt;int&gt;("the number was too big").Async();
    ///
    /// Ok(5)
    ///     .Async()
    ///     .BindAsync(GetSmallNumber)
    ///     .EffectAsync(output => Assert.IsTrue(output.IsOk));
    ///
    /// Ok(20)
    ///     .Async()
    ///     .BindAsync(GetSmallNumber)
    ///     .EffectAsync(
    ///         output => Assert.IsTrue(output.IsError),
    ///         output => Asser.AreEqual(output.UnwrapError().Message, "the number was too big"));
    ///
    /// Error&lt;int&gt;("error occurred")
    ///     .Async()
    ///     .BindAsync(GetSmallNumber)
    ///     .EffectAsync(
    ///         output => Assert.IsTrue(output.IsError),
    ///         output => Assert.AreEqual(output.UnwrapError().Message, "error occurred"));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the input.</typeparam>
    /// <typeparam name="Output">The type of the result after performing 
    /// the onOk function.</typeparam>
    /// <typeparam name="Error">The type of the error that may result from the binding operation.</typeparam>
    /// <param name="result">The previous result to bind.</param>
    /// <param name="binder">The function to perform when the 
    /// previous result is Ok.</param>
    /// <returns>The result of the bind operation.</returns>
    public static async Task<Result<Output, Error>> BindAsync<Ok, Output, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<Result<Output, Error>>> binder)
    {
        var theResult = await result;
        return theResult.IsError
            ? theResult.UnwrapError()
            : await binder(theResult.Unwrap());
    }
}