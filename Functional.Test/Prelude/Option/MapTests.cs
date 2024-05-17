namespace Functional.Test.Prelude.Option;

[TestClass]
public class MapTests
{
    [TestMethod]
    public void MapShouldHandleNullableMappingFunctions()
    {
        static string infallible(int input) => input.ToString();

        var option = 42.Optional();

        option.Map(infallible).AssertInstanceOfType(typeof(Option<string>));
        option.Map(infallible).IsSome.ShouldBeTrue();
        option.Map(infallible).Unwrap().ShouldBe("42");
    }
}
