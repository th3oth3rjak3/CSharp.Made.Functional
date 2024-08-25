namespace Functional;

/// <summary>
/// Import this static class to gain access to all of the extension methods and static methods.
/// </summary>
public static partial class Prelude
{
    /// <summary>
    /// Match the result of a boolean expression and perform a function which returns a value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// (1 &lt; 10)
    ///     .Match(
    ///         // Returned because the condition evaluates to true        
    ///         () => "this will be returned", 
    ///         // Would be returned if the condition had evaluated to false.
    ///         () => "not returned");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="whenTrue">The function to call when true.</param>
    /// <param name="whenFalse">The function to call when false.</param>
    /// <returns>The result of the provided function that was called.</returns>
    public static TResult Match<TResult>(this bool condition, Func<TResult> whenTrue, Func<TResult> whenFalse) =>
        condition
            ? whenTrue()
            : whenFalse();

    /// <summary>
    /// Match the result of a boolean expression and perform a function which returns a value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await (1 &lt; 10)
    ///     .Async()
    ///     .MatchAsync(
    ///         // Returned because the condition evaluates to true        
    ///         () => "this will be returned", 
    ///         // Would be returned if the condition had evaluated to false.
    ///         () => "not returned");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="whenTrue">The function to call when true.</param>
    /// <param name="whenFalse">The function to call when false.</param>
    /// <returns>The result of the provided function that was called.</returns>
    public static async Task<TResult> MatchAsync<TResult>(this Task<bool> condition, Func<TResult> whenTrue, Func<TResult> whenFalse) =>
        (await condition)
            ? whenTrue()
            : whenFalse();

    /// <summary>
    /// Match the result of a boolean expression and perform a function which returns a value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await (1 &lt; 10)
    ///     .Async()
    ///     .MatchAsync(
    ///         // Returned because the condition evaluates to true        
    ///         () => "this will be returned".Async(), 
    ///         // Would be returned if the condition had evaluated to false.
    ///         () => "not returned");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="whenTrue">The function to call when true.</param>
    /// <param name="whenFalse">The function to call when false.</param>
    /// <returns>The result of the provided function that was called.</returns>
    public static async Task<TResult> MatchAsync<TResult>(this Task<bool> condition, Func<Task<TResult>> whenTrue, Func<TResult> whenFalse) =>
        (await condition)
            ? await whenTrue()
            : whenFalse();

    /// <summary>
    /// Match the result of a boolean expression and perform a function which returns a value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await (1 &lt; 10)
    ///     .Async()
    ///     .MatchAsync(
    ///         // Returned because the condition evaluates to true        
    ///         () => "this will be returned", 
    ///         // Would be returned if the condition had evaluated to false.
    ///         () => "not returned".Async());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="whenTrue">The function to call when true.</param>
    /// <param name="whenFalse">The function to call when false.</param>
    /// <returns>The result of the provided function that was called.</returns>
    public static async Task<TResult> MatchAsync<TResult>(this Task<bool> condition, Func<TResult> whenTrue, Func<Task<TResult>> whenFalse) =>
        (await condition)
            ? whenTrue()
            : await whenFalse();

    /// <summary>
    /// Match the result of a boolean expression and perform a function which returns a value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// await (1 &lt; 10)
    ///     .Async()
    ///     .MatchAsync(
    ///         // Returned because the condition evaluates to true        
    ///         () => "this will be returned".Async(), 
    ///         // Would be returned if the condition had evaluated to false.
    ///         () => "not returned".Async());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TResult">The return type.</typeparam>
    /// <param name="condition">The condition to evaluate.</param>
    /// <param name="whenTrue">The function to call when true.</param>
    /// <param name="whenFalse">The function to call when false.</param>
    /// <returns>The result of the provided function that was called.</returns>
    public static async Task<TResult> MatchAsync<TResult>(this Task<bool> condition, Func<Task<TResult>> whenTrue, Func<Task<TResult>> whenFalse) =>
        (await condition)
            ? await whenTrue()
            : await whenFalse();
}
