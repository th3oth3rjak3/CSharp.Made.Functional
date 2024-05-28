namespace Functional.Test.Types.Option;

[ExcludeFromCodeCoverage]
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
    public void OptionShouldHaveAnImplicitOperator()
    {
        static Option<string> DoWork(int input)
        {
            if (input < 20) return input.ToString();
            return None<string>();
        }

        DoWork(10).IsSome.ShouldBeTrue();
        DoWork(30).IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public void OptionShouldUnwrap()
    {
        var option = new Option<string>("some value");
        option.Unwrap().ShouldBe("some value");

        Assert.ThrowsException<OptionUnwrapException>(() => new Option<string>().Unwrap());
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

        new Option<string>("some value")
            .Match(() => "some", () => "none")
            .ShouldBe("some");

        new Option<string>()
            .Match(() => "some", () => "none")
            .ShouldBe("none");
    }

    [TestMethod]
    public void OptionShouldMap()
    {
        new Option<int>(42)
            .Map(value => value.ToString())
            .Unwrap()
            .ShouldBe("42");

        new Option<int>()
            .Map(value => value.ToString())
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();

        new Option<int>(42)
            .Map(() => "hello, world!")
            .Unwrap()
            .ShouldBe("hello, world!");

        new Option<int>()
            .Map(() => "hello, world!")
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();
    }

    [TestMethod]
    public void OptionShouldFilter()
    {
        static bool filterCriteria(string input) => input.Length < 10;

        Some("short")
            .Filter(filterCriteria)
            .IsSome
            .ShouldBeTrue();

        Some("a really long message that should get filtered out")
            .Filter(filterCriteria)
            .IsNone
            .ShouldBeTrue();

        None<string>()
            .Filter(filterCriteria)
            .IsNone
            .ShouldBeTrue();
    }

    [TestMethod]
    public void OptionShouldReduce()
    {
        new Option<string>("some value")
            .Reduce("none")
            .ShouldBe("some value");

        new Option<string>("some value")
            .Reduce(() => "none")
            .ShouldBe("some value");

        new Option<string>()
            .Reduce("none")
            .ShouldBe("none");

        new Option<string>()
            .Reduce(() => "none")
            .ShouldBe("none");
    }

    [TestMethod]
    public void OptionShouldBind()
    {
        Option<string> TryGetString(int input) =>
            input > 10
                ? new Option<string>(input.ToString())
                : new Option<string>();

        Some(42)
            .Bind(TryGetString)
            .Effect(
                some => some.IsSome.ShouldBeTrue(),
                some => some.Unwrap().ShouldBe("42"));

        Some(8)
            .Bind(TryGetString)
            .Effect(none => none.IsNone.ShouldBeTrue());

        None<int>()
            .Bind(TryGetString)
            .Effect(none => none.IsNone.ShouldBeTrue());

        Some(42)
            .Bind(() => TryGetString(30))
            .Effect(
                some => some.IsSome.ShouldBeTrue(),
                some => some.Unwrap().ShouldBe("30"));

        Some(42)
            .Bind(() => TryGetString(8))
            .Effect(none => none.IsNone.ShouldBeTrue());

        None<int>()
            .Bind(() => TryGetString(100))
            .Effect(none => none.IsNone.ShouldBeTrue());
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

        some.Effect(s => someResult = s, () => noneResult = "none").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe("some value");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.Effect(s => someResult = s, () => noneResult = "none").AssertInstanceOfType(typeof(Unit));
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

    [TestMethod]
    public void OptionShouldPerformEffectsWhenSome()
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

        some.EffectSome(s => someResult = s).AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe("some value");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.EffectSome(s => someResult = s).AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);

        reset();

        some.EffectSome(() => someResult = "some").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.EffectSome(() => someResult = "some").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public void OptionShouldPerformEffectsWhenNone()
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

        some.EffectNone(() => noneResult = "none").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);

        reset();

        none.EffectNone(() => noneResult = "none").AssertInstanceOfType(typeof(Unit));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");
    }

    [TestMethod]
    public void OptionShouldTapAndReturnSelf()
    {
        var someResult = string.Empty;
        var noneResult = string.Empty;

        void setSomeInput(string input) => someResult = input;
        void setSome() => someResult = "some";
        void setNone() => noneResult = "none";

        void reset()
        {
            someResult = string.Empty;
            noneResult = string.Empty;
        }

        var none = new Option<string>();
        var some = new Option<string>("some value");

        some.Tap(setSomeInput, setNone).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe("some value");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.Tap(setSomeInput, setNone).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");

        reset();

        some.Tap(setSome, setNone).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.Tap(setSome, setNone).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");

        reset();

        some.TapSome(setSomeInput).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe("some value");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.TapSome(setSomeInput).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);

        reset();

        some.TapSome(setSome).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe("some");
        noneResult.ShouldBe(string.Empty);

        reset();

        none.TapSome(setSome).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);

        reset();

        none.TapNone(setNone).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe("none");

        reset();

        some.TapNone(setNone).AssertInstanceOfType(typeof(Option<string>));
        someResult.ShouldBe(string.Empty);
        noneResult.ShouldBe(string.Empty);
    }


}