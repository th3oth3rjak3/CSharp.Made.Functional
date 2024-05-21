namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class MatchTests
{
    [TestMethod]
    public async Task OptionShouldMatchAsync()
    {
        static string matchSomeInput(int input) => input.ToString();
        static string matchSome() => "Some";
        static Task<string> matchSomeAsync() => matchSome().Async();
        static Task<string> matchSomeInputAsync(int input) => matchSomeInput(input).Async();
        static string matchNone() => "None";
        static Task<string> matchNoneAsync() => matchNone().Async();

        await Some(42)
            .Async()
            .MatchAsync(matchSomeInput, matchNone)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(matchSomeInput, matchNone)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(matchSomeInputAsync, matchNone)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(matchSomeInputAsync, matchNone)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(matchSomeInput, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(matchSomeInput, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(matchSomeInputAsync, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("42"));

        await None<int>()
            .Async()
            .MatchAsync(matchSomeInputAsync, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(matchSome, matchNone)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(matchSome, matchNone)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(matchSomeAsync, matchNone)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(matchSomeAsync, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(matchSome, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(matchSome, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));

        await Some(42)
            .Async()
            .MatchAsync(matchSomeAsync, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("Some"));

        await None<int>()
            .Async()
            .MatchAsync(matchSomeAsync, matchNoneAsync)
            .EffectAsync(output => output.ShouldBe("None"));
    }
}
