namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class MapTests
{
    private string InputMapping(int input) => input.ToString();
    private string PlainMapping() => "mapped";

    private Task<string> InputMappingAsync(int input) => InputMapping(input).Async();
    private Task<string> PlainMappingAsync() => PlainMapping().Async();

    private Exception ErrorInputMapping(string input) => new(input);
    private Exception ErrorPlainMapping() => new("error");
    private Task<Exception> ErrorInputMappingAsync(string input) => ErrorInputMapping(input).Async();
    private Task<Exception> ErrorPlainMappingAsync() => ErrorPlainMapping().Async();
    
    [TestMethod]
    public async Task ResultShouldMapAsync_1()
    {
        var output = 
            await Ok(10)
                .Async()
                .MapAsync(InputMapping);

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe("10");

        output =
            await Error<int>("fail")
                .Async()
                .MapAsync(InputMapping);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldMapAsync_2()
    {
        var output = 
            await Ok(10)
                .Async()
                .MapAsync(PlainMapping);

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe("mapped");

        output =
            await Error<int>("fail")
                .Async()
                .MapAsync(PlainMapping);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldMapAsync_3()
    {
        var output = 
            await Ok(10)
                .Async()
                .MapAsync(InputMappingAsync);

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe("10");

        output =
            await Error<int>("fail")
                .Async()
                .MapAsync(InputMappingAsync);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldMapAsync_4()
    {
        var output = 
            await Ok(10)
                .Async()
                .MapAsync(PlainMappingAsync);

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe("mapped");

        output =
            await Error<int>("fail")
                .Async()
                .MapAsync(PlainMappingAsync);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("fail");
    }

    [TestMethod]
    public async Task ResultShouldMapErrorAsync_1()
    {
        var output =
            await Ok<int, string>(10)
                .Async()
                .MapErrorAsync(ErrorInputMapping)
                .AssertInstanceOfType(typeof(Task<Result<int, Exception>>));

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe(10);

        output =
            await Error<int, string>("fail")
                .Async()
                .MapErrorAsync(ErrorInputMapping);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldMapErrorAsync_2()
    {
        var output =
            await Ok<int, string>(10)
                .Async()
                .MapErrorAsync(ErrorPlainMapping)
                .AssertInstanceOfType(typeof(Task<Result<int, Exception>>));

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe(10);

        output =
            await Error<int, string>("fail")
                .Async()
                .MapErrorAsync(ErrorPlainMapping);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("error");
    }
    
    [TestMethod]
    public async Task ResultShouldMapErrorAsync_3()
    {
        var output =
            await Ok<int, string>(10)
                .Async()
                .MapErrorAsync(ErrorInputMappingAsync)
                .AssertInstanceOfType(typeof(Task<Result<int, Exception>>));

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe(10);

        output =
            await Error<int, string>("fail")
                .Async()
                .MapErrorAsync(ErrorInputMappingAsync);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldMapErrorAsync_4()
    {
        var output =
            await Ok<int, string>(10)
                .Async()
                .MapErrorAsync(ErrorPlainMappingAsync)
                .AssertInstanceOfType(typeof(Task<Result<int, Exception>>));

        output.IsOk.ShouldBeTrue();
        output.Unwrap().ShouldBe(10);

        output =
            await Error<int, string>("fail")
                .Async()
                .MapErrorAsync(ErrorPlainMappingAsync);

        output.IsError.ShouldBeTrue();
        output.UnwrapError().Message.ShouldBe("error");
    }
}