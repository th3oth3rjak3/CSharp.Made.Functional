namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class NullableTests
{
    [TestMethod]
    public async Task AsNullableShouldConvertObjects()
    {
        "hello world!"
            .AsNullable()
            .ShouldBe("hello world!");

        var result =
            "hello world!"
                .Async()
                .AsNullable();

        await result.AssertInstanceOfType(typeof(Task<string?>));
    }
}