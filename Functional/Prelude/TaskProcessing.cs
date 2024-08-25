namespace Functional;

public static partial class Prelude
{
    /// <summary>
    /// Run multiple tasks sequentially in the order provided. 
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task DoSomeWork(int input) => Task.Run(() => Console.WriteLine(input.ToString()));
    /// 
    /// List&lt;Func&lt;int, Task&gt;&gt; actions = [
    ///     input => DoSomeWork(input),
    ///     input => DoSomeWork(input + 1),
    /// ];
    /// 
    /// // This should print 42, then 43 to the console.
    /// await RunSequential(42, actions);
    /// 
    /// // If you want to pass a token.
    /// CancellationTokenSource tokenSource = new();
    /// await RunSequential(42, actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The type of the input.</typeparam>
    /// <param name="input">The input required for the actions.</param>
    /// <param name="actions">Actions to perform using the input.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> RunSequential<T>(
        T input,
        IEnumerable<Func<T, Task>> actions,
        CancellationToken cancellationToken = default)
    {
        foreach (var action in actions)
        {
            if (cancellationToken.IsCancellationRequested) return Unit();
            await action(input);
        }

        return Unit();
    }

    /// <summary>
    /// Run multiple tasks sequentially in the order provided.
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// List&lt;Func&lt;Task&gt;&gt; actions = [
    ///     () => Task.Run(() => Console.WriteLine("first")),
    ///     () => Task.Run(() => Console.WriteLine("second"))
    /// ];
    /// 
    /// // Should print "first" and then "second" to the console.
    /// await RunSequential(actions);
    /// 
    /// // If you want to pass a token.
    /// CancellationTokenSource tokenSource = new();
    /// await RunSequential(actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="actions">The actions to perform.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> RunSequential(
        IEnumerable<Func<Task>> actions,
        CancellationToken cancellationToken = default)
    {
        foreach (var action in actions)
        {
            if (cancellationToken.IsCancellationRequested) return Unit();
            await action();
        }

        return Unit();
    }

    /// <summary>
    /// Run multiple tasks sequentially in the order provided.
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// List&lt;Action&lt;int&gt;&gt; actions = [
    ///     input => Console.WriteLine(input.ToString()),
    ///     input => Console.WriteLine((input + 1).ToString()),
    /// ];
    /// 
    /// // This should print 42, then 43 to the console.
    /// await RunSequential(42, actions);
    /// 
    /// // If you want to pass a token.
    /// CancellationTokenSource tokenSource = new();
    /// await RunSequential(42, actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input.</param>
    /// <param name="actions">The actions to perform.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> RunSequential<T>(
        T input,
        IEnumerable<Action<T>> actions,
        CancellationToken cancellationToken = default)
    {
        var returnValue = Unit().Async();
        foreach (var action in actions)
        {
            if (cancellationToken.IsCancellationRequested) return await returnValue;
            action(input);
        }

        return await returnValue;
    }

    /// <summary>
    /// Run multiple tasks sequentially in the order provided.
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// List&lt;Action&gt; actions = [
    ///     () => Console.WriteLine("first"),
    ///     () => Console.WriteLine("second"),
    /// ];
    /// 
    /// // This should print "first", then "second" to the console.
    /// await RunSequential(actions);
    /// 
    /// // If you want to pass a token.
    /// CancellationTokenSource tokenSource = new();
    /// await RunSequential(actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="actions">The actions to perform.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static Task<Unit> RunSequential(
        IEnumerable<Action> actions,
        CancellationToken cancellationToken = default)
    {
        foreach (var action in actions)
        {
            if (cancellationToken.IsCancellationRequested) return Unit().Async();
            action();
        }

        return Unit().Async();
    }

    /// <summary>
    /// Run multiple tasks in parallel without regard to order.
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// Task DoSomeWork(int input) => Task.Run(() => Console.WriteLine(input.ToString()));
    /// 
    /// List&lt;Func&lt;int, Task&gt;&gt; actions = [
    ///     input => DoSomeWork(input),
    ///     input => DoSomeWork(input + 1)
    /// ];
    /// 
    /// // This will print "42" and "43" but the order will be determined by the runtime.
    /// await RunParallel(42, actions);
    /// 
    /// // To add CancellationToken support:
    /// CancellationTokenSource tokenSource = new();
    /// await RunParallel(42, actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input required for the actions.</param>
    /// <param name="actions">Actions to be performed.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> RunParallel<T>(
        T input,
        IEnumerable<Func<T, Task>> actions,
        CancellationToken cancellationToken = default)
    {
        ParallelOptions options = new()
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        try
        {
            await Parallel.ForEachAsync(
                actions,
                options,
                async (action, token) =>
                {
                    if (token.IsCancellationRequested) return;
                    await action(input);
                });
        }
        catch (OperationCanceledException)
        {
            return Unit();
        }

        return Unit();
    }

    /// <summary>
    /// Run multiple tasks in parallel without regard to order.
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// List&lt;Func&lt;Task&gt;&gt; actions = [
    ///     () => Task.Run(() => Console.WriteLine("maybe first")),
    ///     () => Task.Run(() => Console.WriteLine("maybe second"))
    /// ];
    /// 
    /// // This will print "maybe first" and "maybe second" but the order will be determined by the runtime.
    /// await RunParallel(actions);
    /// 
    /// // To add CancellationToken support:
    /// CancellationTokenSource tokenSource = new();
    /// await RunParallel(actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="actions">The actions to perform.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> RunParallel(
        IEnumerable<Func<Task>> actions,
        CancellationToken cancellationToken = default)
    {
        ParallelOptions options = new()
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        try
        {
            await Parallel.ForEachAsync(
                actions,
                options,
                async (action, token) =>
                {
                    if (token.IsCancellationRequested) return;
                    await action();
                });
        }
        catch (OperationCanceledException)
        {
            return Unit();
        }

        return Unit();
    }

    /// <summary>
    /// Run multiple tasks in parallel without regard to order.
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// List&lt;Action&lt;int&gt;&gt; actions = [
    ///     input => Console.WriteLine(input),
    ///     input => Console.WriteLine(input + 1)
    /// ];
    /// 
    /// // This will print "42" and "43" but the order will be determined by the runtime.
    /// await RunParallel(42, actions);
    /// 
    /// // To add CancellationToken support:
    /// CancellationTokenSource tokenSource = new();
    /// await RunParallel(42, actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <typeparam name="T">The input type.</typeparam>
    /// <param name="input">The input required for the actions.</param>
    /// <param name="actions">Actions to be performed.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> RunParallel<T>(
        T input,
        IEnumerable<Action<T>> actions,
        CancellationToken cancellationToken = default)
    {
        ParallelOptions options = new()
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        try
        {
            await Parallel.ForEachAsync(
                actions,
                options,
                (action, token) =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        action(input);
                    }

                    return ValueTask.CompletedTask;
                });
        }
        catch (OperationCanceledException)
        {
            return Unit();
        }

        return Unit();
    }

    /// <summary>
    /// Run multiple tasks in parallel without regard to order.
    /// When cancelled, the operation will return early and may not finish processing actions.
    /// <example>
    /// <br/><br/>Example:
    /// <code>
    /// List&lt;Action&gt; actions = [
    ///     () => Console.WriteLine("maybe first"),
    ///     () => Console.WriteLine("maybe second")
    /// ];
    /// 
    /// // This will print "maybe first" and "maybe second" but the order will be determined by the runtime.
    /// await RunParallel(actions);
    /// 
    /// // To add CancellationToken support:
    /// CancellationTokenSource tokenSource = new();
    /// await RunParallel(actions, tokenSource.Token);
    /// </code>
    /// </example>
    /// </summary>
    /// <param name="actions">The actions to perform.</param>
    /// <param name="cancellationToken">An optional cancellation token.</param>
    /// <returns>Unit.</returns>
    public static async Task<Unit> RunParallel(
        IEnumerable<Action> actions,
        CancellationToken cancellationToken = default)
    {
        ParallelOptions options = new()
        {
            CancellationToken = cancellationToken,
            MaxDegreeOfParallelism = Environment.ProcessorCount
        };

        try
        {
            await Parallel.ForEachAsync(
                actions,
                options,
                (action, token) =>
                {
                    if (!token.IsCancellationRequested)
                    {
                        action();
                    }

                    return ValueTask.CompletedTask;
                });
        }
        catch (OperationCanceledException)
        {
            return Unit();
        }

        return Unit();
    }
}