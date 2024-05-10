using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Functional.Common;
using Functional.Options;

using static Functional.Common.CommonExtensions;

namespace Functional.Test.Common;

[ExcludeFromCodeCoverage]
[TestClass]
public class CommonExtensionTests
{
    /// <summary>
    /// Test what happens when when converting an int to a string.
    /// It should succeed.
    /// </summary>
    [TestMethod]
    public void MapShouldConvertIntToString() =>
        1.Pipe(value => value.ToString())
            .ShouldBe("1");

    /// <summary>
    /// Test what happens when mutating state that returns void.
    /// The resulting object passed out of the Tap should be updated.
    /// </summary>
    [TestMethod]
    public void ItShouldTapResults() =>
        new TestObject { Value = 1 }
            .Tap(objToChange => objToChange.Value = 2)
            .ShouldBeEquivalentTo(new TestObject() { Value = 2 });

    /// <summary>
    /// This should not change the referential object, but create a new one.
    /// The ignore method only ignores the output of the func delegate in Tap.
    /// If the object was mutated, the Value property would update to 2 regardless
    /// of the func delegate output.
    /// </summary>
    [TestMethod]
    public void ItShouldTapResultsWithOldValues() =>
        new TestRecord(1)
            .Tap(objToChange => (objToChange with { Value = 2 }).Ignore())
            .ShouldBe(new TestRecord(1));

    /// <summary>
    /// Test to ensure that values are wrapped inside of a Task
    /// after the call to AsAsync.
    /// </summary>
    [TestMethod]
    public void ItShouldWrapAsAsync() =>
        1.AsAsync()
            .ShouldBeEquivalentTo(Task.FromResult(1));

    /// <summary>
    /// The tap method should handle values that are tasks without async code in the
    /// lambda to make async/await simpler.
    /// </summary>
    /// <returns>An awaitable task.</returns>
    [TestMethod]
    public async Task ItShouldTapAsync() =>
        await new List<int> { 1 }
            .AsAsync()
            .TapAsync(lst => lst.Add(2))
            .TapAsync(lst => lst.ShouldBeEquivalentTo(new List<int> { 1, 2 }));

    /// <summary>
    /// It should handle non-task type values as inputs and actions that transform
    /// values into tasks.
    /// </summary>
    /// <returns>An awaitable task.</returns>
    [TestMethod]
    public async Task ItShouldTapAsyncOnlyWhenTIsTask() =>
        await new List<int> { 1 }
            .AsAsync()
            .TapAsync(lst => AddAsync(lst, 2))
            .TapAsync(lst => lst.ShouldBeEquivalentTo(new List<int> { 1, 2 }));

    private static async Task AddAsync<T>(List<T> list, T item)
    {
        await Task.Delay(5000);
        _ = await list.AsAsync().TapAsync(lst => lst.Add(item));
    }

    /// <summary>
    /// It should take a value that is a task and a mapping function which
    /// maps a non-task input to a non-task output.
    /// </summary>
    /// <returns>An awaitable task.</returns>
    [TestMethod]
    public async Task ItShouldPipeAsyncInputWithNonTaskMapping() =>
        await 1.AsAsync()
            .PipeAsync(num => num.ToString())
            .TapAsync(str => str.ShouldBeEquivalentTo("1"));

    /// <summary>
    /// It should take a value that is a task and a mapping function which 
    /// maps a non-task to a task type output.
    /// </summary>
    /// <returns>An awaitable task.</returns>
    [TestMethod]
    public async Task ItShouldPipeAsyncInputWithTaskMapping() =>
        await 1.AsAsync()
            .PipeAsync(num => Task.FromResult(num.ToString()))
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public void ConsShouldAddItemsToAnImmutableList() =>
        new List<string>
        {
            "1",
            "2",
        }
        .Pipe(strings => ImmutableList<string>.Empty.AddRange(strings))
        .Tap(strings => Cons("1", "2").ShouldBeEquivalentTo(strings))
        .Ignore();

    /// <summary>
    /// A test object to mock data mutation behavior.
    /// </summary>
    private class TestObject
    {
        internal int Value { get; set; }

        internal void AddOne() => Value++;

