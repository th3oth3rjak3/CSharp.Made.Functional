namespace Functional.Test.Prelude.Result;

[ExcludeFromCodeCoverage]
[TestClass]
public class TapTests
{
    private readonly List<string> effects = [];

    private void EffectInput(string input) => effects.Add(input);
    private void Effect() => effects.Add("effect");
    private Task EffectTask() => EffectAsync(() => effects.Add("effect async"));
    private Task EffectInputTask(string input) => EffectAsync(() => effects.Add(input));
    
    [TestInitialize]
    public void Reset() => effects.Clear();

    [TestMethod]
    public async Task ResultShouldTapAsync_1()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInput,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInput,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_2()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInput,
                Effect)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInput,
                Effect)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("effect");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_3()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInput,
                EffectTask)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInput,
                EffectTask)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("effect async");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_4()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInput,
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInput,
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_5()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                Effect,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("effect");

        await Error<string>("error")
            .Async()
            .TapAsync(
                Effect,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_6()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                () => EffectInput("ok input"),
                () => EffectInput("error input"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok input");

        await Error<string>("error")
            .Async()
            .TapAsync(
                () => EffectInput("ok input"),
                () => EffectInput("error input"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error input");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_7()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                Effect,
                EffectTask)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("effect");

        await Error<string>("error")
            .Async()
            .TapAsync(
                Effect,
                EffectTask)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("effect async");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_8()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                Effect,
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("effect");

        await Error<string>("error")
            .Async()
            .TapAsync(
                Effect,
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_9()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectTask,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("effect async");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectTask,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_10()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectTask,
                Effect)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("effect async");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectTask,
                Effect)
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("effect");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_11()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                () => EffectInputTask("ok input"),
                () => EffectInputTask("error input"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok input");

        await Error<string>("error")
            .Async()
            .TapAsync(
                () => EffectInputTask("ok input"),
                () => EffectInputTask("error input"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error input");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_12()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                () => EffectInputTask("ok input"),
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok input");

        await Error<string>("error")
            .Async()
            .TapAsync(
                () => EffectInputTask("ok input"),
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_13()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInputTask,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInputTask,
                err => EffectInput(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_14()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInputTask,
                () => EffectInput("err"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInputTask,
                () => EffectInput("err"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("err");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_15()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInputTask,
                () => EffectInputTask("err"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInputTask,
                () => EffectInputTask("err"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("err");
    }

    [TestMethod]
    public async Task ResultShouldTapAsync_16()
    {
        await Ok("ok")
            .Async()
            .TapAsync(
                EffectInputTask,
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(1);
        effects.First().ShouldBe("ok");

        await Error<string>("error")
            .Async()
            .TapAsync(
                EffectInputTask,
                err => EffectInputTask(err.Message))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
    }

    [TestMethod]
    public async Task ResultShouldTapOkAsync_1()
    {
        await Error<string>("error")
            .Async()
            .TapOkAsync(
                EffectInput,
                ok => EffectInput(ok + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Ok("input")
            .Async()
            .TapOkAsync(
                EffectInput,
                ok => EffectInput(ok + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(2);
        effects.ShouldContain("input");
        effects.ShouldContain("input!");
    }

    [TestMethod]
    public async Task ResultShouldTapOkAsync_2()
    {
        await Error<string>("error")
            .Async()
            .TapOkAsync(
                () => EffectInput("input"),
                () => EffectInput("input!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Ok("input")
            .Async()
            .TapOkAsync(
                () => EffectInput("input"),
                () => EffectInput("input!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(2);
        effects.ShouldContain("input");
        effects.ShouldContain("input!");
    }

    [TestMethod]
    public async Task ResultShouldTapOkAsync_3()
    {
        await Error<string>("error")
            .Async()
            .TapOkAsync(
                () => EffectInputTask("input"),
                () => EffectInputTask("input!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Ok("input")
            .Async()
            .TapOkAsync(
                () => EffectInputTask("input"),
                () => EffectInputTask("input!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(2);
        effects.ShouldContain("input");
        effects.ShouldContain("input!");
    }

    [TestMethod]
    public async Task ResultShouldTapOkAsync_4()
    {
        await Error<string>("error")
            .Async()
            .TapOkAsync(
                EffectInputTask,
                ok => EffectInputTask(ok + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Ok("input")
            .Async()
            .TapOkAsync(
                EffectInputTask,
                ok => EffectInputTask(ok + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));

        effects.Count.ShouldBe(2);
        effects.ShouldContain("input");
        effects.ShouldContain("input!");
    }

    [TestMethod]
    public async Task ResultShouldTapErrorAsync_1()
    {
        await Ok("input")
            .Async()
            .TapErrorAsync(
                err => EffectInput(err.Message),
                err => EffectInput(err.Message + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Error<string>("error")
            .Async()
            .TapErrorAsync(
                err => EffectInput(err.Message),
                err => EffectInput(err.Message + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
        effects.ShouldContain("error!");
    }

    [TestMethod]
    public async Task ResultShouldTapErrorAsync_2()
    {
        await Ok("input")
            .Async()
            .TapErrorAsync(
                () => EffectInput("err"),
                () => EffectInput("err!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Error<string>("error")
            .Async()
            .TapErrorAsync(
                () => EffectInput("err"),
                () => EffectInput("err!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("err");
        effects.ShouldContain("err!");
    }

    [TestMethod]
    public async Task ResultShouldTapErrorAsync_3()
    {
        await Ok("input")
            .Async()
            .TapErrorAsync(
                () => EffectInputTask("err"),
                () => EffectInputTask("err!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Error<string>("error")
            .Async()
            .TapErrorAsync(
                () => EffectInputTask("err"),
                () => EffectInputTask("err!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("err");
        effects.ShouldContain("err!");
    }

    [TestMethod]
    public async Task ResultShouldTapErrorAsync_4()
    {
        await Ok("input")
            .Async()
            .TapErrorAsync(
                err => EffectInputTask(err.Message),
                err => EffectInputTask(err.Message + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.ShouldBeEmpty();
        
        await Error<string>("error")
            .Async()
            .TapErrorAsync(
                err => EffectInputTask(err.Message),
                err => EffectInputTask(err.Message + "!"))
            .AssertInstanceOfType(typeof(Task<Result<string, Exception>>));
        
        effects.Count.ShouldBe(2);
        effects.ShouldContain("error");
        effects.ShouldContain("error!");
    }
}