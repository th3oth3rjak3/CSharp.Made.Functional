
using System.Collections.Immutable;

using Functional.Monadic;
using Functional.Results;

using static Functional.Results.ResultExtensions;

namespace Functional.Test.Results;

[TestClass]
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
                    .Failure<string, ImmutableList<string>>(strs)
                    .Tap(
                        res => res.Match(
                            success => throw new ShouldAssertException("It should fail."),
                            failure => failure.Tap(
                                f => f.Count.ShouldBe(2),
                                f => f.ElementAt(0).ShouldBe("zero"),
                                f => f.ElementAt(1).ShouldBe("one")))));

    [TestMethod]
    public void ItShouldReduceSuccesses() =>
        "success".Success<string, string>()
            .Reduce("failure")
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFuncs() =>
    "success".Success<string, string>()
        .Reduce(() => "something else")
        .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFuncsAndFailureResults() =>
        "success".Success<string, string>()
            .Reduce(failure => failure)
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceFailures() =>
        Result
            .Failure<string, string>("failure message")
            .Reduce("another message")
            .ShouldBe("another message");

    [TestMethod]
    public void ItShouldReduceFailuresWithFuncs() =>
        Result
            .Failure<string, string>("failure message")
            .Reduce(() => "something else")
            .ShouldBe("something else");

    [TestMethod]
    public void ItShouldReduceFailuresWithFuncsAndFailureResults() =>
        Result
            .Failure<string, string>("failure message")
            .Reduce(failure => failure)
            .ShouldBe("failure message");

    [TestMethod]
    public Task ItShouldReduceSuccessesAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => res.Success<string, string>())
            .ReduceAsync("alternate")
            .TapAsync(
                result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFuncsAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => res.Success<string, string>())
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFuncsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(res => res.Success<string, string>())
        .ReduceAsync(_ => "alternate")
        .TapAsync(
            Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceFailuresAsync() =>
    Task.FromResult("success")
        .PipeAsync(res => Result.Failure<string, string>("failure message"))
        .ReduceAsync("alternate")
        .TapAsync(
            result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFuncsAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => Result.Failure<string, string>("failure message"))
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFuncsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(res => Result.Failure<string, string>("failure message"))
        .ReduceAsync(failure => failure)
        .TapAsync(
            Result => Result.ShouldBe("failure message"));

    [TestMethod]
    public void ItShouldBindSuccessesToSuccesses() =>
        1.Success<int, string>()
            .Bind(value => value.ToString().Success<string, string>())
            .ShouldBeOfType(typeof(Result<string, string>));

    [TestMethod]
    public void ItShouldBindSuccessesToFailures() =>
        1.Success<int, string>()
            .Bind(_ => Result.Failure<string, string>("oh no"))
            .ShouldBeOfType(typeof(Result<string, string>));

    [TestMethod]
    public void ItShouldBindFailuresWithSuccessesToFailures() =>
        Result
            .Failure<string, string>("oh no")
            .Bind(never => never.Success<string, string>())
            .Match(
                success => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.ShouldBe("oh no")));

    [TestMethod]
    public void ItShouldBindFailuresWithFailuresToFailures() =>
        Result
            .Failure<string, string>("oh no")
            .Bind(_ => Result.Failure<int, string>("Some othe failure."))
            .Match(
                success => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.ShouldBe("oh no")));

    [TestMethod]
    public async Task ItShouldBindAsync() =>
        await Result
            .Success<string, string>("yay")
            .AsAsync()
            .BindAsync(res => res.Success<string, string>())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    failure => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindAsyncWithFailure() =>
    await Result
        .Failure<string, string>("failure message")
        .AsAsync()
        .BindAsync(res => res.Success<string, string>())
        .TapAsync(res =>
            res.Match(
                success => throw new ShouldAssertException("Shouldn't have passed"),
                failure => res.Tap(_ => failure.ShouldBe("failure message"))));

    [TestMethod]
    public async Task ItShouldBind() =>
        await Result
            .Success<string, string>("yay")
            .AsAsync()
            .BindAsync(res => res.Success<string, string>().AsAsync())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    failure => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindFailures() =>
        await Result
            .Failure<string, string>("failure message")
            .AsAsync()
            .BindAsync(res => res.Success<string, string>().AsAsync())
            .TapAsync(res =>
                res.Match(
                    success => throw new ShouldAssertException("Shouldn't have passed"),
                    failure => res.Tap(_ => failure.ShouldBe("failure message"))));

    [TestMethod]
    public void ItShouldMapSuccessResults() =>
        Result.Success<int, string>(1)
            .Map(one => one.ToString())
            .ShouldBeEquivalentTo(Result.Success<string, string>("1"));

    [TestMethod]
    public void ItShouldNotMapFailureResults() =>
        Result.Failure<int, string>("error")
            .Map(one => one.ToString())
            .Match(
                success => throw new ShouldAssertException("It should have been a failure."),
                failure => failure.Tap(f => f.ShouldBe("error")))
            .Ignore();

    [TestMethod]
    public async Task ItShouldMapSuccessResultsAsync() =>
        await Result.Success<int, string>(1)
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res => res.ShouldBeEquivalentTo(Result.Success<string, string>("1")));

    [TestMethod]
    public async Task ItShouldNotMapFailureResultsAsync() =>
        await Result.Failure<int, string>("error")
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res =>
                res.Match(
                    success => throw new ShouldAssertException("Shouldn't have passed"),
                    failure => res.Tap(_ => failure.ShouldBe("error"))));

    [TestMethod]
    public async Task ItShouldBindSuccessAsyncResults() =>
        await Result.Success<int, string>(1)
            .AsAsync()
            .BindAsync(one => one.ToString().Success<string, string>().AsAsync())
            .ReduceAsync("error")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldBindFailureAsyncResults() =>
    await Result.Failure<int, string>("error")
        .AsAsync()
        .BindAsync(one => one.ToString().Success<string, string>().AsAsync())
        .ReduceAsync("error")
        .TapAsync(str => str.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsync() =>
        await Result
            .Success<int, string>(1)
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                 failure => failure)
            .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsync() =>
        await Result
            .Failure<int, string>("error")
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                failure => failure)
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsyncWithAsyncMapping() =>
    await Result
        .Success<int, string>(1)
        .AsAsync()
        .MatchAsync(
            success => success.ToString().AsAsync(),
             failure => failure.AsAsync())
        .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsyncWithAsyncMapping() =>
        await Result
            .Failure<int, string>("error")
            .AsAsync()
            .MatchAsync(
                success => success.ToString().AsAsync(),
                failure => failure.AsAsync())
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public void ItShouldBindAllSuccesses() =>
        new List<Result<int, string>>()
        {
            Result.Success<int, string>(1),
            Result.Success<int, string>(2),
            Result.Success<int, string>(3),
        }
            .BindAll()
            .ShouldBeEquivalentTo(Result.Success<List<int>, List<string>>(new List<int>() { 1, 2, 3 }));

    [TestMethod]
    public void ItShouldBindAllFailures() =>
        new List<Result<int, string>>()
        {
            Result.Failure<int, string>("one"),
            Result.Failure<int, string>("two"),
            Result.Failure<int, string>("three")
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                Result
                    .Failure<List<int>, List<string>>(
                        new List<string>()
                        { "one", "two", "three" }));

    [TestMethod]
    public void ItShouldBindSuccessAndFailuresToFailure() =>
        new List<Result<int, string>>()
        {
            Result.Success<int, string>(1),
            Result.Success<int, string>(2),
            Result.Failure<int, string>("three")
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                Result
                    .Failure<List<int>, List<string>>(new List<string>() { "three" }));

    [TestMethod]
    public void ItShouldPerformSuccessEffect()
    {
        var successEffect = false;
        var failureEffect = false;
        void successAction(string _) { successEffect = true; }
        void failureAction(string _) { failureEffect = true; }

        Result.Success<string, string>("success")
            .Effect(successAction, failureAction);

        successEffect.ShouldBeTrue();
        failureEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformFailureEffect()
    {
        var successEffect = false;
        var failureEffect = false;
        void successAction(string _) { successEffect = true; }
        void failureAction(string _) { failureEffect = true; }

        Result.Failure<string, string>("failure")
            .Effect(successAction, failureAction);

        successEffect.ShouldBeFalse();
        failureEffect.ShouldBeTrue();
    }
}

