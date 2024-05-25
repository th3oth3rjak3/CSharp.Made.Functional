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
    /// <typeparam name="TOk">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedOk, TError>> MapAsync<TOk, TMappedOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<TOk, TMappedOk> mapper) =>
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
    /// <typeparam name="TOk">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedOk">The type of the converted input.</typeparam>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedOk, TError>> MapAsync<TOk, TMappedOk, TError>(
        this Task<Result<TOk, TError>> result,
        Func<TOk, Task<TMappedOk>> mapper)
    {
        var theResult = await result;
        if (theResult.IsOk) return await mapper(theResult.Unwrap());
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
    /// <typeparam name="TOk">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedError">The type of the converted input.</typeparam>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TOk, TMappedError>> MapErrorAsync<TOk, TError, TMappedError>(
        this Task<Result<TOk, TError>> result,
        Func<TError, TMappedError> errorMapper) =>
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
    /// <typeparam name="TOk">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedError">The type of the converted input.</typeparam>
    /// <typeparam name="TError">The type of the Error.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TOk, TMappedError>> MapErrorAsync<TOk, TError, TMappedError>(
        this Task<Result<TOk, TError>> result,
        Func<TError, Task<TMappedError>> errorMapper)
    {
        var theResult = await result;
        if (theResult.IsOk) return theResult.Unwrap();
        return await errorMapper(theResult.UnwrapError());
    }
}