namespace Functional;

public static partial class Prelude
{
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

    public static Task<Unit> RunSequential<T>(
        T input,
        IEnumerable<Action<T>> actions,
        CancellationToken cancellationToken = default)
    {
        var returnValue = Unit().Async();
        foreach (var action in actions)
        {
            if (cancellationToken.IsCancellationRequested) return returnValue;
            action(input);
        }

        return returnValue;
    }

    public static Task<Unit> RunSequential(
        IEnumerable<Action> actions,
        CancellationToken cancellationToken = default)
    {
        var returnValue = Unit().Async();
        foreach (var action in actions)
        {
            if (cancellationToken.IsCancellationRequested) return returnValue;
            action();
        }

        return returnValue;
    }

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
        catch (OperationCanceledException _)
        {
            return Unit();
        }

        return Unit();
    }

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
        catch (OperationCanceledException _)
        {
            return Unit();
        }

        return Unit();
    }

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
        catch (OperationCanceledException _)
        {
            return Unit();
        }

        return Unit();
    }

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
        catch (OperationCanceledException _)
        {
            return Unit();
        }

        return Unit();
    }
}