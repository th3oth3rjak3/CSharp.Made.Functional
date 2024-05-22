using System.Diagnostics.CodeAnalysis;

namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Return the default Unit.
    /// </summary>
    public static Unit Unit => Unit.Default;

    /// <summary>
    /// Create a Task from an input.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input to wrap as a Task.</param>
    /// <returns>A new Task.</returns>
    public static Task<T> Async<T>(this T input) =>
        Task.FromResult(input);

    /// <summary>
    /// Ignores the output of a function. C# does this by default in functions
    /// that return void, but this can be used to declare that the output is
    /// intentionally ignored.
    /// </summary>
    /// <typeparam name="T">Any input type.</typeparam>
    /// <param name="_">This parameter is ignored.</param>
    [ExcludeFromCodeCoverage]
    public static void Ignore<T>(this T? _) { }

    /// <summary>
    /// Ignores the output of an async function. C# does this by default in functions
    /// that return void, but this can be used to declare that the output is
    /// intentionally ignored.
    /// </summary>
    /// <typeparam name="T">Any input type.</typeparam>
    /// <param name="toIgnore">This parameter is ignored.</param>
    public static async Task IgnoreAsync<T>(this Task<T> toIgnore) =>
        await toIgnore;
}
