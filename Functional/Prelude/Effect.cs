namespace Functional;
public static partial class Prelude
{
    /// <summary>
    /// Perform effects on any input type that returns unit.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value.</param>
    /// <param name="actions">Actions to perform on the input value.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect<T>(this T input, params Action<T>[] actions) =>
        input.Pipe(actions);

    /// <summary>
    /// Perform effects ignoring the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The ignored input.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect<T>(this T input, params Action[] actions) =>
        input.Pipe(actions);

    /// <summary>
    /// Perform an effect that returns unit.
    /// </summary>
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect(Action action)
    {
        action();
        return Unit.Default;
    }

    /// <summary>
    /// Perform effects on the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Action<T>[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform effects ignoring the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to ignore.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Action[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform effects on the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Func<T, Task>[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform effects ignoring the input value.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The ignored input value.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(this Task<T> input, params Func<Task>[] actions) =>
        await input.PipeAsync(actions);

    /// <summary>
    /// Perform an effect which returns unit.
    /// </summary>
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(Action action) =>
        await Effect(action).Async();

    /// <summary>
    /// Perform an effect which returns unit.
    /// </summary>
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(Func<Task> action)
    {
        await action();
        return Unit.Default;
    }
}
