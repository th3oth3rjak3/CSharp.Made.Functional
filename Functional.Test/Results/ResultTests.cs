
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Functional.Common;
using Functional.Options;
using Functional.Results;

using static Functional.Results.ResultExtensions;

namespace Functional.Test.Results;

[TestClass]
[ExcludeFromCodeCoverage]
public class ResultTests
{
    [TestMethod]
    public void ItShouldCreateFailureWithManyMessages() =>
        ImmutableList<string>
            .Empty
            .Add("zero")
            .Add("one")
            .Pipe(strs =>
                Result
                    .Error<string, ImmutableList<string>>(strs)
                    .Tap(
                        res => res.Match(
                            success => throw new ShouldAssertException("It should fail."),
                            failure => failure.Tap(
                                f => f.Count.ShouldBe(2),
                                f => f.ElementAt(0).ShouldBe("zero"),
                                f => f.ElementAt(1).ShouldBe("one")))));

    [TestMethod]
    public void ItShouldReduceSuccesses() =>
        "success".Ok<string, string>()
            .Reduce("failure")
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFunctions() =>
    "success".Ok<string, string>()
        .Reduce(() => "something else")
        .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFunctionsAndFailureResults() =>
        "success".Ok<string, string>()
            .Reduce(failure => failure)
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceFailures() =>
        Result
            .Error<string, string>("failure message")
            .Reduce("another message")
            .ShouldBe("another message");

    [TestMethod]
    public void ItShouldReduceFailuresWithFunctions() =>
        Result
            .Error<string, string>("failure message")
            .Reduce(() => "something else")
            .ShouldBe("something else");

    [TestMethod]
    public void ItShouldReduceFailuresWithFunctionsAndFailureResults() =>
        Result
            .Error<string, string>("failure message")
            .Reduce(failure => failure)
            .ShouldBe("failure message");

