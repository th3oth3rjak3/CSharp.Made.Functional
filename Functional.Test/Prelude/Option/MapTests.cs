namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class MapTests
{
    [TestMethod]
    public async Task OptionShouldMapAsync()
    {
        static string mappingWithInput(int input) => input.ToString();
        static string mappingWithoutInput() => "Some";
        static Task<string> mappingWithInputAsync(int input) => mappingWithInput(input).Async();
        static Task<string> mappingWithoutInputAsync() => mappingWithoutInput().Async();

        await Some(42)
            .Async()
            .MapAsync(mappingWithInput)
            .TapAsync(
                output => output.IsSome.ShouldBeTrue(),
                output => output.Unwrap().ShouldBe("42"));

        await None<int>()
            .Async()
            .MapAsync(mappingWithInput)
            .EffectAsync(
                output => output.IsNone.ShouldBeTrue(),
                output => output.AssertInstanceOfType(typeof(Option<string>)));

        await Some(42)
            .Async()
            .MapAsync(mappingWithoutInput)
            .TapAsync(
                output => output.IsSome.ShouldBeTrue(),
                output => output.Unwrap().ShouldBe("Some"));

        await None<int>()
            .Async()
            .MapAsync(mappingWithoutInput)
            .EffectAsync(
                output => output.IsNone.ShouldBeTrue(),
                output => output.AssertInstanceOfType(typeof(Option<string>)));

        await Some(42)
            .Async()
            .MapAsync(mappingWithInputAsync)
            .TapAsync(
                output => output.IsSome.ShouldBeTrue(),
                output => output.Unwrap().ShouldBe("42"));

        await None<int>()
            .Async()
            .MapAsync(mappingWithInputAsync)
            .EffectAsync(
                output => output.IsNone.ShouldBeTrue(),
                output => output.AssertInstanceOfType(typeof(Option<string>)));

        await Some(42)
            .Async()
            .MapAsync(mappingWithoutInputAsync)
            .TapAsync(
                output => output.IsSome.ShouldBeTrue(),
                output => output.Unwrap().ShouldBe("Some"));

        await None<int>()
            .Async()
            .MapAsync(mappingWithoutInputAsync)
            .EffectAsync(
                output => output.IsNone.ShouldBeTrue(),
                output => output.AssertInstanceOfType(typeof(Option<string>)));
    }

    [TestMethod]
    public async Task CollectionsShouldMapOptions()
    {
        IEnumerable<Option<int>> collection = [Some(123), None<int>(), Some(456)];
        var mapped = collection.Map(value => value.ToString());

        mapped.ShouldBe([Some("123"), None<string>(), Some("456")]);

        mapped = collection.Map(() => "Some");

        mapped.ShouldBe([Some("Some"), None<string>(), Some("Some")]);

        mapped = await collection.Async().MapAsync(value => value.ToString());

        mapped.ShouldBe([Some("123"), None<string>(), Some("456")]);

        mapped = await collection.Async().MapAsync(() => "Some");

        mapped.ShouldBe([Some("Some"), None<string>(), Some("Some")]);
    }
}
