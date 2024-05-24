using Functional.Types;

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
    public static Unit Effect<T>(this T input, params Action<T>[] actions) =>
        input.Tap(actions).Pipe(_ => Unit());

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
    /// <param name="input">The ignored input.</param>
    /// <param name="actions">Actions to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect<T>(this T input, params Action[] actions) =>
        input.Tap(actions).Pipe(_ => Unit());

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
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static Unit Effect(Action action)
    {
        action();
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
        IEnumerable<Action<T>> actions,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await input.PipeAsync(input =>
                processingOrder switch
                {
                    ProcessingOrder.Sequential => RunSequential(input, actions, cancellationToken),
                    ProcessingOrder.Parallel => RunParallel(input, actions, cancellationToken),
                    _ => Unit().Async()
                });

    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        Action<T> action,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
        await input.PipeAsync(input =>
            processingOrder switch
            {
                ProcessingOrder.Sequential => RunSequential(input, [action], cancellationToken),
                ProcessingOrder.Parallel => RunParallel(input, [action], cancellationToken),
                _ => Unit().Async()
            });



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
        IEnumerable<Action> actions,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await input.PipeAsync(() =>
                processingOrder switch
                {
                    ProcessingOrder.Sequential => RunSequential(actions, cancellationToken),
                    ProcessingOrder.Parallel => RunParallel(actions, cancellationToken),
                    _ => Unit().Async()
                });

    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        Action action,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await input.PipeAsync(() =>
                processingOrder switch
                {
                    ProcessingOrder.Sequential => RunSequential([action], cancellationToken),
                    ProcessingOrder.Parallel => RunParallel([action], cancellationToken),
                    _ => Unit().Async()
                });

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
        IEnumerable<Func<T, Task>> actions,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await input.PipeAsync(input =>
                processingOrder switch
                {
                    ProcessingOrder.Sequential => RunSequential(input, actions, cancellationToken),
                    ProcessingOrder.Parallel => RunParallel(input, actions, cancellationToken),
                    _ => Unit().Async()
                });

    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        Func<T, Task> action,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await input.PipeAsync(input =>
                processingOrder switch
                {
                    ProcessingOrder.Sequential => RunSequential(input, [action], cancellationToken),
                    ProcessingOrder.Parallel => RunParallel(input, [action], cancellationToken),
                    _ => Unit().Async()
                });

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
        IEnumerable<Func<Task>> actions,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await input.PipeAsync(() =>
                processingOrder switch
                {
                    ProcessingOrder.Sequential => RunSequential(actions, cancellationToken),
                    ProcessingOrder.Parallel => RunParallel(actions, cancellationToken),
                    _ => Unit().Async()
                });

    public static async Task<Unit> EffectAsync<T>(
        this Task<T> input,
        Func<Task> action,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await input.PipeAsync(() =>
                processingOrder switch
                {
                    ProcessingOrder.Sequential => RunSequential([action], cancellationToken),
                    ProcessingOrder.Parallel => RunParallel([action], cancellationToken),
                    _ => Unit().Async()
                });

    public static async Task<Unit> EffectAsync(
        IEnumerable<Action> actions,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await (processingOrder switch
            {
                ProcessingOrder.Sequential => RunSequential(actions, cancellationToken),
                ProcessingOrder.Parallel => RunParallel(actions, cancellationToken),
                _ => Unit().Async()
            });

    /// <summary>
    /// Perform an effect which returns unit.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task somethingToDo() => EffectAsync(() => Console.WriteLine("Hello, world!"));
    /// await somethingToDo()
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="action">An action to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        Action action,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await (processingOrder switch
            {
                ProcessingOrder.Sequential => RunSequential([action], cancellationToken),
                ProcessingOrder.Parallel => RunParallel([action], cancellationToken),
                _ => Unit().Async()
            });

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
        IEnumerable<Func<Task>> actions,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await (processingOrder switch
            {
                ProcessingOrder.Sequential => RunSequential(actions, cancellationToken),
                ProcessingOrder.Parallel => RunParallel(actions, cancellationToken),
                _ => Unit().Async()
            });

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
    /// <param name="action">Action to perform.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> EffectAsync(
        Func<Task> action,
        ProcessingOrder processingOrder = ProcessingOrder.Sequential,
        CancellationToken cancellationToken = default) =>
            await (processingOrder switch
            {
                ProcessingOrder.Sequential => RunSequential([action], cancellationToken),
                ProcessingOrder.Parallel => RunParallel([action], cancellationToken),
                _ => Unit().Async()
            });
}
