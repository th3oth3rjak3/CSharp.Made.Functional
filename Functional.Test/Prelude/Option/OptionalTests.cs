namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class OptionalTests
{
    [TestMethod]
    public async Task ItShouldConvertNullableToOptions()
    {
        static string? maybeString(bool provideNull = false) =>
            provideNull
            ? null
            : "some value";

        maybeString()
            .Optional()
            .IsSome
            .ShouldBeTrue();

        maybeString(true)
            .Optional()
            .IsNone
            .ShouldBeTrue();

        await maybeString()
            .Async()
            .Optional()
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await maybeString(true)
            .Async()
            .Optional()
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await maybeString()
            .Pipe(ValueTask.FromResult)
            .Optional()
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await maybeString(true)
            .Pipe(ValueTask.FromResult)
            .Optional()
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        (42 as int?)
            .Optional()
            .IsSome
            .ShouldBeTrue();

        (null as int?)
            .Optional()
            .IsNone
            .ShouldBeTrue();

        await (42 as int?)
            .Async()
            .Optional()
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await (null as int?)
            .Async()
            .Optional()
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

        await ValueTask.FromResult(42 as int?)
            .Optional()
            .EffectAsync(option => option.IsSome.ShouldBeTrue());

        await ValueTask.FromResult(null as int?)
            .Optional()
            .EffectAsync(option => option.IsNone.ShouldBeTrue());

    }
}