namespace Functional.Test.PreludeTests.Common;

[ExcludeFromCodeCoverage]
[TestClass]
public class TapTests
{

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
            return Value.Async();
        }
    }

    /// <summary>
    /// A test record to verify behavior mapping with records.
    /// </summary>
    /// <param name="Value">The value to store.</param>
    private record TestRecord(int Value);

    [TestMethod]
    public void ItShouldTapResults() =>
        new TestObject { Value = 1 }
            .Tap(objToChange => objToChange.Value = 2)
            .ShouldBeEquivalentTo(new TestObject() { Value = 2 });

    [TestMethod]
    public void ItShouldTapResultsWithOldValues() =>
        new TestRecord(1)
            .Tap(objToChange => (objToChange with { Value = 2 }).Ignore())
            .ShouldBe(new TestRecord(1));

    [TestMethod]
    public async Task ItShouldAllowActionsPassedToTapAsync() =>
        await new TestObject { Value = 1 }
            .Async()
            .TapAsync(obj => obj.AddOne())
            .TapAsync(obj => obj.Value.ShouldBe(2));

    [TestMethod]
    public async Task ItShouldAllowTaskMappersToBePassedToTapAsync() =>
        await new TestObject { Value = 1 }
            .Async()
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
    public async Task ItShouldTapAsync() =>
    await new List<int> { 1 }
        .Async()
        .TapAsync(lst => lst.Add(2))
        .TapAsync(lst => lst.ShouldBeEquivalentTo(new List<int> { 1, 2 }));
}
