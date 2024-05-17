namespace Functional.Test.Types.Option;

[TestClass]
public class OptionTests
{
    [TestMethod]
    public void OptionShouldConstructCorrectly()
    {
        new Option<string>("value").IsSome.ShouldBeTrue();
        new Option<string>(null).IsNone.ShouldBeTrue();
        new Option<string>().IsNone.ShouldBeTrue();

        new Option<int>(42).IsSome.ShouldBeTrue();
        new Option<int>().IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public void OptionShouldUnwrap()
    {
        var option = new Option<string>("some value");
        option.Unwrap().ShouldBe("some value");

        Assert.ThrowsException<InvalidOperationException>(() => new Option<string>().Unwrap());
    }

    [TestMethod]
    public void OptionShouldHaveExhaustiveMatch()
    {
        new Option<string>("some value")
            .Match(value => value, () => "none")
            .ShouldBe("some value");

        new Option<string>()
            .Match(value => value, () => "none")
            .ShouldBe("none");
    }

    [TestMethod]
    public void OptionShouldPerformEffects()
    {
        var some = new Option<string>("some value");
        var none = new Option<string>();

        var someResult = string.Empty;
        var noneResult = string.Empty;

        void reset()
        {
            someResult = string.Empty;
            noneResult = string.Empty;
        }

        some.Effect(some => someResult = some, () => noneResult = "none").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe("some value");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.Effect(some => someResult = some, () => noneResult = "none").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");

        reset();

        some.Effect(() => someResult = "some", () => noneResult = "none").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.Effect(() => someResult = "some", () => noneResult = "none").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }
}