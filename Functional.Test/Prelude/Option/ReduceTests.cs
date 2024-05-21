namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class ReduceTests
{
    [TestMethod]
    public async Task OptionsShouldReduceAsync()
    {
        await "some value"
            .AsNullable()
            .Async()
            .Optional()
            .ReduceAsync(() => "none")
            .EffectAsync(output => output.ShouldBe("some value"));

        await (null as string)
            .Async()
            .Optional()
            .ReduceAsync(() => "none")
            .EffectAsync(output => output.ShouldBe("none"));

        await "some value"
            .AsNullable()
            .Async()
            .Optional()
            .ReduceAsync("none")
            .EffectAsync(output => output.ShouldBe("some value"));

        await (null as string)
            .Async()
            .Optional()
            .ReduceAsync("none")
            .EffectAsync(output => output.ShouldBe("none"));


        await "some value"
            .AsNullable()
            .Async()
            .Optional()
            .ReduceAsync(() => "none".Async())
            .EffectAsync(output => output.ShouldBe("some value"));

        await (null as string)
            .Async()
            .Optional()
            .ReduceAsync(() => "none".Async())
            .EffectAsync(output => output.ShouldBe("none"));

        await "some value"
            .AsNullable()
            .Async()
            .Optional()
            .ReduceAsync("none".Async())
            .EffectAsync(output => output.ShouldBe("some value"));

        await (null as string)
            .Async()
            .Optional()
            .ReduceAsync("none".Async())
            .EffectAsync(output => output.ShouldBe("none"));
    }
}