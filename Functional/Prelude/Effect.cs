namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Perform effects on any input type that returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string savedValue = string.Empty;
    /// 
    /// Unit output = 
    ///     "important value"
    ///         .Effect(
    ///             value => savedValue = value,
    ///             value => Console.WriteLine($"Saved the value: {value}"));
    ///             
    /// Assert.AreEqual(savedValue, "important value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value.</param>
    /// <param name="actions">Actions to perform on the input value.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect<T>(this T input, params Action<T>[] actions)
    {
        actions.ToList().ForEach(action => action(input));
        return Unit();
    }

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string savedValue = string.Empty;
    /// 
    /// Unit output = 
    ///     "ignored value"
    ///         .Effect(
    ///             () => savedValue = "performed effect",
    ///             () => Console.WriteLine("Performing effect"));
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="_">The ignored input.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect<T>(this T _, params Action[] actions)
    {
        actions.ToList().ForEach(action => action());
        return Unit();
    }

    /// <summary>
    /// Perform an effect that returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string savedValue = string.Empty;
    /// 
    /// string output = 
    ///     Effect(() => savedValue = "saved")
    ///         .Pipe(42)
    ///         .Pipe(value => value.ToString());
    ///         
    /// Assert.AreEqual(output, "42");
    /// Assert.AreEqual(savedValue, "saved");
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect(params Action[] actions)
    {
        actions.ToList().ForEach(action => action());
        return Unit();
    }

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string saved = string.Empty;
    /// 
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         value => Console.WriteLine(value),
    ///         value => saved = value);
    ///         
    /// Assert.AreEqual(saved, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        params Action<T>[] actions) =>
            await input.PipeAsync(value => RunSequential(value, actions));

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string saved = string.Empty;
    /// 
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         // Example using Sequential processing
    ///         ProcessingOrder.Sequential,
    ///         value => Console.WriteLine(value),
    ///         value => saved = value);
    ///         
    /// Assert.AreEqual(saved, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="processingOrder">The order to process the actions.</param>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        params Action<T>[] actions) =>
            await input.PipeAsync(value =>
                processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(value, actions)
                    : RunSequential(value, actions));

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string saved = string.Empty;
    /// CancellationTokenSource source = new();
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         source.Token,
    ///         value => Console.WriteLine(value),
    ///         value => saved = value);
    ///         
    /// Assert.AreEqual(saved, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        CancellationToken cancellationToken,
        params Action<T>[] actions) =>
            await input.PipeAsync(value => RunSequential(value, actions, cancellationToken));

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string saved = string.Empty;
    /// CancellationTokenSource source = new();
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         ProcessingOrder.Sequential,
    ///         source.Token,
    ///         value => Console.WriteLine(value),
    ///         value => saved = value);
    ///         
    /// Assert.AreEqual(saved, "some value");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="processingOrder">The order to process the actions.</param>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action<T>[] actions) =>
            await input.PipeAsync(value =>
                processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(value, actions, cancellationToken)
                    : RunSequential(value, actions, cancellationToken));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string savedValue = string.Empty;
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(
    ///             () => savedValue = "performed effect",
    ///             () => Console.WriteLine("Performing effect"));
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to ignore.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        params Action[] actions) =>
            await input.PipeAsync(() => RunSequential(actions));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string savedValue = string.Empty;
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(
    ///             ProcessingOrder.Sequential,
    ///             () => savedValue = "performed effect",
    ///             () => Console.WriteLine("Performing effect"));
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="processingOrder">The processing order.</param>
    /// <param name="input">The input value to ignore.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        params Action[] actions) =>
        await input.PipeAsync(() =>
            processingOrder == ProcessingOrder.Parallel
                ? RunParallel(actions)
                : RunSequential(actions));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string savedValue = string.Empty;
    /// CancellationTokenSource source = new();
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(
    ///             source.Token,
    ///             () => savedValue = "performed effect",
    ///             () => Console.WriteLine("Performing effect"));
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="input">The input value to ignore.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        CancellationToken cancellationToken,
        params Action[] actions) =>
            await input.PipeAsync(() => RunSequential(actions, cancellationToken));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// string savedValue = string.Empty;
    /// CancellationTokenSource source = new();
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(
    ///             ProcessingOrder.Sequential,
    ///             source.Token,
    ///             () => savedValue = "performed effect",
    ///             () => Console.WriteLine("Performing effect"));
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="processingOrder">The order to process the tasks.</param>
    /// <param name="input">The input value to ignore.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] actions) =>
            await input.PipeAsync(() =>
                processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(actions, cancellationToken)
                    : RunSequential(actions, cancellationToken));

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// Task doAsyncWork(string input) => Task.Run(Console.WriteLine(input));
    /// 
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         value => doAsyncWork(value),
    ///         value => doAsyncWork(value + "!"));
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        params Func<T, Task>[] actions) =>
            await input.PipeAsync(value => RunSequential(value, actions));

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// Task doAsyncWork(string input) => Task.Run(Console.WriteLine(input));
    /// 
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         ProcessingOrder.Sequential,
    ///         value => doAsyncWork(value),
    ///         value => doAsyncWork(value + "!"));
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="processingOrder">The order to process the actions.</param>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        params Func<T, Task>[] actions) =>
            await input.PipeAsync(value =>
                processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(value, actions)
                    : RunSequential(value, actions));

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// CancellationTokenSource source = new();
    /// 
    /// Task doAsyncWork(string input) => Task.Run(Console.WriteLine(input));
    /// 
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         source.Token,
    ///         value => doAsyncWork(value),
    ///         value => doAsyncWork(value + "!"));
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        CancellationToken cancellationToken,
        params Func<T, Task>[] actions) =>
            await input.PipeAsync(value => RunSequential(value, actions, cancellationToken));

    /// <summary>
    /// Perform effects on the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// 
    /// Task doAsyncWork(string input) => Task.Run(Console.WriteLine(input));
    /// CancellationTokenSource source = new();
    /// 
    /// await "some value"
    ///     .Async()
    ///     .EffectAsync(
    ///         ProcessingOrder.Sequential,
    ///         source.Token,
    ///         value => doAsyncWork(value),
    ///         value => doAsyncWork(value + "!"));
    ///         
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="processingOrder">The order to process the tasks.</param>
    /// <param name="input">The input value to perform effects on.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<T, Task>[] actions) =>
            await input.PipeAsync(value =>
                processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(value, actions, cancellationToken)
                    : RunSequential(value, actions, cancellationToken));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doAsyncWork() => Task.Run(Console.WriteLine("Performed effect"));
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(() => doAsyncWork());
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The ignored input value.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        params Func<Task>[] actions) =>
            await input.PipeAsync(() => RunSequential(actions));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doAsyncWork() => Task.Run(Console.WriteLine("Performed effect"));
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(
    ///             ProcessingOrder.Sequential,
    ///             () => doAsyncWork(),
    ///             () => doAsyncWork());
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="processingOrder">The order to process the tasks.</param>
    /// <param name="input">The ignored input value.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        params Func<Task>[] actions) =>
            await input.PipeAsync(() =>
                processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(actions)
                    : RunSequential(actions));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doAsyncWork() => Task.Run(Console.WriteLine("Performed effect"));
    /// CancellationTokenSource source = new();
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(
    ///             source.Token,
    ///             () => doAsyncWork());
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="input">The ignored input value.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        CancellationToken cancellationToken,
        params Func<Task>[] actions) =>
            await input.PipeAsync(() => RunSequential(actions, cancellationToken));

    /// <summary>
    /// Perform effects ignoring the input value.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doAsyncWork() => Task.Run(Console.WriteLine("Performed effect"));
    /// CancellationTokenSource source = new();
    /// 
    /// Unit output = 
    ///     await "ignored value"
    ///         .Async()
    ///         .EffectAsync(
    ///             ProcessingOrder.Sequential,
    ///             source.Token,
    ///             () => doAsyncWork(),
    ///             () => doAsyncWork());
    ///             
    /// Assert.AreEqual(savedValue, "performed effect");
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="processingOrder">The order to process the tasks.</param>
    /// <param name="input">The ignored input value.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Task>[] actions) =>
            await input.PipeAsync(() =>
                processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(actions, cancellationToken)
                    : RunSequential(actions, cancellationToken));

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task DoWork() => EffectAsync(() => Console.WriteLine("Hello, world!"));
    /// await DoWork()
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        params Action[] actions) =>
            await RunSequential(actions);

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task DoWork() => 
    ///     EffectAsync(
    ///         ProcessingOrder.Sequential,
    ///         () => Console.WriteLine("Hello, world!"),
    ///         () => Console.WriteLine("Hello, again..."));
    /// await DoWork()
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="processingOrder">The order to process the tasks.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        ProcessingOrder processingOrder,
        params Action[] actions) =>
            await (processingOrder == ProcessingOrder.Parallel
                    ? RunParallel(actions)
                    : RunSequential(actions));

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// CancellationTokenSource source = new();
    /// Task DoWork() => 
    ///     EffectAsync(
    ///         source.Token,
    ///         () => Console.WriteLine("Hello, world!"));
    ///         
    /// await DoWork()
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        CancellationToken cancellationToken,
        params Action[] actions) =>
            await RunSequential(actions, cancellationToken);

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// CancellationTokenSource source = new();
    /// 
    /// Task DoWork() => 
    ///     EffectAsync(
    ///         ProcessingOrder.Sequential,
    ///         source.Token,
    ///         () => Console.WriteLine("Hello, world!"),
    ///         () => Console.WriteLine("Hello, again..."));
    ///         
    /// await DoWork()
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="processingOrder">The order to process the tasks.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Action[] actions) =>
        await (processingOrder == ProcessingOrder.Parallel
                ? RunParallel(actions, cancellationToken)
                : RunSequential(actions, cancellationToken));

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doWork() => Effect(() => Console.WriteLine("doing work")).Async();
    /// 
    /// await EffectAsync(doWork);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        params Func<Task>[] actions) =>
            await RunSequential(actions);

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doWork() => Effect(() => Console.WriteLine("doing work")).Async();
    /// 
    /// await EffectAsync(
    ///     ProcessingOrder.Sequential,
    ///     () => doWork(),
    ///     () => doWork());
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="processingOrder">The order to process the actions.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        ProcessingOrder processingOrder,
        params Func<Task>[] actions) =>
        await (processingOrder == ProcessingOrder.Parallel
                ? RunParallel(actions)
                : RunSequential(actions));

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doWork() => Effect(() => Console.WriteLine("doing work")).Async();
    /// CancellationTokenSource source = new();
    /// 
    /// await EffectAsync(
    ///     source.Token,
    ///     () => doWork());
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        CancellationToken cancellationToken,
        params Func<Task>[] actions) =>
            await RunSequential(actions, cancellationToken);

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task doWork() => Effect(() => Console.WriteLine("doing work")).Async();
    /// CancellationTokenSource source = new();
    /// 
    /// await EffectAsync(
    ///     ProcessingOrder.Sequential,
    ///     source.Token,
    ///     () => doWork(),
    ///     () => doWork());
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="cancellationToken">A cancellation token.</param>
    /// <param name="processingOrder">The order to process the tasks.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        ProcessingOrder processingOrder,
        CancellationToken cancellationToken,
        params Func<Task>[] actions) =>
        await (processingOrder == ProcessingOrder.Parallel
                ? RunParallel(actions, cancellationToken)
                : RunSequential(actions, cancellationToken));

}