namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class ConstructionTests
{
    [TestMethod]
    public void ItShouldConstructOk()
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
    public void ItShouldConstructErrors()
    {
        Error<string, int>(-1)
            .AssertInstanceOfType(typeof(Result<string, int>))
            .IsError
            .ShouldBeTrue();

        Error<string>(new Exception("Error!"))
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsError
            .ShouldBeTrue();

        Error<int>("error")
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .Effect(
                output => output.IsError.ShouldBeTrue(),
                output => output.UnwrapError().Message.ShouldBe("error"));
    }
}