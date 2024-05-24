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

    public static async Task<Unit> RunSequential<T>(
        T input,
        IEnumerable<Action<T>> actions,
        CancellationToken cancellationToken = default)
    {
        await Task.Factory.StartNew(() =>
        {
            foreach (var action in actions)
            {
                if (cancellationToken.IsCancellationRequested) return;
                action(input);
            }
        }, cancellationToken);

        return Unit();
    }

    public static async Task<Unit> RunSequential(
        IEnumerable<Action> actions,
        CancellationToken cancellationToken = default)
    {
        await Task.Factory.StartNew(() =>
        {
            foreach (var action in actions)
            {
                if (cancellationToken.IsCancellationRequested) return;
                action();
            }
        }, cancellationToken);

        return Unit();
    }

    public static async Task<Unit> RunParallel<T>(
        T input,
        IEnumerable<Func<T, Task>> actions,
        CancellationToken cancellationToken = default)
    {
        await Parallel.ForEachAsync(
            actions,
            cancellationToken,
            async (action, token) =>
            {
                if (token.IsCancellationRequested) return;
                await action(input);
            });

        return Unit();
    }

    public static async Task<Unit> RunParallel(
        IEnumerable<Func<Task>> actions,
        CancellationToken cancellationToken = default)
    {
        await Parallel.ForEachAsync(
            actions,
            cancellationToken,
            async (action, token) =>
            {
                if (token.IsCancellationRequested) return;
                await action();
            });

        return Unit();
    }

    public static async Task<Unit> RunParallel<T>(
        T input,
        IEnumerable<Action<T>> actions,
        CancellationToken cancellationToken = default)
    {
        await Parallel.ForEachAsync(
            actions,
            cancellationToken,
            async (action, token) =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Run(() => action(input), cancellationToken);
                }
            });

        return Unit();
    }

    public static async Task<Unit> RunParallel(
        IEnumerable<Action> actions,
        CancellationToken cancellationToken = default)
    {
        await Parallel.ForEachAsync(
            actions,
            cancellationToken,
            async (action, token) =>
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    await Task.Run(() => action(), cancellationToken);
                }
            });

        return Unit();
    }
}
