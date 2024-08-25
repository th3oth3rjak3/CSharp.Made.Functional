namespace Functional;
public static partial class Prelude
{
    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output =
    ///     "some value"
    ///         // Writes "some value" to the console.
    ///         .Tap(Console.WriteLine);
    ///
    /// Assert.AreEqual(output, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns>The input.</returns>
    public static T Tap<T>(this T input, params Action<T>[] actions)
    {
        actions.ToList().ForEach(action => action(input));
        return input;
    }

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output =
    ///     "some value"
    ///         // Writes "writing the the console" to the console.
    ///         .Tap(() => Console.WriteLine("writing the the console"));
    ///
    /// Assert.AreEqual(output, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns>The input.</returns>
    public static T Tap<T>(this T input, params Action[] actions)
    {
        actions.ToList().ForEach(action => action());
        return input;
    }

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output =
    ///     await "some value"
    ///         .Async()
    ///         // Writes "some value" to the console.
    ///         .TapAsync(Console.WriteLine);
    ///
    /// Assert.AreEqual(output, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns>The input.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Action<T>[] actions) =>
        (await input).Tap(actions);

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output =
    ///     await "some value"
    ///         .Async()
    ///         // Writes "writing to the console" to the console.
    ///         .TapAsync(() => Console.WriteLine("writing to the console"));
    ///
    /// Assert.AreEqual(output, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns>The input.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Action[] actions) =>
        (await input).Tap(actions);

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output =
    ///     await "some value"
    ///         .Async()
    ///         // Writes "some value" to the console.
    ///         .TapAsync(input => EffectAsync(() => Console.WriteLine(input)));
    ///
    /// Assert.AreEqual(output, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns>The input.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Func<T, Task>[] actions)
    {
        var theInput = await input;
        return await RunSequential(theInput, actions).PipeAsync(theInput);
    }

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// var output =
    ///     await "some value"
    ///         .Async()
    ///         // Writes "writing to the console" to the console.
    ///         .TapAsync(() => EffectAsync(() => Console.WriteLine("writing to the console")));
    ///
    /// Assert.AreEqual(output, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns>The input.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Func<Task>[] actions)
    {
        var theInput = await input;
        return await RunSequential(actions).PipeAsync(theInput);
    }
}