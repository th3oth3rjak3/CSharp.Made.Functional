namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static Option<TResult> Bind<TInput, TResult>(
        this Option<TInput> optional,
        Func<TInput, Option<TResult>> binder) =>
            optional
                .Map(binder)
                .Reduce(Option.None<TResult>);

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Option<TResult>> binder) =>
            await optional
                .MapAsync(binder)
                .ReduceAsync(Option.None<TResult>);

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<Option<TResult>>> binder) =>
            await optional
                .MapAsync(binder)
                .ReduceAsync(Option.None<TResult>);
}
