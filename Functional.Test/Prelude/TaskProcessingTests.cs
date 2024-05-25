namespace Functional.Test.Prelude;

[TestClass]
public class TaskProcessingTests
{
    [TestMethod]
    public async Task ItShouldProcessSequentially_1()
    {
        List<int> values = [];

        List<Func<int, Task>> actions =
        [
            (value) => Task.Run(() => values.Add(value)),
            (value) => Task.Run(() => values.Add(value + 1))
        ];

        var tokenSource = new CancellationTokenSource();


        await RunSequential(1, actions, tokenSource.Token)
            .AssertInstanceOfType(typeof(Task<Unit>));

        values.Count.ShouldBe(2);
        values[0].ShouldBe(1);
        values[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task ItShouldProcessSequentially_2()
    {
        List<int> values = [];

        List<Func<Task>> actions =
        [
            () => Task.Run(() => values.Add(1)),
            () => Task.Run(() => values.Add(2))
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
}