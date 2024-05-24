using System.Diagnostics.CodeAnalysis;


namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Return the default Unit.
    /// </summary>
    public static Unit Unit() => new();

    /// <summary>
    /// Create a Task from an input.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;string&gt; asyncString = "hello world".Async();
    /// string value = await asyncString;
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input to wrap as a Task.</param>
    /// <returns>A new Task.</returns>
    public static Task<T> Async<T>(this T input) =>
        Task.FromResult(input);

    /// <summary>
    /// Ignores the output of a function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// void PerformWork()
    /// {
    ///     // Without Ignore, using discard operator
    ///     _ = Some("123")
    ///         .Match(() => "some", () => "none");
    ///         
    ///     // With Ignore
    ///     Some("123")
    ///         // Returns a string
    ///         .Match(() => "some", () => "none") 
    ///         // Ignores that output.
    ///         .Ignore();  
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">Any input type.</typeparam>
    /// <param name="_">This parameter is ignored.</param>
    [ExcludeFromCodeCoverage]
    public static void Ignore<T>(this T? _) { }

    /// <summary>
    /// Ignores the output of an async function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task PerformWork()
    /// {
    ///     // Without Ignore, using discard operator
    ///     _ = await Some("123")
    ///         .Async()
    ///         .MatchAsync(() => "some", () => "none");
    ///         
    ///     // With Ignore
    ///     await Some("123")
    ///         .Async()
    ///         // Returns a string
    ///         .MatchAsync(() => "some", () => "none") 
    ///         // Ignores that output.
    ///         .IgnoreAsync();  
    /// }
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">Any input type.</typeparam>
    /// <param name="toIgnore">This parameter is ignored.</param>
    public static async Task IgnoreAsync<T>(this Task<T> toIgnore) =>
        await toIgnore;
}