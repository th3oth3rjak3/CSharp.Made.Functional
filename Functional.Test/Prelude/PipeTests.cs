namespace Functional.Test.Prelude.Common;

[TestClass]
public class PipeTests
{
    [TestMethod]
    public void ItShouldPipeValues()
    {
        42.Pipe(value => value.ToString()).ShouldBe("42");
    }

    [TestMethod]
    public void ItShouldPipeAndReturnUnit()
    {
        var list = new List<int>();
        var input = 1;

        input
            .Pipe(list.Add, input => list.Add(input + 1))
            .Pipe(unit => unit.ShouldBeOfType<Unit>());

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public void ItShouldPipeIgnoringTheInput() =>
        "something to ignore"
            .Pipe(() => Unit.Default)
            .ShouldBe(Unit.Default);

    [TestMethod]
    public async Task ItShouldPipeAndReturnUnitAsync()
    {
        var list = new List<int>();
        var input = 1;

        await input
            .Async()
            .PipeAsync(list.Add, input => list.Add(input + 1))
            .PipeAsync(unit => unit.ShouldBeOfType<Unit>());

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task ItShouldPipeAndReturnUnitAsyncWithTaskReturns()
    {
        var list = new List<int>();
        var input = 1;

        Task addToList(int input) => Task.Run(() => list.Add(input));

        await input
            .Async()
            .PipeAsync(addToList, input => addToList(input + 1))
            .PipeAsync(unit => unit.ShouldBeOfType<Unit>());

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task ItShouldPipeAndReturnUnitAsyncWithActions()
    {
        var list = new List<int>();
        var input = 1;

        await input
            .Async()
            .PipeAsync(() => list.Add(42), () => list.Add(43))
            .PipeAsync(unit => unit.ShouldBeOfType<Unit>());

        list.Count.ShouldBe(2);
        list[0].ShouldBe(42);
        list[1].ShouldBe(43);
    }

    [TestMethod]
    public async Task ItShouldPipeAndIgnoreInput() =>
        await "ignored input"
            .Async()
            .PipeAsync(() => Unit.Default)
            .TapAsync(value => value.ShouldBeOfType<Unit>())
            .IgnoreAsync();

    [TestMethod]
    public void PipeShouldHandleActionsWithNoInput()
    {
        var list = new List<int>();

        "ignored"
            .Pipe(
                () => list.Add(1),
                () => list.Add(2))
            .ShouldBeOfType<Unit>();

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task ItShouldPipeAsyncInputWithNonTaskMapping() =>
        await 1.Async()
            .PipeAsync(num => num.ToString())
            .TapAsync(str => str.ShouldBeEquivalentTo("1"));

    [TestMethod]
    public async Task ItShouldPipeAsyncInputWithTaskMapping() =>
        await 1.Async()
            .PipeAsync(num => Task.FromResult(num.ToString()))
            .TapAsync(str => str.ShouldBe("1"));
}
