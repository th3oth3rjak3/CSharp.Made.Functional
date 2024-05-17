namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action<T> doWhenSome, Action doWhenNone) =>
        (await optional)
            .Effect(doWhenSome, doWhenNone);

    /// <summary>
    /// Perform a side-effect on an option type.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public static async Task<Unit> EffectAsync<T>(this Task<Option<T>> optional, Action doWhenSome, Action doWhenNone) =>
        (await optional)
            .Effect(_ => doWhenSome(), doWhenNone);

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Action<T> doWhenSome) =>
        (await optional)
            .Effect(doWhenSome, () => { });

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Action doWhenSome) =>
        (await optional)
            .Effect(_ => doWhenSome(), () => { });

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Func<T, Task> doWhenSome)
    {
        var option = await optional;
        if (option.IsSome)
        {
            await doWhenSome(option.Unwrap()!);
        }

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is Some.
    /// </summary>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenSome">Perform this action when the value is Some.</param>
    public static async Task<Unit> EffectSomeAsync<T>(this Task<Option<T>> optional, Func<Task> doWhenSome)
    {
        var option = await optional;
        if (option.IsSome)
        {
            await doWhenSome();
        }

        return Unit.Default;
    }

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is None.
    /// </summary>
    /// <typeparam name="T">The type of the option if it were some.</typeparam>
    /// <param name="optional">The option to perform the side-effect on.</param>
    /// <param name="doWhenNone">Perform this action when the value is None.</param>
    public static async Task<Unit> EffectNoneAsync<T>(this Task<Option<T>> optional, Action doWhenNone) =>
        (await optional)
            .Effect(_ => { }, doWhenNone);

    /// <summary>
    /// Perform a side-effect on an option type when the inner value is None.
    /// </summary>
    /// <typeparam name="T">The inner type of the option.</typeparam>
    /// <param name="optional"></param>
    /// <param name="doWhenNone"></param>
    /// <returns></returns>
    public static async Task<Unit> EffectNoneAsync<T>(this Task<Option<T>> optional, Func<Task> doWhenNone)
    {
        var option = await optional;
        if (option.IsNone) await doWhenNone();

        return Unit.Default;
    }

}
