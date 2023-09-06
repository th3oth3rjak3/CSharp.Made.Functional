
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
                    .Failure<string>(strs)
                    .Tap(
                        res => res.Match(
                            success => throw new ShouldAssertException("It should fail."),
                            failure => failure.Tap(
                                f => f.FailureMessages.Count.ShouldBe(2),
                                f => f.FailureMessages.ElementAt(0).ShouldBe("zero"),
                                f => f.FailureMessages.ElementAt(1).ShouldBe("one")))));

    [TestMethod]
    public void ItShouldReduceSuccesses() =>
        "success".Success()
            .Reduce("failure")
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFuncs() =>
    "success".Success()
        .Reduce(() => "something else")
        .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFuncsAndFailureResults() =>
        "success".Success()
            .Reduce(failure => failure.FailureMessages.First())
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceFailures() =>
        Result
            .Failure<object>("failure message")
            .Reduce("another message")
            .ShouldBe("another message");

    [TestMethod]
    public void ItShouldReduceFailuresWithFuncs() =>
        Result
            .Failure<string>("failure message")
            .Reduce(() => "something else")
            .ShouldBe("something else");

    [TestMethod]
    public void ItShouldReduceFailuresWithFuncsAndFailureResults() =>
        Result
            .Failure<string>("failure message")
            .Reduce(failure => failure.FailureMessages.First())
            .ShouldBe("failure message");

    [TestMethod]
    public Task ItShouldReduceSuccessesAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => res.Success())
            .ReduceAsync("alternate")
            .TapAsync(
                result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFuncsAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => res.Success())
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFuncsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(res => res.Success())
        .ReduceAsync(_ => "alternate")
        .TapAsync(
            Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceFailuresAsync() =>
    Task.FromResult("success")
        .PipeAsync(res => Result.Failure<string>("failure message"))
        .ReduceAsync("alternate")
        .TapAsync(
            result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFuncsAsync() =>
        Task.FromResult("success")
            .PipeAsync(res => Result.Failure<string>("failure message"))
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFuncsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(res => Result.Failure<string>("failure message"))
        .ReduceAsync(failure => failure.FailureMessages.First())
        .TapAsync(
            Result => Result.ShouldBe("failure message"));

    [TestMethod]
    public void ItShouldBindSuccessesToSuccesses() =>
        1.Success()
            .Bind(value => value.ToString().Success())
            .ShouldBeOfType(typeof(Result<string>));

    [TestMethod]
    public void ItShouldBindSuccessesToFailures() =>
        1.Success()
            .Bind(_ => Result.Failure<string>("oh no"))
            .ShouldBeOfType(typeof(Result<string>));

    [TestMethod]
    public void ItShouldBindFailuresWithSuccessesToFailures() =>
        Result
            .Failure<string>("oh no")
            .Bind(never => never.Success())
            .Match(
                success => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.FailureMessages.First().ShouldBe("oh no")));

    [TestMethod]
    public void ItShouldBindFailuresWithFailuresToFailures() =>
        Result
            .Failure<string>("oh no")
            .Bind(_ => Result.Failure<int>("Some othe failure."))
            .Match(
                success => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.FailureMessages.First().ShouldBe("oh no")));

    [TestMethod]
    public async Task ItShouldBindAsync() =>
        await Result
            .Success("yay")
            .AsAsync()
            .BindAsync(res => res.Success())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    failure => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindAsyncWithFailure() =>
    await Result
        .Failure<string>("failure message")
        .AsAsync()
        .BindAsync(res => res.Success())
        .TapAsync(res =>
            res.Match(
                success => throw new ShouldAssertException("Should have failed."),
                failure =>
                    failure
                        .Tap(
                            f => f.FailureMessages.Count.ShouldBe(1),
                            f => f.FailureMessages.First().ShouldBe("failure message"))));

    [TestMethod]
    public async Task ItShouldBind() =>
        await Result
            .Success("yay")
            .Bind(res => res.Success().AsAsync())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    failure => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindFailures() =>
        await Result
            .Failure<string>("failure message")
            .Bind(res => res.Success().AsAsync())
            .TapAsync(res =>
                res.Match(
                    success => throw new ShouldAssertException("Should have failed."),
                    failure =>
                        failure
                            .Tap(
                                f => f.FailureMessages.Count.ShouldBe(1),
                                f => f.FailureMessages.First().ShouldBe("failure message"))));

    [TestMethod]
    public void ItShouldMapSuccessResults() =>
        Result.Success(1)
            .Map(one => one.ToString())
            .ShouldBeEquivalentTo(Result.Success("1"));

    [TestMethod]
    public void ItShouldNotMapFailureResults() =>
        Result.Failure<int>("error")
            .Map(one => one.ToString())
            .Match(
                success => throw new ShouldAssertException("It should have been a failure."),
                failure => failure.Tap(
                    fail => fail.FailureMessages.Count.ShouldBe(1),
                    fail => fail.FailureMessages.First().ShouldBe("error")))
            .Ignore();

    [TestMethod]
    public async Task ItShouldMapSuccessResultsAsync() =>
        await Result.Success(1)
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res => res.ShouldBeEquivalentTo(Result.Success("1")));

    [TestMethod]
    public async Task ItShouldNotMapFailureResultsAsync() =>
        await Result.Failure<int>("error")
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res =>
                res.Match(
                    success => throw new ShouldAssertException("It should have been a failure."),
                    failure => failure.Tap(
                        fail => fail.FailureMessages.Count.ShouldBe(1),
                        fail => fail.FailureMessages.First().ShouldBe("error"))));

    [TestMethod]
    public async Task ItShouldBindSuccessAsyncResults() =>
        await Result.Success(1)
            .AsAsync()
            .BindAsync(one => one.ToString().Success().AsAsync())
            .ReduceAsync("error")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldBindFailureAsyncResults() =>
    await Result.Failure<int>("error")
        .AsAsync()
        .BindAsync(one => one.ToString().Success().AsAsync())
        .ReduceAsync("error")
        .TapAsync(str => str.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsync() =>
        await Result
            .Success(1)
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                 failure => failure.FailureMessages.First())
            .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsync() =>
        await Result
            .Failure<int>("error")
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                failure => failure.FailureMessages.First())
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public void ItShouldBindAllSuccesses() =>
        new List<Result<int>>()
        {
            Result.Success(1),
            Result.Success(2),
            Result.Success(3),
        }
            .BindAll()
            .ShouldBeEquivalentTo(Result.Success(new List<int>() { 1, 2, 3 }));

    [TestMethod]
    public void ItShouldBindAllFailures() =>
        new List<Result<int>>()
        {
            Result.Failure<int>("one"),
            Result.Failure<int>("two"),
            Result.Failure<int>("three")
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                Result
                    .Failure<List<int>>(
                        ImmutableList<string>
                            .Empty
                            .Add("one")
                            .Add("two")
                            .Add("three")));

    [TestMethod]
    public void ItShouldBindSuccessAndFailuresToFailure() =>
        new List<Result<int>>()
        {
            Result.Success(1),
            Result.Success(2),
            Result.Failure<int>("three")
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                Result
                    .Failure<List<int>>("three"));

}

