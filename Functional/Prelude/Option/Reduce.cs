namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Extract the contents of an Option when Some, otherwise return the alternate value when None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int some =
    ///     await Some(123)
    ///         .Async()
    ///         .ReduceAsync(() => 0);
    ///
    /// Assert.AreEqual(some, 123);
    ///
    /// int none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .ReduceAsync(() => 0);
    ///
    /// Assert.AreEqual(none, 0);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(this Task<Option<T>> optional, Func<T> alternate)
    {
        var result = await optional;

        return result.Match(
            some => some,
            alternate);
    }
    
    /// <summary>
    /// Extract the contents of an Option when Some, otherwise return the alternate value when None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int some =
    ///     await Some(123)
    ///         .Async()
    ///         .ReduceAsync(0);
    ///
    /// Assert.AreEqual(some, 123);
    ///
    /// int none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .ReduceAsync(0);
    ///
    /// Assert.AreEqual(none, 0);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Task<Option<T>> optional,
        T alternate)
    {
        var result = await optional;

        return result.Match(
            some => some,
            () => alternate);
    }

    /// <summary>
    /// Extract the contents of an Option when Some, otherwise return the alternate value when None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int some =
    ///     await Some(123)
    ///         .Async()
    ///         .ReduceAsync(Task.FromResult(0));
    ///
    /// Assert.AreEqual(some, 123);
    ///
    /// int none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .ReduceAsync(Task.FromResult(0));
    ///
    /// Assert.AreEqual(none, 0);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Task<Option<T>> optional,
        Task<T> alternate)
    {
        var result = await optional;

        return result.IsSome ? result.Unwrap() : await alternate;
    }

    /// <summary>
    /// Extract the contents of an Option when Some, otherwise return the alternate value when None.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int some =
    ///     await Some(123)
    ///         .Async()
    ///         .ReduceAsync(() => Task.FromResult(0));
    ///
    /// Assert.AreEqual(some, 123);
    ///
    /// int none =
    ///     await None&lt;int&gt;()
    ///         .Async()
    ///         .ReduceAsync(() => Task.FromResult(0));
    ///
    /// Assert.AreEqual(none, 0);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="optional">The option to extract contents from when Some.</param>
    /// <param name="alternate">An alternate value to provide when None.</param>
    /// <returns>The resulting contents.</returns>
    public static async Task<T> ReduceAsync<T>(
        this Task<Option<T>> optional,
        Func<Task<T>> alternate)
    {
        var result = await optional;
        return result.IsSome ? result.Unwrap() : await alternate();
    }
}