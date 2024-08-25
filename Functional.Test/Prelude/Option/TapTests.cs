namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class TapTests
{
    private string SomeResult { get; set; } = string.Empty;
    private string NoneResult { get; set; } = string.Empty;

    private static Option<int> Some => Some(123);
    private static Option<int> None => None<int>();

    [TestInitialize]
    public void Setup()
    {
        SomeResult = string.Empty;
        NoneResult = string.Empty;
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_1()
    {
        await Some
            .Async()
            .TapAsync(
                value => SomeResult = value.ToString(),
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("123");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_1()
    {
        await None
            .Async()
            .TapAsync(
                value => SomeResult = value.ToString(),
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_2()
    {
        await Some
            .Async()
            .TapAsync(
                value => SomeResult = value.ToString(),
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("123");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_2()
    {
        await None
            .Async()
            .TapAsync(
                value => SomeResult = value.ToString(),
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_3()
    {
        await Some
            .Async()
            .TapAsync(
                () => SomeResult = "Some",
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("Some");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_3()
    {
        await None
            .Async()
            .TapAsync(
                () => SomeResult = "Some",
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_4()
    {
        await Some
            .Async()
            .TapAsync(
                () => SomeResult = "Some",
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("Some");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_4()
    {
        await None
            .Async()
            .TapAsync(
                () => SomeResult = "Some",
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_5()
    {
        await Some
            .Async()
            .TapAsync(
                value => EffectAsync(() => SomeResult = value.ToString()),
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("123");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_5()
    {
        await None
            .Async()
            .TapAsync(
                value => EffectAsync(() => SomeResult = value.ToString()),
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_6()
    {
        await Some
            .Async()
            .TapAsync(
                value => EffectAsync(() => SomeResult = value.ToString()),
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("123");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_6()
    {
        await None
            .Async()
            .TapAsync(
                value => EffectAsync(() => SomeResult = value.ToString()),
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_7()
    {
        await Some
            .Async()
            .TapAsync(
                () => EffectAsync(() => SomeResult = "Some"),
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("Some");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_7()
    {
        await None
            .Async()
            .TapAsync(
                () => EffectAsync(() => SomeResult = "Some"),
                () => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenSome_8()
    {
        await Some
            .Async()
            .TapAsync(
                () => EffectAsync(() => SomeResult = "Some"),
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("Some");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapAsyncWhenNone_8()
    {
        await None
            .Async()
            .TapAsync(
                () => EffectAsync(() => SomeResult = "Some"),
                () => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldTapSomeAsyncWhenSome_1()
    {
        await Some
            .Async()
            .TapSomeAsync(value => SomeResult = value.ToString())
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("123");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldNotTapSomeAsyncWhenNone_1()
    {
        await None
            .Async()
            .TapSomeAsync(value => SomeResult = value.ToString())
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapSomeAsyncWhenSome_2()
    {
        await Some
            .Async()
            .TapSomeAsync(() => SomeResult = "Some")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("Some");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldNotTapSomeAsyncWhenNone_2()
    {
        await None
            .Async()
            .TapSomeAsync(() => SomeResult = "Some")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapSomeAsyncWhenSome_3()
    {
        await Some
            .Async()
            .TapSomeAsync(value => EffectAsync(() => SomeResult = value.ToString()))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("123");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldNotTapSomeAsyncWhenNone_3()
    {
        await None
            .Async()
            .TapSomeAsync(value => EffectAsync(() => SomeResult = value.ToString()))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapSomeAsyncWhenSome_4()
    {
        await Some
            .Async()
            .TapSomeAsync(() => EffectAsync(() => SomeResult = "Some"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe("Some");
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldNotTapSomeAsyncWhenNone_4()
    {
        await None
            .Async()
            .TapSomeAsync(() => EffectAsync(() => SomeResult = "Some"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapNoneAsyncWhenNone_1()
    {
        await None
            .Async()
            .TapNoneAsync(() => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldNotTapNoneAsyncWhenSome_1()
    {
        await Some
            .Async()
            .TapNoneAsync(() => NoneResult = "None")
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task OptionShouldTapNoneAsyncWhenNone_2()
    {
        await None
            .Async()
            .TapNoneAsync(() => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe("None");
    }

    [TestMethod]
    public async Task OptionShouldNotTapNoneAsyncWhenSome_2()
    {
        await Some
            .Async()
            .TapNoneAsync(() => EffectAsync(() => NoneResult = "None"))
            .AssertInstanceOfType(typeof(Task<Option<int>>));

        SomeResult.ShouldBe(string.Empty);
        NoneResult.ShouldBe(string.Empty);
    }
}