namespace Functional;
public static partial class Prelude
{
    /// <summary>
    /// Return the default Unit.
    /// </summary>
    public static Unit Unit => Unit.Default;

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


}
