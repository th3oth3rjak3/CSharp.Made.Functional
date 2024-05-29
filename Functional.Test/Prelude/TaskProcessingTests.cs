namespace Functional.Test.Prelude;

[ExcludeFromCodeCoverage]
[TestClass]
public class TaskProcessingTests
{
    [TestMethod]
    public async Task ItShouldProcessSequentially_1()
    {
        List<int> values = [];

        List<Func<int, Task>> actions =
        [
            value => RunSlowly(value),
            value => RunFast(value + 1)
        ];

        var tokenSource = new CancellationTokenSource();


        await RunSequential(1, actions, tokenSource.Token)
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(2);
        values[0].ShouldBe(1);
        values[1].ShouldBe(2);

        return;

        async Task RunSlowly(int input)
        {
            await Task.Delay(500);
            values.Add(input);
        }

        async Task RunFast(int input)
        {
            await Task.Run(() => values.Add(input));
        }
    }

    [TestMethod]
    public async Task ItShouldProcessSequentially_2()
    {
        List<int> values = [];

        List<Func<Task>> actions =
        [
            () => RunSlowly(42),
            () => RunFast(43)
        ];

        var tokenSource = new CancellationTokenSource();


        await RunSequential(actions, tokenSource.Token)
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(2);
        values[0].ShouldBe(42);
        values[1].ShouldBe(43);

        return;

        async Task RunSlowly(int input)
        {
            await Task.Delay(500);
            values.Add(input);
        }

        async Task RunFast(int input)
        {
            await Task.Run(() => values.Add(input));
        }
    }

