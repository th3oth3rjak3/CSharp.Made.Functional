﻿namespace Functional.Test.Prelude;

[TestClass]
public class MatchTests
{
    private static bool WhenTrue() => true;
    private static bool WhenFalse() => false;
    private static Task<bool> WhenTrueAsync() => Task.FromResult(true);
    private static Task<bool> WhenFalseAsync() => Task.FromResult(false);

    [TestMethod]
    public void ItShouldMatchConditionalExpressions()
    {
        (1 < 10)
            .Match(WhenTrue, WhenFalse)
            .ShouldBeTrue();

        (1 > 10)
            .Match(WhenTrue, WhenFalse)
            .ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldMatchConditionalExpressionsAsync_1()
    {
        await (1 < 10)
            .Async()
            .MatchAsync(WhenTrue, WhenFalse)
            .EffectAsync(output => output.ShouldBeTrue());

        await (1 > 10)
            .Async()
            .MatchAsync(WhenTrue, WhenFalse)
            .EffectAsync(output => output.ShouldBeFalse());
    }

    [TestMethod]
    public async Task ItShouldMatchConditionalExpressionsAsync_2()
    {
        await (1 < 10)
            .Async()
            .MatchAsync(WhenTrueAsync, WhenFalse)
            .EffectAsync(output => output.ShouldBeTrue());

        await (1 > 10)
            .Async()
            .MatchAsync(WhenTrueAsync, WhenFalse)
            .EffectAsync(output => output.ShouldBeFalse());
    }

    [TestMethod]
    public async Task ItShouldMatchConditionalExpressionsAsync_3()
    {
        await (1 < 10)
            .Async()
            .MatchAsync(WhenTrue, WhenFalseAsync)
            .EffectAsync(output => output.ShouldBeTrue());

        await (1 > 10)
            .Async()
            .MatchAsync(WhenTrue, WhenFalseAsync)
            .EffectAsync(output => output.ShouldBeFalse());
    }

    [TestMethod]
    public async Task ItShouldMatchConditionalExpressionsAsync_4()
    {
        await (1 < 10)
            .Async()
            .MatchAsync(WhenTrueAsync, WhenFalseAsync)
            .EffectAsync(output => output.ShouldBeTrue());

        await (1 > 10)
            .Async()
            .MatchAsync(WhenTrueAsync, WhenFalseAsync)
            .EffectAsync(output => output.ShouldBeFalse());
    }
}