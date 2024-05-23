namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Map a Success result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Success(42)
    ///         .Async()
    ///         .MapAsync(value => value.ToString());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedSuccess">The type of the converted input.</typeparam>
    /// <typeparam name="TFailure">The type of the failure.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedSuccess, TFailure>> MapAsync<TSuccess, TMappedSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TSuccess, TMappedSuccess> mapper)
    {
        var outcome = await result;

        return outcome.Map(mapper);
    }

    /// <summary>
    /// Map a Success result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;string, Exception&gt; mapped = 
    ///     await Success(42)
    ///         .Async()
    ///         .MapAsync(value => value.ToString().Async());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedSuccess">The type of the converted input.</typeparam>
    /// <typeparam name="TFailure">The type of the failure.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="mapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TMappedSuccess, TFailure>> MapAsync<TSuccess, TMappedSuccess, TFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TSuccess, Task<TMappedSuccess>> mapper)
    {
        var theResult = await result;

        if (theResult.IsSuccess)
        {
            var mapped = await mapper(theResult.Unwrap());
            return Success<TMappedSuccess, TFailure>(mapped);
        }

        return Failure<TMappedSuccess, TFailure>(theResult.UnwrapFailure());
    }

    /// <summary>
    /// Map a Failure result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Failure&lt;int, string&gt;("error message")
    ///         .Async()
    ///         .MapFailureAsync(err => new Exception(err));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedFailure">The type of the converted input.</typeparam>
    /// <typeparam name="TFailure">The type of the failure.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TSuccess, TMappedFailure>> MapFailureAsync<TSuccess, TFailure, TMappedFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TFailure, TMappedFailure> errorMapper)
    {
        var theResult = await result;

        if (theResult.IsSuccess) return Success<TSuccess, TMappedFailure>(theResult.Unwrap());

        var mapped = errorMapper(theResult.UnwrapFailure());
        return Failure<TSuccess, TMappedFailure>(mapped);
    }


    /// <summary>
    /// Map a Failure result from a previous operation to a new result.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Result&lt;int, Exception&gt; mapped = 
    ///     await Failure&lt;int, string&gt;("error message")
    ///         .Async()
    ///         .MapFailureAsync(err => new Exception(err).Async());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TSuccess">The type of the contents from the previous result.</typeparam>
    /// <typeparam name="TMappedFailure">The type of the converted input.</typeparam>
    /// <typeparam name="TFailure">The type of the failure.</typeparam>
    /// <param name="result">The previous result.</param>
    /// <param name="errorMapper">A mapping function to convert the contents of the old result to the new contents.</param>
    /// <returns>A new result after the mapping operation has taken place.</returns>
    public static async Task<Result<TSuccess, TMappedFailure>> MapFailureAsync<TSuccess, TFailure, TMappedFailure>(
        this Task<Result<TSuccess, TFailure>> result,
        Func<TFailure, Task<TMappedFailure>> errorMapper)
    {
        var theResult = await result;

        if (theResult.IsSuccess) return Success<TSuccess, TMappedFailure>(theResult.Unwrap());

        var mapped = await errorMapper(theResult.UnwrapFailure());
        return Failure<TSuccess, TMappedFailure>(mapped);
    }
}
