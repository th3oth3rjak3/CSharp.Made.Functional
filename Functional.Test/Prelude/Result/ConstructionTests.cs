namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class ConstructionTests
{
    [TestMethod]
    public void ItShouldConstrucTOkes()
    {
        Ok<string, int>("success")
            .AssertInstanceOfType(typeof(Result<string, int>))
            .IsOk
            .ShouldBeTrue();

        Ok("success")
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsOk
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldConstrucTErrors()
    {
        Error<string, int>(-1)
            .AssertInstanceOfType(typeof(Result<string, int>))
            .IsError
            .ShouldBeTrue();

        Error<string>(new Exception("Error!"))
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsError
            .ShouldBeTrue();
    }
}