    [TestMethod]
    public async Task ItShouldProcessSequentially_3()
    {
        List<int> values = [];

        List<Action<int>> actions =
        [
            value => values.Add(value),
            value => values.Add(value + 1)
        ];

        var tokenSource = new CancellationTokenSource();


        await RunSequential(1, actions, tokenSource.Token)
            .Async()
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(2);
        values[0].ShouldBe(1);
        values[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task ItShouldProcessSequentially_4()
    {
        List<int> values = [];

        List<Action> actions =
        [
            () => values.Add(1),
            () => values.Add(2)
        ];

        var tokenSource = new CancellationTokenSource();


        await RunSequential(actions, tokenSource.Token)
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(2);
        values[0].ShouldBe(1);
        values[1].ShouldBe(2);
        values.ShouldNotContain(42);
    }

    [TestMethod]
    public async Task RunSequentialShouldCancel_1()
    {
        List<int> values = [];

        var tokenSource = new CancellationTokenSource();

        List<Func<int, Task>> actions =
        [
            (value) => Task.Run(() =>
            {
                values.Add(value);
                // Simulating a cancelled token after the first addition.
                tokenSource.Cancel();
            }),
            async (value) =>
            {
                await Task.Delay(10_000);
                await Task.Run(() => values.Add(value + 1));
            }
        ];

        await RunSequential(1, actions, tokenSource.Token)
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(1);
        values[0].ShouldBe(1);
    }

    [TestMethod]
    public async Task RunSequentialShouldCancel_2()
    {
        List<int> values = [];

        var tokenSource = new CancellationTokenSource();

        List<Action> actions =
        [
            () => Task.Run(() =>
            {
                values.Add(1);
                // Simulating a cancelled token after the first addition.
                tokenSource.Cancel();
            }),
            async () =>
            {
                await Task.Delay(10_000);
                await Task.Run(() => values.Add(2));
            }
        ];

        await RunSequential(actions, tokenSource.Token)
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(1);
        values[0].ShouldBe(1);
    }

    [TestMethod]
    public async Task RunSequentialShouldCancel_3()
    {
        List<int> values = [];

        var tokenSource = new CancellationTokenSource();

        List<Action<int>> actions =
        [
            (value) =>
            {
                values.Add(value);
                // Simulating a cancelled token after the first addition.
                tokenSource.Cancel();
            },
            (value) =>
            {
                values.Add(value + 1);
            }
        ];

        await RunSequential(1, actions, tokenSource.Token)
            .Async()
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(1);
        values[0].ShouldBe(1);
    }

    [TestMethod]
    public async Task RunSequentialShouldCancel_4()
    {
        List<int> values = [];

        var tokenSource = new CancellationTokenSource();

        List<Action> actions =
        [
            () =>
            {
                values.Add(1);
                // Simulating a cancelled token after the first addition.
                tokenSource.Cancel();
            },
            () =>
            {
                values.Add(2);
            }
        ];

        await RunSequential(actions, tokenSource.Token)
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(1);
        values[0].ShouldBe(1);
    }

    [TestMethod]
    public async Task RunParallelShouldWork_1()
    {
        var slowOutput = string.Empty;
        var fastOutput = string.Empty;
        List<string> output = [];

        List<Func<int, Task>> actions = [
            input => RunSlowly(input),
            input => RunFast(input + 1)
        ];

        await RunParallel(42, actions);

        output.ShouldContain("42");
        output.ShouldContain("43");

        // This may become a flakey test later, but the idea is that the delay
        // below should cause the runtime to do the fast task first.
        output[0].ShouldBe("43");

        return;

        async Task RunSlowly(int input)
        {
            await Task.Delay(500);
            output.Add(input.ToString());
        }

        async Task RunFast(int input)
        {
            await Task.Run(() => output.Add(input.ToString()));
        }
    }

    [TestMethod]
    public async Task RunParallelShouldWork_2()
    {
        var slowOutput = string.Empty;
        var fastOutput = string.Empty;
        List<string> output = [];

        List<Func<Task>> actions = [
            () => RunSlowly(42),
            () => RunFast(43)
        ];

        await RunParallel(actions);

        output.ShouldContain("42");
        output.ShouldContain("43");

        // This may become a flakey test later, but the idea is that the delay
        // below should cause the runtime to do the fast task first.
        output[0].ShouldBe("43");

        return;

        async Task RunSlowly(int input)
        {
            await Task.Delay(500);
            output.Add(input.ToString());
        }

        async Task RunFast(int input)
        {
            await Task.Run(() => output.Add(input.ToString()));
        }
    }

    [TestMethod]
    public async Task RunParallelShouldWork_3()
    {
        List<int> output = [];

        List<Action<int>> actions = [
            input => output.Add(input),
            input => output.Add(input + 1)
        ];

        await RunParallel(42, actions);

        output.ShouldContain(42);
        output.ShouldContain(43);
    }

    [TestMethod]
    public async Task RunParallelShouldWork_4()
    {
        List<int> output = [];

        List<Action> actions = [
            () => output.Add(42),
            () => output.Add(43)
        ];

        await RunParallel(actions);

        output.ShouldContain(42);
        output.ShouldContain(43);
    }

    [TestMethod]
    public async Task RunParallelShouldCancel_1()
    {
        CancellationTokenSource tokenSource = new();
        List<int> output = [];
        List<Func<int, Task>> actions = [
            input => RunSlow(input),
            input => RunSlow(input + 1)
        ];

        tokenSource.Cancel();
        await RunParallel(42, actions, tokenSource.Token);

        output.ShouldBeEmpty();

        return;

        async Task RunSlow(int input)
        {
            await Task.Delay(5_000);
            output.Add(input);
        }
    }

    [TestMethod]
    public async Task RunParallelShouldCancel_2()
    {
        CancellationTokenSource tokenSource = new();
        List<int> output = [];
        List<Func<Task>> actions = [
            () => RunSlow(42),
            () => RunSlow(43)
        ];

        tokenSource.Cancel();
        await RunParallel(actions, tokenSource.Token);

        output.ShouldBeEmpty();

        return;

        async Task RunSlow(int input)
        {
            await Task.Delay(5_000);
            output.Add(input);
        }
    }

    [TestMethod]
    public async Task RunParallelShouldCancel_3()
    {
        CancellationTokenSource tokenSource = new();
        List<int> output = [];
        List<Action<int>> actions = [
            input => output.Add(input),
            input => output.Add(input + 1)
        ];

        tokenSource.Cancel();
        await RunParallel(42, actions, tokenSource.Token);

        output.ShouldBeEmpty();
    }

    [TestMethod]
    public async Task RunParallelShouldCancel_4()
    {
        CancellationTokenSource tokenSource = new();
        List<int> output = [];
        List<Action> actions = [
            () => output.Add(42),
            () => output.Add(43)
        ];

        tokenSource.Cancel();
        await RunParallel(actions, tokenSource.Token);

        output.ShouldBeEmpty();
    }
}