        internal Task<int> AddOneAsync()
        {
            AddOne();
            return Value.AsAsync();
        }
    }

    /// <summary>
    /// A test record to verify behavior mapping with records.
    /// </summary>
    /// <param name="Value">The value to store.</param>
    private record TestRecord(int Value);

    [TestMethod]
    public async Task ItShouldAllowActionsPassedToTapAsync() =>
        await new TestObject { Value = 1 }
            .AsAsync()
            .TapAsync(obj => obj.AddOne())
            .TapAsync(obj => obj.Value.ShouldBe(2));

    [TestMethod]
    public async Task ItShouldAllowTaskMappersToBePassedToTapAsync() =>
        await new TestObject { Value = 1 }
            .AsAsync()
            .PipeAsync(obj => obj.AddOneAsync())
            .TapAsync(Task.FromResult)
            .TapAsync(number => number.ShouldBe(2));

    [TestMethod]
    public void ItShouldTapWithNonReturningActions()
    {
        var result1 = false;
        var result2 = false;

        "string"
            .Tap(() => result1 = true, () => result2 = true)
            .Tap(value => value.ShouldBe("string"));

        result1.ShouldBeTrue();
        result2.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldPipeAndReturnUnit()
    {
        var list = new List<int>();
        var input = 1;

        input
            .Pipe(input => list.Add(input), input => list.Add(input + 1))
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
            .AsAsync()
            .PipeAsync(input => list.Add(input), input => list.Add(input + 1))
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
            .AsAsync()
            .PipeAsync(input => addToList(input), input => addToList(input + 1))
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
            .AsAsync()
            .PipeAsync(() => list.Add(42), () => list.Add(43))
            .PipeAsync(unit => unit.ShouldBeOfType<Unit>());

        list.Count.ShouldBe(2);
        list[0].ShouldBe(42);
        list[1].ShouldBe(43);
    }

    [TestMethod]
    public async Task ItShouldPipeAndIgnoreInput() =>
        await "ignored input"
            .AsAsync()
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
    public void EffectShouldHandleActionsWithInput()
    {
        var effectResult = string.Empty;

        "input"
            .Effect(input => effectResult = input)
            .ShouldBeOfType<Unit>();

        effectResult.ShouldBe("input");
    }

    [TestMethod]
    public void EffectShouldHandleActionsWithNoInput()
    {
        var effectResult = string.Empty;

        "ignored"
            .Effect(() => effectResult = "input")
            .ShouldBeOfType<Unit>();

        effectResult.ShouldBe("input");
    }

    [TestMethod]
    public async Task EffectAsyncShouldHandleActionsWithInput()
    {
        var effectResult = string.Empty;

        await "input"
            .AsAsync()
            .EffectAsync(input => effectResult = input)
            .EffectAsync(unit => unit.ShouldBeOfType<Unit>());

        effectResult.ShouldBe("input");
    }

    [TestMethod]
    public async Task EffectAsyncShouldHandleActionsWithNoInput()
    {
        var list = new List<int>();

        await "ignored"
            .AsAsync()
            .EffectAsync(
                () => list.Add(1),
                () => list.Add(2))
            .EffectAsync(unit => unit.ShouldBeOfType<Unit>());

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task EffectShouldHandleActionsWithNoInputAndTaskOutput()
    {
        var list = new List<int>();

        async Task addListItem(int input) => await input.AsAsync().EffectAsync(input => list.Add(input));

        await "ignored input"
            .AsAsync()
            .EffectAsync(() => addListItem(1), () => addListItem(2));

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }

    [TestMethod]
    public async Task EffectShouldHandleActionsWithInputAndTaskOutput()
    {
        var list = new List<int>();

        async Task addListItem(int input) => await input.AsAsync().EffectAsync(input => list.Add(input));

        await 1
            .AsAsync()
            .EffectAsync(value => addListItem(value), value => addListItem(value + 1));

        list.Count.ShouldBe(2);
        list[0].ShouldBe(1);
        list[1].ShouldBe(2);
    }


    [TestMethod]
    public void EffectShouldWorkWithoutExtensionMethod()
    {
        var effectResult = false;
        Effect(() => effectResult = true).ShouldBeOfType<Unit>();
        effectResult.ShouldBeTrue();
    }
}
