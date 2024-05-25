using System.CodeDom.Compiler;
using System.Runtime.CompilerServices;

using Functional.Types;

namespace Functional.Test.Prelude;

[ExcludeFromCodeCoverage]
[TestClass]
public class EffectTests
{
    private List<string> EffectResult { get; } = [];
    private CancellationTokenSource TokenSource { get; set; } = new();

    [TestInitialize]
    public void Reset()
    {
        EffectResult.Clear();
        TokenSource = new CancellationTokenSource();
    }

    private Task PerformEffectAsync(string input) => Task.Run(() => EffectResult.Add(input));
    private void PerformEffect(string input) => EffectResult.Add(input);

    [TestMethod]
    public void EffectShouldWorkWithInput()
    {
        "input"
            .Effect(PerformEffect, PerformEffect)
            .AssertInstanceOfType(typeof(Unit));

        EffectResult.Count(value => value == "input").ShouldBe(2);
    }

    [TestMethod]
    public void EffectShouldWorkWithoutInput()
    {
        "ignored"
            .Effect(() => PerformEffect("input1"), () => PerformEffect("input2"))
            .AssertInstanceOfType(typeof(Unit));

        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("input1");
        EffectResult.ShouldContain("input2");
    }

    [TestMethod]
    public void EffectStaticMethodShouldWorkWithoutInput()
    {
        Effect(
            () => PerformEffect("input1"), 
            () => PerformEffect("input2"));

        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("input1");
        EffectResult.ShouldContain("input2");
    }

