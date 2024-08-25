namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class ReduceTests
{
    [TestMethod]
    public async Task ResultShouldReduceAsync_1()
    {
        await Ok(42)
            .Async()
            .ReduceAsync(0)
            .EffectAsync(output => output.ShouldBe(42));

        await Error<int>("error")
            .Async()
            .ReduceAsync(0)
            .EffectAsync(output => output.ShouldBe(0));
    }

    [TestMethod]
    public async Task ResultShouldReduceAsync_2()
    {
        await Ok(42)
            .Async()
            .ReduceAsync(0.Async())
            .EffectAsync(output => output.ShouldBe(42));

        await Error<int>("error")
            .Async()
            .ReduceAsync(0.Async())
            .EffectAsync(output => output.ShouldBe(0));
    }

    [TestMethod]
    public async Task ResultShouldReduceAsync_3()
    {
        await Ok<string, Exception>("hello, world!")
            .Async()
            .ReduceAsync(err => err.Message)
            .EffectAsync(output => output.ShouldBe("hello, world!"));

        await Error<string>("error")
            .Async()
            .ReduceAsync(err => err.Message)
            .EffectAsync(output => output.ShouldBe("error"));
    }

    [TestMethod]
    public async Task ResultShouldReduceAsync_4()
    {
        await Ok<string, Exception>("hello, world!")
            .Async()
            .ReduceAsync(err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("hello, world!"));

        await Error<string>("error")
            .Async()
            .ReduceAsync(err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("error"));
    }

    [TestMethod]
    public async Task ResultShouldReduceAsync_5()
    {
        await Ok(42)
            .Async()
            .ReduceAsync(() => 0)
            .EffectAsync(output => output.ShouldBe(42));

        await Error<int>("error")
            .Async()
            .ReduceAsync(() => 0)
            .EffectAsync(output => output.ShouldBe(0));
    }

    [TestMethod]
    public async Task ResultShouldReduceAsync_6()
    {
        await Ok(42)
            .Async()
            .ReduceAsync(() => 0.Async())
            .EffectAsync(output => output.ShouldBe(42));

        await Error<int>("error")
            .Async()
            .ReduceAsync(() => 0.Async())
            .EffectAsync(output => output.ShouldBe(0));
    }
}