namespace Functional;
public static partial class Prelude
{
    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns></returns>
    public static T Tap<T>(this T input, params Action<T>[] actions)
    {
        actions
            .ToList()
            .ForEach(action =>
                action(input));

        return input;
    }

    /// <summary>
    /// Tap into a value to perform a series of actions which could return void.
    /// This function is used to turn imperative code into functional, fluent syntax.
    /// </summary>
    /// <typeparam name="T">The type of the input object.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="actions">An array of actions to perform on the input.</param>
    /// <returns></returns>
    public static T Tap<T>(this T input, params Action[] actions)
    {
        actions
            .ToList()
            .ForEach(action =>
                action());

        return input;
    }

    /// <summary>
    /// Used to wrap an action function that is asynchronous.
    /// </summary>
    /// <typeparam name="T">The type of the <paramref name="input"/>.</typeparam>
    /// <param name="input">The input to use with the <paramref name="actions"/>.</param>
    /// <param name="actions">The action to await and call on the <paramref name="input"/>.</param>
    /// <returns>The resulting input as a task.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Func<T, Task>[] actions)
    {
        await Task.WhenAll(
            actions
                .Select(async action =>
                    await action(await input)));

        return await input;
    }

    /// <summary>
    /// Used to wrap an action that is asynchronous.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input used in the async action.</param>
    /// <param name="actions">The action to perform on the input.</param>
    /// <typeparam name="TResult">The resulting type after performing actions on the input.</typeparam>
    /// <returns>The input value.</returns>
    public static async Task<T> TapAsync<T, TResult>(
        this Task<T> input,
        params Func<T, Task<TResult>>[] actions)
    {
        await Task
            .WhenAll(
                actions
                    .Select(async action =>
                        await action(await input)))
            .IgnoreAsync();

        return await input;
    }

    /// <summary>
    /// Used to perform an action which returns void on an input that is a Task.
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="input">The input to be awaited and then acted upon.</param>
    /// <param name="actions">The action to perform after awaiting the input.</param>
    /// <returns>The input as a task.</returns>
    public static async Task<T> TapAsync<T>(this Task<T> input, params Action<T>[] actions) =>
        (await input)
            .Tap(actions);

}