    [TestMethod]
    public async Task EffectAsync_ActionT_Plain()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                PerformEffect, 
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");
    }
    
    [TestMethod]
    public async Task EffectAsync_ActionT_Order()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                PerformEffect,
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");
        
        Reset();

        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                PerformEffect,
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("input");
        EffectResult.ShouldContain("input!");
    }
    
    [TestMethod]
    public async Task EffectAsync_ActionT_Cancellation()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                CancellationToken.None,
                PerformEffect,
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");

        Reset();

        await TokenSource.CancelAsync();
        await CancelAction().AssertInstanceOfType(typeof(Task<Unit>));
        EffectResult.Count.ShouldBe(0);
        return;

        async Task<Unit> CancelAction() =>
            await "input".Async()
                .EffectAsync(TokenSource.Token, PerformEffect, value => PerformEffect(value + "!"));
    }
    
    [TestMethod]
    public async Task EffectAsync_ActionT_Order_Cancellation()
    {
        // Test normal sequential execution.
        var result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                PerformEffect,
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");

        Reset();
        
        // Test normal parallel execution
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                PerformEffect,
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldContain("input");
        EffectResult.ShouldContain("input!");
        EffectResult.Count.ShouldBe(2);

        Reset();
        
        // Test cancellation with sequential during first effect.
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                value =>
                {
                    PerformEffect(value);
                    TokenSource.Cancel();
                },
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(1);
        EffectResult.First().ShouldBe("input");
        
        Reset();
        
        // Test cancellation with parallel execution.
        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                PerformEffect,
                value => PerformEffect(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task EffectAsync_Action_Plain()
    {
        var result =
            await "input"
                .Async()
                .EffectAsync(
                    () => PerformEffect("value"),
                    () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");
    }
    
    [TestMethod]
    public async Task EffectAsync_Action_Order()
    {
        var result =
            await "input"
                .Async()
                .EffectAsync(
                    ProcessingOrder.Sequential,
                    () => PerformEffect("value"),
                    () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result =
            await "input"
                .Async()
                .EffectAsync(
                    ProcessingOrder.Parallel,
                    () => PerformEffect("value"),
                    () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");
    }
    
    [TestMethod]
    public async Task EffectAsync_Action_Cancellation()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();
        
        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task EffectAsync_Action_Order_Cancellation()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");

        Reset();

        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();

        Reset();

        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTTask_Plain()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTTask_Order()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");
        
        Reset();

        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldContain("input");
        EffectResult.ShouldContain("input!");
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTTask_Cancellation()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                TokenSource.Token,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");

        Reset();

        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                TokenSource.Token,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTTask_Order_Cancellation()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult[0].ShouldBe("input");
        EffectResult[1].ShouldBe("input!");

        Reset();

        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("input");
        EffectResult.ShouldContain("input!");

        Reset();

        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();

        Reset();

        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                PerformEffectAsync,
                value => PerformEffectAsync(value + "!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTask_Plain()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTask_Order()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTask_Cancellation()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                TokenSource.Token,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();
        
        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                TokenSource.Token,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task EffectAsync_FuncTask_Order_Cancellation()
    {
        var result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");

        Reset();

        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();

        Reset();

        await TokenSource.CancelAsync();
        result = await "input"
            .Async()
            .EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                () => PerformEffectAsync("value"),
                () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }

    [TestMethod]
    public async Task StaticEffectAsync_Action_Plain()
    {
        var result = await EffectAsync(
            () => PerformEffect("value"),
            () => PerformEffect("value!"));
        result.AssertInstanceOfType(typeof(Unit));

        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");
    }
    
    [TestMethod]
    public async Task StaticEffectAsync_Action_Order()
    {
        var result = await EffectAsync(
            ProcessingOrder.Sequential,
            () => PerformEffect("value"),
            () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result = await EffectAsync(
            ProcessingOrder.Parallel,
            () => PerformEffect("value"),
            () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");
    }
    
    [TestMethod]
    public async Task StaticEffectAsync_Action_Cancellation()
    {
        var result = await EffectAsync(
            TokenSource.Token,
            () => PerformEffect("value"),
            () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        await TokenSource.CancelAsync();
        result = await EffectAsync(
            TokenSource.Token,
            () => PerformEffect("value"),
            () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task StaticEffectAsync_Action_Order_Cancellation()
    {
        var result = await EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result = await EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");

        Reset();

        await TokenSource.CancelAsync();
        result = await EffectAsync(
                ProcessingOrder.Sequential,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();

        Reset();

        await TokenSource.CancelAsync();
        result = await EffectAsync(
                ProcessingOrder.Parallel,
                TokenSource.Token,
                () => PerformEffect("value"),
                () => PerformEffect("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
        
    }
    
    [TestMethod]
    public async Task StaticEffectAsync_FuncTask_Plain()
    {
        var result = await EffectAsync(
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));
        result.AssertInstanceOfType(typeof(Unit));

        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");
    }
    
    [TestMethod]
    public async Task StaticEffectAsync_FuncTask_Order()
    {
        var result = await EffectAsync(
            ProcessingOrder.Sequential,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result = await EffectAsync(
            ProcessingOrder.Parallel,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");
    }
    
    [TestMethod]
    public async Task StaticEffectAsync_FuncTask_Cancellation()
    {
        var result = await EffectAsync(
            TokenSource.Token,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        await TokenSource.CancelAsync();
        result = await EffectAsync(
            TokenSource.Token,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
    
    [TestMethod]
    public async Task StaticEffectAsync_FuncTask_Order_Cancellation()
    {
        var result = await EffectAsync(
            ProcessingOrder.Sequential,
            TokenSource.Token,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult[0].ShouldBe("value");
        EffectResult[1].ShouldBe("value!");

        Reset();

        result = await EffectAsync(
            ProcessingOrder.Parallel,
            TokenSource.Token,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.Count.ShouldBe(2);
        EffectResult.ShouldContain("value");
        EffectResult.ShouldContain("value!");

        Reset();

        await TokenSource.CancelAsync();
        result = await EffectAsync(
            ProcessingOrder.Sequential,
            TokenSource.Token,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();

        Reset();

        await TokenSource.CancelAsync();
        result = await EffectAsync(
            ProcessingOrder.Parallel,
            TokenSource.Token,
            () => PerformEffectAsync("value"),
            () => PerformEffectAsync("value!"));

        result.AssertInstanceOfType(typeof(Unit));
        EffectResult.ShouldBeEmpty();
    }
}