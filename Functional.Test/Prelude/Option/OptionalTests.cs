namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class OptionalTests
{
    [TestMethod]
    public void StaticSomeAndNoneMethodsShouldCreateOptions()
    {
        Some(42).IsSome.ShouldBeTrue();
        Some("value").IsSome.ShouldBeTrue();

        None<string>().IsNone.ShouldBeTrue();
        None<int>().IsNone.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldConvertNullableToOptions()
    {
        "some value"
            .Optional()
            .IsSome
            .ShouldBeTrue();

        (null as string)
            .Optional()
            .IsNone
            .ShouldBeTrue();

        await "some value"
            .Async()
            .Optional()
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await (null as string)
            .Async()
            .Optional()
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        42.Optional()
            .IsSome
            .ShouldBeTrue();

        (null as int?)
            .Optional()
            .IsNone
            .ShouldBeTrue();

        await 42
            .Async()
            .Optional()
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await (null as int?)
            .Async()
            .Optional()
            .EffectAsync(option => option.IsNone.ShouldBeTrue());
    }
}
