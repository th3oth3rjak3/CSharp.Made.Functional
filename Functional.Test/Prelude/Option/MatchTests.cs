namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class MatchTests
{
    // TODO: Break this up into multiple tests. See TapTests for inspo.
    [TestMethod]
    public async Task OptionShouldMatchAsync()
    {
        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInput, MatchNone)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInput, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInput, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInput, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeInputAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(MatchSome, MatchNone)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSome, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNone)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(MatchSome, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSome, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(MatchSomeAsync, MatchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));
        return;

        static string MatchSome() => "Some";
        static Task<string> MatchSomeAsync() => MatchSome().Async();
        static string MatchSomeInput(int input) => input.ToString();
        static Task<string> MatchSomeInputAsync(int input) => MatchSomeInput(input).Async();
        static string MatchNone() => "None";
        static Task<string> MatchNoneAsync() => MatchNone().Async();
    }
}