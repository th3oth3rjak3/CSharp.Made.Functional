﻿
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
            .FMap(strs =>
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
            .FMapAsync(res => res.Success())
            .ReduceAsync("alternate")
            .TapAsync(
                result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFuncsAsync() =>
        Task.FromResult("success")
            .FMapAsync(res => res.Success())
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFuncsAsyncFailure() =>
    Task.FromResult("success")
        .FMapAsync(res => res.Success())
        .ReduceAsync(_ => "alternate")
        .TapAsync(
            Result => Result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceFailuresAsync() =>
    Task.FromResult("success")
        .FMapAsync(res => Result.Failure<string>("failure message"))
        .ReduceAsync("alternate")
        .TapAsync(
            result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFuncsAsync() =>
        Task.FromResult("success")
            .FMapAsync(res => Result.Failure<string>("failure message"))
            .ReduceAsync(() => "alternate")
            .TapAsync(
                Result => Result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFuncsAsyncFailure() =>
    Task.FromResult("success")
        .FMapAsync(res => Result.Failure<string>("failure message"))
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
                    success => success.Tap(s => s.Contents.ShouldBe("yay")),
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
                    success => success.Tap(s => s.Contents.ShouldBe("yay")),
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

}