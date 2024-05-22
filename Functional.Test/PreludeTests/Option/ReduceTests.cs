namespace Functional.Test.PreludeTests.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class ReduceTests
{
    [DataRow("some value", "some value")]
    [DataRow(null, "none")]
    [TestMethod]
    public async Task OptionsShouldReduceAsync(string? input, string expected)
    {
        await input
            .Async()
            .Optional()
            .ReduceAsync(() => "none")
            .EffectAsync(output => output.ShouldBe(expected));

        await input
            .Async()
            .Optional()
            .ReduceAsync("none")
            .EffectAsync(output => output.ShouldBe(expected));

        await input
            .Async()
            .Optional()
            .ReduceAsync(() => "none".Async())
            .EffectAsync(output => output.ShouldBe(expected));

        await input
            .Async()
            .Optional()
            .ReduceAsync("none".Async())
            .EffectAsync(output => output.ShouldBe(expected));
    }
}