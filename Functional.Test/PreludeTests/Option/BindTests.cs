namespace Functional.Test.PreludeTests.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class BindTests
{
    [TestMethod]
    public void ItShouldBind()
    {
        static Option<string> TryGetString(int input) =>
            input > 10
            ? new(input.ToString())
            : new();

        var option = 42.Optional();
        option.Bind(TryGetString).AssertInstanceOfType(typeof(Option<string>));
        option.Bind(TryGetString).Unwrap().ShouldBe("42");

        new Option<int>().Bind(TryGetString).IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldBindAsyncWithNonTaskBinding()
    {
        static Option<string> TryGetString(int input) =>
            input > 10
            ? new(input.ToString())
            : new();

        var option = 42.Optional().Async();
        await option.BindAsync(TryGetString).AssertInstanceOfType(typeof(Task<Option<string>>));
        (await option.BindAsync(TryGetString)).Unwrap().ShouldBe("42");

        (await new Option<int>().Async().BindAsync(TryGetString)).IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldBindAsyncWithTaskBinding()
    {
        static Task<Option<string>> TryGetString(int input) =>
            input > 10
            ? new Option<string>(input.ToString()).Async()
            : new Option<string>().Async();

        var option = 42.Optional().Async();
        await option.BindAsync(TryGetString).AssertInstanceOfType(typeof(Task<Option<string>>));
        (await option.BindAsync(TryGetString)).Unwrap().ShouldBe("42");

        (await new Option<int>().Async().BindAsync(TryGetString)).IsNone.ShouldBeTrue();
    }
}
