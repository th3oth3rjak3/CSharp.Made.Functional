namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class EffectTests
{
    private string SuccessResult { get; set; } = string.Empty;
    private string ErrorResult { get; set; } = string.Empty;
    
    [TestInitialize]
    public void Reset()
    {
        SuccessResult = string.Empty;
        ErrorResult = string.Empty;
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_1()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                Error => ErrorResult = Error.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                Error => ErrorResult = Error)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_2()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_3()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                Error => Task.Run(() => ErrorResult = Error.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                Error => Task.Run(() => ErrorResult = Error))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_4()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => SuccessResult = success.ToString(),
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_5()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_6()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                Error => ErrorResult = Error.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                Error => ErrorResult = Error)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_7()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                Error => Task.Run(() => ErrorResult = Error.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                Error => Task.Run(() => ErrorResult = Error))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_8()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => SuccessResult = "success",
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_9()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                Error => ErrorResult = Error.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                Error => ErrorResult = Error)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_10()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_11()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                Error => Task.Run(() => ErrorResult = Error.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                Error => Task.Run(() => ErrorResult = Error))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_12()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("1");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                success => Task.Run(() => SuccessResult = success.ToString()),
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_13()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                Error => ErrorResult = Error.Message)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                Error => ErrorResult = Error)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_14()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_15()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                Error => Task.Run(() => ErrorResult = Error.Message))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                Error => Task.Run(() => ErrorResult = Error))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffectsAsync_16()
    {
        await Ok(1)
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBe("success");
        ErrorResult.ShouldBeEmpty();

        Reset();

        await Error<int, string>("Error!")
            .Async()
            .EffectAsync(
                () => Task.Run(() => SuccessResult = "success"),
                () => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBeEmpty();
        ErrorResult.ShouldBe("fail");
    }

    [TestMethod]
    public async Task ResultShouldPerformEffecTOk_1()
    {
        await Error<int, string>("Error")
            .Async()
            .EffectOkAsync(success => SuccessResult = success.ToString())
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Ok(1)
            .Async()
            .EffectOkAsync(success => SuccessResult = success.ToString())
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("1");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffecTOk_2()
    {
        await Error<int, string>("Error")
            .Async()
            .EffectOkAsync(() => SuccessResult = "success")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Ok(1)
            .Async()
            .EffectOkAsync(() => SuccessResult = "success")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("success");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffecTOk_3()
    {
        await Error<int, string>("Error")
            .Async()
            .EffectOkAsync(success => Task.Run(() => SuccessResult = success.ToString()))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Ok(1)
            .Async()
            .EffectOkAsync(success => Task.Run(() => SuccessResult = success.ToString()))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("1");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffecTOk_4()
    {
        await Error<int, string>("Error")
            .Async()
            .EffectOkAsync(() => Task.Run(() => SuccessResult = "success"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));

        SuccessResult.ShouldBeEmpty();

        await Ok(1)
            .Async()
            .EffectOkAsync(() => Task.Run(() => SuccessResult = "success"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        SuccessResult.ShouldBe("success");
    }

    [TestMethod]
    public async Task ResultShouldPerformEffecTError_1()
    {
        await Ok<int, string>(1)
            .Async()
            .EffectErrorAsync(Error => ErrorResult = Error)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBeEmpty();
        
        await Error<int, string>("Error!")
            .Async()
            .EffectErrorAsync(Error => ErrorResult = Error)
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffecTError_2()
    {
        await Ok<int, string>(1)
            .Async()
            .EffectErrorAsync(() => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBeEmpty();
        
        await Error<int, string>("Error!")
            .Async()
            .EffectErrorAsync(() => ErrorResult = "fail")
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBe("fail");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffecTError_3()
    {
        await Ok<int, string>(1)
            .Async()
            .EffectErrorAsync(Error => Task.Run(() => ErrorResult = Error))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBeEmpty();
        
        await Error<int, string>("Error!")
            .Async()
            .EffectErrorAsync(Error => Task.Run(() => ErrorResult = Error))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBe("Error!");
    }
    
    [TestMethod]
    public async Task ResultShouldPerformEffecTError_4()
    {
        await Ok<int, string>(1)
            .Async()
            .EffectErrorAsync(() => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBeEmpty();
        
        await Error<int, string>("Error!")
            .Async()
            .EffectErrorAsync(() => Task.Run(() => ErrorResult = "fail"))
            .EffectAsync(output => output.AssertInstanceOfType(typeof(Unit)));
        
        ErrorResult.ShouldBe("fail");
    }
}