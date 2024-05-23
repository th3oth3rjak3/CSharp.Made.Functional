namespace Functional.Test.PreludeTests.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class ConstructionTests
{
    [TestMethod]
    public void ItShouldConstructSuccesses()
    {
        Success<string, int>("success")
            .AssertInstanceOfType(typeof(Result<string, int>))
            .IsSuccess
            .ShouldBeTrue();

        Success("success")
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsSuccess
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldConstructFailures()
    {
        Failure<string, int>(-1)
            .AssertInstanceOfType(typeof(Result<string, int>))
            .IsFailure
            .ShouldBeTrue();

        Failure<string>(new Exception("failure!"))
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsFailure
            .ShouldBeTrue();
    }
}