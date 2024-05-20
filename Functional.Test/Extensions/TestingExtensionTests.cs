namespace Functional.Test.Extensions;

[ExcludeFromCodeCoverage]
[TestClass]
public class TestingExtensionTests
{
    [TestMethod]
    public void AssertInstanceOfShouldWorkWhenMatchingTypes()
    {
        "string value".AssertInstanceOfType(typeof(string));
        new List<string>().AssertInstanceOfType(typeof(IEnumerable<string>));
        new List<string>().AssertInstanceOfType(typeof(List<string>));
        42.AssertInstanceOfType(typeof(int));
    }

    [TestMethod]
    public void AssertThrowsWhenNotInstanceOfType() =>
        Assert.ThrowsException<AssertFailedException>(() => "string value".AssertInstanceOfType(typeof(int)));
}
