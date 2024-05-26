namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class MatchTests
{
    [TestMethod]
    public async Task ResultShouldMatchAsync_1()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_2()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_3()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_4()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_5()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok",
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok",
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_6()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok",
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok",
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_7()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                err => err.Message)
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_8()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                err => err.Message.Async())
            .EffectAsync(output => output.ShouldBe("error"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_9()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                () => "err")
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                () => "err")
            .EffectAsync(output => output.ShouldBe("err"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_10()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString(),
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("err"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_11()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                () => "err")
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                () => "err")
            .EffectAsync(output => output.ShouldBe("err"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_12()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("1"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                ok => ok.ToString().Async(),
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("err"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_13()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok",
                () => "err")
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok",
                () => "err")
            .EffectAsync(output => output.ShouldBe("err"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_14()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                () => "err")
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                () => "err")
            .EffectAsync(output => output.ShouldBe("err"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_15()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok",
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok",
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("err"));
    }
    
    [TestMethod]
    public async Task ResultShouldMatchAsync_16()
    {
        await Ok(1)
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("ok"));

        await Error<int>("error")
            .Async()
            .MatchAsync(
                () => "ok".Async(),
                () => "err".Async())
            .EffectAsync(output => output.ShouldBe("err"));
    }
}