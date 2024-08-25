namespace Functional.Test.Prelude.Option;

[ExcludeFromCodeCoverage]
[TestClass]
public class ConstructionTests
{
    [TestMethod]
    public void SomeShouldConstructNewOptionsThatAreSome()
    {
        Some("123")
            .AssertInstanceOfType(typeof(Option<string>))
            .IsSome
            .ShouldBeTrue();

        // Should still handle null values as None types.
        Some(null as string)
            .IsNone
            .ShouldBeTrue();
    }

    [TestMethod]
    public void NOneShouldConstructNewOptionsThatAreNone() =>
        None<string>()
            .AssertInstanceOfType(typeof(Option<string>))
            .IsNone
            .ShouldBeTrue();
}