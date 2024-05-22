namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Map one object type to another using a mapping function.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// int input = 42;
    /// string output = input.Pipe(value => value.ToString());
    /// 
    /// Assert.AreEqual(output, "42");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TResult">The type to convert to.</typeparam>
    /// <param name="input">The input object.</param>
    /// <param name="mapper">A function delegate which transforms the 
    /// input type to the output type.</param>
    /// <returns>The mapped result.</returns>
    public static TResult Pipe<T, TResult>(this T input, Func<T, TResult> mapper) =>
        mapper(input);

    // TODO: Examples
    /// <summary>
    /// Perform a series of actions on the input and return unit.
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <typeparam name="TOutput">The output type.</typeparam>
    /// <param name="input">The input value.</param>
    /// <param name="mapper">A mapping function with no inputs.</param>
    /// <returns>Unit.</returns>
    public static TOutput Pipe<T, TOutput>(this T input, Func<TOutput> mapper) =>
        input.Pipe(_ => mapper());

    // TODO: Examples
    /// <summary>
    /// Used to wrap an async function that transforms a Task of <typeparamref name="TInput"/> to <typeparamref name="TInput"/>.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TResult">The output type.</typeparam>
    /// <param name="input">The input to transform.</param>
    /// <param name="func">The transformation function.</param>
    /// <returns>A result of the function as a task.</returns>
    public static async Task<TResult> PipeAsync<TInput, TResult>(this Task<TInput> input, Func<TInput, Task<TResult>> func) =>
        await func(await input);

    // TODO: Examples
    /// <summary>
    /// Used to wrap an async mapping function that transforms 
    /// <typeparamref name="TInput"/> to <typeparamref name="TResult"/>.
    /// </summary>
    /// <typeparam name="TInput">The input type.</typeparam>
    /// <typeparam name="TResult">The resulting type.</typeparam>
    /// <param name="input">The input to transform.</param>
    /// <param name="func">The transformation function.</param>
    /// <returns>The result of the transformation function.</returns>
    public static async Task<TResult> PipeAsync<TInput, TResult>(
        this Task<TInput> input,
        Func<TInput, TResult> func) =>
            func(await input);

    // TODO: Examples
    /// <summary>
    /// Used to wrap an async input that performs actions.
    /// </summary>
    /// <typeparam name="TInput">The type of the input.</typeparam>
    /// <typeparam name="TOutput">The type of the output.</typeparam>
    /// <param name="input">The input to perform actions on.</param>
    /// <param name="mapper">A mapping function that ignores the input from the pipeline.</param>
    /// <returns>An awaitable Unit.</returns>
    public static async Task<TOutput> PipeAsync<TInput, TOutput>(this Task<TInput> input, Func<TOutput> mapper) =>
        await (await input)
            .Pipe(_ => mapper())
            .Async();

}