    [TestMethod]
    public Task ItShouldReduceSuccessesAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => res.Ok<string, string>())
            .ReduceAsync("alternate")
            .TapAsync(
                result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFunctionsAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => res.Ok<string, string>())
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFunctionsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(res => res.Ok<string, string>())
        .ReduceAsync(_ => "alternate")
        .TapAsync(
            Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceFailuresAsync() =>
    Task.FromResult("success")
        .PipeAsync(res => Result.Error<string, string>("failure message"))
        .ReduceAsync("alternate")
        .TapAsync(
            result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFunctionsAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => Result.Error<string, string>("failure message"))
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFunctionsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(res => Result.Error<string, string>("failure message"))
        .ReduceAsync(failure => failure)
        .TapAsync(
            Result => Result.ShouldBe("failure message"));

    [TestMethod]
    public void ItShouldBindSuccessesToSuccesses() =>
        1.Ok<int, string>()
            .Bind(value => value.ToString().Ok<string, string>())
            .ShouldBeOfType(typeof(Result<string, string>));

    [TestMethod]
    public void ItShouldBindSuccessesToFailures() =>
        1.Ok<int, string>()
            .Bind(_ => Result.Error<string, string>("oh no"))
            .ShouldBeOfType(typeof(Result<string, string>));

    [TestMethod]
    public void ItShouldBindFailuresWithSuccessesToFailures() =>
        Result
            .Error<string, string>("oh no")
            .Bind(never => never.Ok<string, string>())
            .Match(
                success => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.ShouldBe("oh no")));

    [TestMethod]
    public void ItShouldBindFailuresWithFailuresToFailures() =>
        Result
            .Error<string, string>("oh no")
            .Bind(_ => Result.Error<int, string>("Some other failure."))
            .Match(
                success => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.ShouldBe("oh no")));

    [TestMethod]
    public async Task ItShouldBindAsync() =>
        await Result
            .Ok<string, string>("yay")
            .AsAsync()
            .BindAsync(res => res.Ok<string, string>())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    failure => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindAsyncWithFailure() =>
    await Result
        .Error<string, string>("failure message")
        .AsAsync()
        .BindAsync(res => res.Ok<string, string>())
        .TapAsync(res =>
            res.Match(
                success => throw new ShouldAssertException("Shouldn't have passed"),
                failure => res.Tap(_ => failure.ShouldBe("failure message"))));

    [TestMethod]
    public async Task ItShouldBind() =>
        await Result
            .Ok<string, string>("yay")
            .AsAsync()
            .BindAsync(res => res.Ok<string, string>().AsAsync())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    failure => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindFailures() =>
        await Result
            .Error<string, string>("failure message")
            .AsAsync()
            .BindAsync(res => res.Ok<string, string>().AsAsync())
            .TapAsync(res =>
                res.Match(
                    success => throw new ShouldAssertException("Shouldn't have passed"),
                    failure => res.Tap(_ => failure.ShouldBe("failure message"))));

    [TestMethod]
    public void ItShouldMapSuccessResults() =>
        Result.Ok<int, string>(1)
            .Map(one => one.ToString())
            .ShouldBeEquivalentTo(Result.Ok<string, string>("1"));

    [TestMethod]
    public void ItShouldNotMapFailureResults() =>
        Result.Error<int, string>("error")
            .Map(one => one.ToString())
            .Match(
                success => throw new ShouldAssertException("It should have been a failure."),
                failure => failure.Tap(f => f.ShouldBe("error")))
            .Ignore();

    [TestMethod]
    public async Task ItShouldMapSuccessResultsAsync() =>
        await Result.Ok<int, string>(1)
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res => res.ShouldBeEquivalentTo(Result.Ok<string, string>("1")));

    [TestMethod]
    public async Task ItShouldNotMapFailureResultsAsync() =>
        await Result.Error<int, string>("error")
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res =>
                res.Match(
                    success => throw new ShouldAssertException("Shouldn't have passed"),
                    failure => res.Tap(_ => failure.ShouldBe("error"))));

    [TestMethod]
    public async Task ItShouldBindSuccessAsyncResults() =>
        await Result.Ok<int, string>(1)
            .AsAsync()
            .BindAsync(one => one.ToString().Ok<string, string>().AsAsync())
            .ReduceAsync("error")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldBindFailureAsyncResults() =>
    await Result.Error<int, string>("error")
        .AsAsync()
        .BindAsync(one => one.ToString().Ok<string, string>().AsAsync())
        .ReduceAsync("error")
        .TapAsync(str => str.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsync() =>
        await Result
            .Ok<int, string>(1)
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                 failure => failure)
            .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsync() =>
        await Result
            .Error<int, string>("error")
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                failure => failure)
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsyncWithAsyncMapping() =>
    await Result
        .Ok<int, string>(1)
        .AsAsync()
        .MatchAsync(
            success => success.ToString().AsAsync(),
             failure => failure.AsAsync())
        .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsyncWithAsyncMapping() =>
        await Result
            .Error<int, string>("error")
            .AsAsync()
            .MatchAsync(
                success => success.ToString().AsAsync(),
                failure => failure.AsAsync())
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public void ItShouldBindAllSuccesses() =>
        new List<Result<int, string>>()
        {
            Result.Ok<int, string>(1),
            Result.Ok<int, string>(2),
            Result.Ok<int, string>(3),
        }
            .BindAll()
            .ShouldBeEquivalentTo(Result.Ok<List<int>, List<string>>(new List<int>() { 1, 2, 3 }));

    [TestMethod]
    public void ItShouldBindAllFailures() =>
        new List<Result<int, string>>()
        {
            Result.Error<int, string>("one"),
            Result.Error<int, string>("two"),
            Result.Error<int, string>("three")
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                Result
                    .Error<List<int>, List<string>>(
                        new List<string>()
                        { "one", "two", "three" }));

    [TestMethod]
    public void ItShouldBindSuccessAndFailuresToFailure() =>
        new List<Result<int, string>>()
        {
            Result.Ok<int, string>(1),
            Result.Ok<int, string>(2),
            Result.Error<int, string>("three")
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                Result
                    .Error<List<int>, List<string>>(new List<string>() { "three" }));

    [TestMethod]
    public void ItShouldPerformSuccessEffect()
    {
        var successEffect = false;
        var failureEffect = false;
        void successAction(string _) => successEffect = true;
        void failureAction(string _) => failureEffect = true;

        Result.Ok<string, string>("success")
            .Effect(successAction, failureAction);

        successEffect.ShouldBeTrue();
        failureEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformFailureEffect()
    {
        var successEffect = false;
        var failureEffect = false;
        void successAction(string _) => successEffect = true;
        void failureAction(string _) => failureEffect = true;

        Result.Error<string, string>("failure")
            .Effect(successAction, failureAction);

        successEffect.ShouldBeFalse();
        failureEffect.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldDoEffectsAsyncWhenOk()
    {
        var msg = "";

        await "123"
            .Pipe(Result.Ok<string, Exception>)
            .AsAsync()
            .EffectAsync(
                ok => msg = ok,
                exn => msg = "Exception");

        msg.ShouldBe("123");
    }

    [TestMethod]
    public async Task ItShouldDoEffectsAsyncWhenError()
    {
        var msg = "";

        await new Exception("Error")
            .Pipe(Result.Error<string, Exception>)
            .AsAsync()
            .EffectAsync(
                ok => msg = ok,
                exn => msg = exn.Message);

        msg.ShouldBe("Error");
    }
}

