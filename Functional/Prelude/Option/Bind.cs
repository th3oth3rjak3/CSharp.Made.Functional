namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Option&lt;string&gt; TryGetString(int input) =>
    ///     input > 10
    ///     ? new(input.ToString())
    ///     : new();
    ///     
    /// Option&lt;string&gt; value = await 42.Optional().Async().BindAsync(TryGetString);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Option<TResult>> binder)
            where TInput : notnull
            where TResult : notnull =>
            await optional
                .MapAsync(binder)
                .ReduceAsync(None<TResult>);

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;Option&lt;string&gt;&gt; TryGetString(int input) =>
    ///     input > 10
    ///     ? new Option&lt;string&gt;(input.ToString()).Async()
    ///     : new Option&lt;string&gt;().Async();
    ///     
    /// Option&lt;string&gt; value = await 42.Optional().Async().BindAsync(TryGetString);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<TInput, Task<Option<TResult>>> binder)
            where TInput : notnull
            where TResult : notnull =>
                await optional
                    .MapAsync(binder)
                    .ReduceAsync(None<TResult>);

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;Option&lt;string&gt;&gt; TryGetString(int input) =>
    ///     input > 10
    ///     ? new Option&lt;string&gt;(input.ToString()).Async()
    ///     : new Option&lt;string&gt;().Async();
    ///     
    /// Option&lt;string&gt; value =
    ///     await 42
    ///         .Optional()
    ///         .Async()
    ///         .BindAsync(() => TryGetString(30));
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<Option<TResult>> binder)
        where TInput : notnull
        where TResult : notnull =>
            await optional
                .MapAsync(binder)
                .ReduceAsync(None<TResult>);

    /// <summary>
    /// Used instead of Map when the mapping function produces an Option type.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task&lt;Option&lt;string&gt;&gt; TryGetString(int input) =>
    ///     input > 10
    ///     ? new Option&lt;string&gt;(input.ToString()).Async()
    ///     : new Option&lt;string&gt;().Async();
    ///     
    /// Option&lt;string&gt; value =
    ///     await 42
    ///         .Optional()
    ///         .Async()
    ///         .BindAsync(() => TryGetString(30).Async());
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TResult">The type of the output.</typeparam>
    /// <param name="optional">The option to bind.</param>
    /// <param name="binder">The binding function.</param>
    /// <returns>An option of the output type.</returns>
    public static async Task<Option<TResult>> BindAsync<TInput, TResult>(
        this Task<Option<TInput>> optional,
        Func<Task<Option<TResult>>> binder)
        where TInput : notnull
        where TResult : notnull =>
            await optional
                .MapAsync(binder)
                .ReduceAsync(None<TResult>);
}