namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class EffectTests
{
    private string SuccessResult { get; set; } = string.Empty;
    private string FailureResult { get; set; } = string.Empty;
    
    [TestInitialize]
    public void Reset()
    {
        SuccessResult = string.Empty;
        FailureResult = string.Empty;
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_1()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                failure => FailureResult = failure.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                failure => FailureResult = failure)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_2()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_3()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                failure => Task.Run(() => FailureResult = failure.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                failure => Task.Run(() => FailureResult = failure))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_4()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_5()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_6()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                failure => FailureResult = failure.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                failure => FailureResult = failure)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_7()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                failure => Task.Run(() => FailureResult = failure.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                failure => Task.Run(() => FailureResult = failure))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_8()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_9()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                failure => FailureResult = failure.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                failure => FailureResult = failure)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_10()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_11()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                failure => Task.Run(() => FailureResult = failure.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                failure => Task.Run(() => FailureResult = failure))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_12()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_13()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                failure => FailureResult = failure.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                failure => FailureResult = failure)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_14()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_15()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                failure => Task.Run(() => FailureResult = failure.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                failure => Task.Run(() => FailureResult = failure))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_16()
    {
        await Success(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        FailureResult.ShouldBeEmpty();

        Reset();

        await Failure<int, string>("failure!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        FailureResult.ShouldBe("fail");
    }

    [TestMethod]
    public async Task ResultShouldPerformEffectSuccess_1()
    {
        await Failure<int, string>("failure")
            .Async()
            .EffectSuccessAsync(success => SuccessResult = success.ToString())
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Success(1)
            .Async()
            .EffectSuccessAsync(success => SuccessResult = success.ToString())
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("1");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectSuccess_2()
    {
        await Failure<int, string>("failure")
            .Async()
            .EffectSuccessAsync(() => SuccessResult = "success")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Success(1)
            .Async()
            .EffectSuccessAsync(() => SuccessResult = "success")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("success");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectSuccess_3()
    {
        await Failure<int, string>("failure")
            .Async()
            .EffectSuccessAsync(success => Task.Run(() => SuccessResult = success.ToString()))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Success(1)
            .Async()
            .EffectSuccessAsync(success => Task.Run(() => SuccessResult = success.ToString()))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("1");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectSuccess_4()
    {
        await Failure<int, string>("failure")
            .Async()
            .EffectSuccessAsync(() => Task.Run(() => SuccessResult = "success"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Success(1)
            .Async()
            .EffectSuccessAsync(() => Task.Run(() => SuccessResult = "success"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("success");
    }

    [TestMethod]
    public async Task ResultShouldPerformEffectFailure_1()
    {
        await Success<int, string>(1)
            .Async()
            .EffectFailureAsync(failure => FailureResult = failure)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBeEmpty();
        
        await Failure<int, string>("failure!")
            .Async()
            .EffectFailureAsync(failure => FailureResult = failure)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectFailure_2()
    {
        await Success<int, string>(1)
            .Async()
            .EffectFailureAsync(() => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBeEmpty();
        
        await Failure<int, string>("failure!")
            .Async()
            .EffectFailureAsync(() => FailureResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectFailure_3()
    {
        await Success<int, string>(1)
            .Async()
            .EffectFailureAsync(failure => Task.Run(() => FailureResult = failure))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBeEmpty();
        
        await Failure<int, string>("failure!")
            .Async()
            .EffectFailureAsync(failure => Task.Run(() => FailureResult = failure))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBe("failure!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectFailure_4()
    {
        await Success<int, string>(1)
            .Async()
            .EffectFailureAsync(() => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBeEmpty();
        
        await Failure<int, string>("failure!")
            .Async()
            .EffectFailureAsync(() => Task.Run(() => FailureResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        FailureResult.ShouldBe("fail");
    }
}