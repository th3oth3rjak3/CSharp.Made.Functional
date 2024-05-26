namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class BindTests
{
    [TestMethod]
    public async Task ItShouldBindAsync_1()
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
    public async Task ItShouldBindAsync_2()
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
    
    [TestMethod]
    public async Task ItShouldBindAsync_3()
    {
        static Option<string> TryGetString(int input) =>
            input > 10
                ? new Option<string>(input.ToString())
                : new Option<string>();

        var option = 42.Optional().Async();
        await option.BindAsync(() => TryGetString(30)).AssertInstanceOfType(typeof(Task<Option<string>>));
        (await option.BindAsync(() => TryGetString(30))).Unwrap().ShouldBe("30");

        (await new Option<int>().Async().BindAsync(() => TryGetString(8))).IsNone.ShouldBeTrue();
    }
    
    [TestMethod]
    public async Task ItShouldBindAsync_4()
    {
        static Task<Option<string>> TryGetString(int input) =>
            input > 10
                ? new Option<string>(input.ToString()).Async()
                : new Option<string>().Async();

        var option = 42.Optional().Async();
        await option
            .BindAsync(() => TryGetString(30))
            .AssertInstanceOfType(typeof(Task<Option<string>>));
        
        (await option.BindAsync(() => TryGetString(30))).Unwrap().ShouldBe("30");

        (await new Option<int>().Async().BindAsync(() => TryGetString(8))).IsNone.ShouldBeTrue();
    }
}