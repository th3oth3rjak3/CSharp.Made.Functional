using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

using Functional.Common;
using Functional.Results;

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
            .Pipe(strings =>
                strings
                    .Error<string, ImmutableList<string>>()
                    .Tap(
                        res => res.Match(
                            _ => throw new ShouldAssertException("It should fail."),
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
        "failure message"
            .Error<string, string>()
            .Reduce("another message")
            .ShouldBe("another message");

    [TestMethod]
    public void ItShouldReduceFailuresWithFunctions() =>
        "failure message"
            .Error<string, string>()
            .Reduce(() => "something else")
            .ShouldBe("something else");

    [TestMethod]
    public void ItShouldReduceFailuresWithFunctionsAndFailureResults() =>
        "failure message"
            .Error<string, string>()
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
                result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFunctionsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(res => res.Ok<string, string>())
        .ReduceAsync(_ => "alternate")
        .TapAsync(
            result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceFailuresAsync() =>
    Task.FromResult("success")
        .PipeAsync(_ => "failure message".Error<string, string>())
        .ReduceAsync("alternate")
        .TapAsync(
            result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFunctionsAsync() =>
        Task.FromResult("success")
            .PipeAsync(_ => "failure message".Error<string, string>())
            .ReduceAsync(() => "alternate")
            .TapAsync(
                result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFunctionsAsyncFailure() =>
    Task.FromResult("success")
        .PipeAsync(_ => "failure message".Error<string, string>())
        .ReduceAsync(failure => failure)
        .TapAsync(
            result => result.ShouldBe("failure message"));

    [TestMethod]
    public void ItShouldBindSuccessesToSuccesses() =>
        1.Ok<int, string>()
            .Bind(value => value.ToString().Ok<string, string>())
            .ShouldBeOfType(typeof(Result<string, string>));

    [TestMethod]
    public void ItShouldBindSuccessesToFailures() =>
        1.Ok<int, string>()
            .Bind(_ => "oh no".Error<string, string>())
            .ShouldBeOfType(typeof(Result<string, string>));

    [TestMethod]
    public void ItShouldBindFailuresWithSuccessesToFailures() =>
        "oh no"
            .Error<string, string>()
            .Bind(never => never.Ok<string, string>())
            .Match(
                _ => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.ShouldBe("oh no")));

    [TestMethod]
    public void ItShouldBindFailuresWithFailuresToFailures() =>
        "oh no"
            .Error<string, string>()
            .Bind(_ => "Some other failure.".Error<int, string>())
            .Match(
                _ => throw new ShouldAssertException("Shouldn't succeed"),
                failure => failure.Tap(f => f.ShouldBe("oh no")));

    [TestMethod]
    public async Task ItShouldBindAsync() =>
        await "yay"
            .Ok<string, string>()
            .AsAsync()
            .BindAsync(res => res.Ok<string, string>())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    _ => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindAsyncWithFailure() =>
    await "failure message"
        .Error<string, string>()
        .AsAsync()
        .BindAsync(res => res.Ok<string, string>())
        .TapAsync(res =>
            res.Match(
                _ => throw new ShouldAssertException("Shouldn't have passed"),
                failure => res.Tap(_ => failure.ShouldBe("failure message"))));

    [TestMethod]
    public async Task ItShouldBind() =>
        await "yay"
            .Ok<string, string>()
            .AsAsync()
            .BindAsync(res => res.Ok<string, string>().AsAsync())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    _ => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindFailures() =>
        await "failure message"
            .Error<string, string>()
            .AsAsync()
            .BindAsync(res => res.Ok<string, string>().AsAsync())
            .TapAsync(res =>
                res.Match(
                    _ => throw new ShouldAssertException("Shouldn't have passed"),
                    failure => res.Tap(_ => failure.ShouldBe("failure message"))));

    [TestMethod]
    public void ItShouldMapSuccessResults() =>
        1.Ok<int, string>()
            .Map(one => one.ToString())
            .ShouldBeEquivalentTo("1".Ok<string, string>());

    [TestMethod]
    public void ItShouldNotMapFailureResults() =>
        "error".Error<int, string>()
            .Map(one => one.ToString())
            .Match(
                _ => throw new ShouldAssertException("It should have been a failure."),
                failure => failure.Tap(f => f.ShouldBe("error")))
            .Ignore();

    [TestMethod]
    public async Task ItShouldMapSuccessResultsAsync() =>
        await 1.Ok<int, string>()
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res => res.ShouldBeEquivalentTo("1".Ok<string, string>()));

    [TestMethod]
    public async Task ItShouldNotMapFailureResultsAsync() =>
        await "error".Error<int, string>()
            .AsAsync()
            .MapAsync(one => one.ToString())
            .TapAsync(res =>
                res.Match(
                    _ => throw new ShouldAssertException("Shouldn't have passed"),
                    failure => res.Tap(_ => failure.ShouldBe("error"))));

    [TestMethod]
    public async Task ItShouldMapSuccessResultsWithAsyncMapperWhenOk()
    {
        var result =
            100
                .Ok<int, int>()
                .AsAsync()
                .MapAsync(intValue => intValue.ToString().AsAsync());

        var awaited = await result;
        awaited.ShouldBeOfType<Result<string, int>>();
        
        awaited.IsOk.ShouldBeTrue();
        awaited.Unwrap().ShouldBe("100");
    }

    [TestMethod]
    public async Task ItShouldMapSuccessResultsWithAsyncMapperWhenError()
    {
        var result =
            404
                .Error<int, int>()
                .AsAsync()
                .MapAsync(intValue => intValue.ToString().AsAsync());

        var awaited = await result;
        awaited.ShouldBeOfType<Result<string, int>>();
        
        awaited.IsError.ShouldBeTrue();
        awaited.UnwrapError().ShouldBe(404);
    }
    
    [TestMethod]
    public async Task ItShouldBindSuccessAsyncResults() =>
        await 1.Ok<int, string>()
            .AsAsync()
            .BindAsync(one => one.ToString().Ok<string, string>().AsAsync())
            .ReduceAsync("error")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldBindFailureAsyncResults() =>
    await "error".Error<int, string>()
        .AsAsync()
        .BindAsync(one => one.ToString().Ok<string, string>().AsAsync())
        .ReduceAsync("error")
        .TapAsync(str => str.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsync() =>
        await 1
            .Ok<int, string>()
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                 failure => failure)
            .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsync() =>
        await "error"
            .Error<int, string>()
            .AsAsync()
            .MatchAsync(
                success => success.ToString(),
                failure => failure)
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsyncWithAsyncMapping() =>
    await 1
        .Ok<int, string>()
        .AsAsync()
        .MatchAsync(
            success => success.ToString().AsAsync(),
             failure => failure.AsAsync())
        .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsyncWithAsyncMapping() =>
        await "error"
            .Error<int, string>()
            .AsAsync()
            .MatchAsync(
                success => success.ToString().AsAsync(),
                failure => failure.AsAsync())
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public void ItShouldBindAllSuccesses() =>
        new List<Result<int, string>>()
        {
            1.Ok<int, string>(),
            2.Ok<int, string>(),
            3.Ok<int, string>(),
        }
            .BindAll()
            .ShouldBeEquivalentTo(new List<int> { 1, 2, 3 }.Ok<List<int>, List<string>>());

    [TestMethod]
    public void ItShouldBindAllFailures() =>
        new List<Result<int, string>>()
        {
            "one".Error<int, string>(),
            "two".Error<int, string>(),
            "three".Error<int, string>()
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                new List<string> { "one", "two", "three" }
                    .Error<List<int>, List<string>>());

    [TestMethod]
    public void ItShouldBindSuccessAndFailuresToFailure() =>
        new List<Result<int, string>>
            {
            1.Ok<int, string>(),
            2.Ok<int, string>(),
            "three".Error<int, string>()
        }
            .BindAll()
            .ShouldBeEquivalentTo(
                new List<string> { "three" }
                    .Error<List<int>, List<string>>());

    [TestMethod]
    public void ItShouldPerformSuccessEffect()
    {
        var successEffect = false;
        var failureEffect = false;

        "success".Ok<string, string>()
            .Effect(SuccessAction, FailureAction);

        successEffect.ShouldBeTrue();
        failureEffect.ShouldBeFalse();
        return;

        void FailureAction(string _) => failureEffect = true;

        void SuccessAction(string _) => successEffect = true;
    }

    [TestMethod]
    public void ItShouldPerformFailureEffect()
    {
        var successEffect = false;
        var failureEffect = false;

        "failure".Error<string, string>()
            .Effect(SuccessAction, FailureAction);

        successEffect.ShouldBeFalse();
        failureEffect.ShouldBeTrue();
        return;

        void FailureAction(string _) => failureEffect = true;

        void SuccessAction(string _) => successEffect = true;
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
                _ => msg = "Exception");

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

    [TestMethod]
    public async Task ItShouldMatchAsyncWhenOkSyncAndErrorAsyncWhenOk()
    {
        var result =
            "ok"
                .Ok<string, int>()
                .AsAsync()
                .MatchAsync(
                    ok => ok,
                    err => err.ToString().AsAsync());
        
        (await result).ShouldBe("ok");
    }
    
    [TestMethod]
    public async Task ItShouldMatchAsyncWhenOkSyncAndErrorAsyncWhenError()
    {
        var result =
            1
                .Error<string, int>()
                .AsAsync()
                .MatchAsync(
                    ok => ok,
                    err => err.ToString().AsAsync());
        
        (await result).ShouldBe("1");
    }
    
    [TestMethod]
    public async Task ItShouldMatchAsyncWhenOkAsyncAndErrorSyncWhenOk()
    {
        var result =
            "ok"
                .Ok<string, int>()
                .AsAsync()
                .MatchAsync(
                    ok => ok.AsAsync(),
                    err => err.ToString());
        
        (await result).ShouldBe("ok");
    }
    
    [TestMethod]
    public async Task ItShouldMatchAsyncWhenOkAsyncAndErrorSyncWhenError()
    {
        var result =
            1
                .Error<string, int>()
                .AsAsync()
                .MatchAsync(
                    ok => ok.AsAsync(),
                    err => err.ToString());
        
        (await result).ShouldBe("1");
    }

    [TestMethod]
    public void ItShouldMapErrorsWhenOk() =>
        "ok value"
            .Ok<string, int>()
            .MapError(errInt => errInt.ToString())
            .ShouldBeOfType<Result<string, string>>();

    [TestMethod]
    public void ItShouldMapErrorsWhenError()
    {
        var result = 
            404
                .Error<string, int>()
                .MapError(errCode => errCode.ToString());

        result.ShouldBeOfType<Result<string, string>>();
        
        result.UnwrapError().ShouldBe("404");
    }
    
    [TestMethod]
    public async Task ItShouldMapErrorsAsyncWhenOkWithSyncMapper()
    {
        var result =
            "ok value"
                .Ok<string, int>()
                .AsAsync()
                .MapErrorAsync(errInt => errInt.ToString());

        var contents = await result;
        
        contents.ShouldBeOfType<Result<string, string>>();
        contents.IsOk.ShouldBeTrue();
        contents.Unwrap().ShouldBe("ok value");

    }
        

    [TestMethod]
    public async Task ItShouldMapErrorsAsyncWhenErrorWithSyncMapper()
    {
        var result = 
            404
                .Error<string, int>()
                .AsAsync()
                .MapErrorAsync(errCode => errCode.ToString());

        var contents = await result;
        
        contents.ShouldBeOfType<Result<string, string>>();
        contents.IsError.ShouldBeTrue();
        contents.UnwrapError().ShouldBe("404");
    }
    
    [TestMethod]
    public async Task ItShouldMapErrorsAsyncWhenOkWithAsyncMapper()
    {
        var result =
            "ok value"
                .Ok<string, int>()
                .AsAsync()
                .MapErrorAsync(errInt => errInt.ToString().AsAsync());

        var contents = await result;
        
        contents.ShouldBeOfType<Result<string, string>>();
        contents.IsOk.ShouldBeTrue();
        contents.Unwrap().ShouldBe("ok value");

    }
        

    [TestMethod]
    public async Task ItShouldMapErrorsAsyncWhenErrorWithAsyncMapper()
    {
        var result = 
            404
                .Error<string, int>()
                .AsAsync()
                .MapErrorAsync(errCode => errCode.ToString().AsAsync());

        var contents = await result;
        
        contents.ShouldBeOfType<Result<string, string>>();
        contents.IsError.ShouldBeTrue();
        contents.UnwrapError().ShouldBe("404");
    }
}

