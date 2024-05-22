namespace Functional.Test.PreludeTests.Common;

[TestClass]
public class PipeTests
{
    [TestMethod]
    public void ItShouldPipeValues() =>
        42.Pipe(value => value.ToString())
            .ShouldBe("42");

    [TestMethod]
    public void ItShouldPipeIgnoringTheInput() =>
        "something to ignore"
            .Pipe(() => Unit())
            .ShouldBeOfType<Unit>();

    [TestMethod]
    public async Task ItShouldPipeAndIgnoreInput() =>
        await "ignored input"
            .Async()
            .PipeAsync(() => Unit())
            .TapAsync(value => value.ShouldBeOfType(typeof(Unit)))
            .IgnoreAsync();

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
