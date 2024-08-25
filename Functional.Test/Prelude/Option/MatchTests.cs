namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class MatchTests
{
    private static string MatchSome() => "Some";
    private static Task<string> MatchSomeAsync() => MatchSome().Async();
    private static string MatchSomeInput(int input) => input.ToString();
    private static Task<string> MatchSomeInputAsync(int input) => MatchSomeInput(input).Async();
    private static string MatchNone() => "None";
    private static Task<string> MatchNoneAsync() => MatchNone().Async();

    [TestMethod]
    public async Task OptionShouldMatchAsync_1()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInput, MatchNone)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInput, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));
    }

    [TestMethod]
    public async Task OptionShouldMatchAsync_2()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));
    }

    [TestMethod]
    public async Task OptionShouldMatchAsync_3()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInput, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInput, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));
    }

    [TestMethod]
    public async Task OptionShouldMatchAsync_4()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));
    }

    [TestMethod]
    public async Task OptionShouldMatchAsync_5()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSome, MatchNone)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSome, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));
    }

    [TestMethod]
    public async Task OptionShouldMatchAsync_6()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));
    }

    [TestMethod]
    public async Task OptionShouldMatchAsync_7()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSome, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSome, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));
    }

    [TestMethod]
    public async Task OptionShouldMatchAsync_8()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));
    }
}