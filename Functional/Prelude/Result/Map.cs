namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .MapAsync(value => value.ToString());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedOk, Error>> MapAsync<Ok, TMappedOk, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, TMappedOk> mapper) =>
            (await result).Map(mapper);

    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .MapAsync(() => "alternate value");
    /// 
    /// Assert.AreEqual(mapped.Unwrap(), "alternate value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedOk, Error>> MapAsync<Ok, TMappedOk, Error>(
        this Task<Result<Ok, Error>> result,
        Func<TMappedOk> mapper) =>
            (await result).Map(mapper);

    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .MapAsync(value => value.ToString().Async());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedOk, Error>> MapAsync<Ok, TMappedOk, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Ok, Task<TMappedOk>> mapper)
    {
        var theResult = await result;
        if (theResult.IsOk) return await mapper(theResult.Unwrap());
        return theResult.UnwrapError();
    }

    /// <summary>
    /// Map an Ok result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Ok(42)
    ///         .Async()
    ///         .MapAsync(() => "alternate value".Async());
    ///
    /// Assert.AreEqual(mapped.Unwrap(), "alternate value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedOk, Error>> MapAsync<Ok, TMappedOk, Error>(
        this Task<Result<Ok, Error>> result,
        Func<Task<TMappedOk>> mapper)
    {
        var theResult = await result;
        if (theResult.IsOk) return await mapper();
        return theResult.UnwrapError();
    }

    /// <summary>
    /// Map an Error result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Error&lt;int, string&gt;("error message")
    ///         .Async()
    ///         .MapErrorAsync(err => new Exception(err));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedError">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Ok, TMappedError>> MapErrorAsync<Ok, Error, TMappedError>(
        this Task<Result<Ok, Error>> result,
        Func<Error, TMappedError> errorMapper) =>
            (await result).MapError(errorMapper);

    /// <summary>
    /// Map an Error result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Error&lt;int, string&gt;("error message")
    ///         .Async()
    ///         .MapErrorAsync(() => new Exception("alternate message"));
    ///
    /// Assert.AreEqual(mapped.UnwrapError().Message, "alternate message");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedError">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Ok, TMappedError>> MapErrorAsync<Ok, Error, TMappedError>(
        this Task<Result<Ok, Error>> result,
        Func<TMappedError> errorMapper) =>
        (await result).MapError(errorMapper);

    /// <summary>
    /// Map an Error result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Error&lt;int, string&gt;("error message")
    ///         .Async()
    ///         .MapErrorAsync(err => new Exception(err).Async());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedError">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Ok, TMappedError>> MapErrorAsync<Ok, Error, TMappedError>(
        this Task<Result<Ok, Error>> result,
        Func<Error, Task<TMappedError>> errorMapper)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult.Unwrap();
        return await errorMapper(theResult.UnwrapError());
    }

    /// <summary>
    /// Map an Error result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Error&lt;int, string&gt;("error message")
    ///         .Async()
    ///         .MapErrorAsync(() => new Exception("alternate message").Async());
    ///
    /// Assert.AreEqual(mapped.UnwrapError().Message, "alternate message");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="Ok">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedError">The type of the converted input.</typeparam>
    /// <typeparam name="Error">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<Ok, TMappedError>> MapErrorAsync<Ok, Error, TMappedError>(
        this Task<Result<Ok, Error>> result,
        Func<Task<TMappedError>> errorMapper)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult.Unwrap();
        return await errorMapper();
    }
}