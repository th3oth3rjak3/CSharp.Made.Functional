namespace Functional.Test.PreludeTests.Option;
[TestClass]
public class UnwrapTests
{
    [TestMethod]
    public async Task OptionShouldUnwrapAsync()
    {
        var some = Some("123").Async();
        var none = None<string>().Async();

        var inner = await some.Unwrap();
        inner.ShouldBe("123");

        await Assert.ThrowsExceptionAsync<InvalidOperationException>(() => none.Unwrap());
    }

    [TestMethod]
    public async Task OptionShouldHaveIsSomeAsync()
    {
        var some = Some("123").Async();
        var none = None<string>().Async();

        var isSome = await some.IsSome();
        var isNone = await some.IsNone();
        isSome.ShouldBeTrue();
        isNone.ShouldBeFalse();

        isSome = await none.IsSome();
        isNone = await none.IsNone();

        isSome.ShouldBeFalse();
        isNone.ShouldBeTrue();
    }
}