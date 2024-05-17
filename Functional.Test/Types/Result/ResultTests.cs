using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

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
            .Async()
            .BindAsync(res => res.Ok<string, string>())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    _ => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindAsyncWithFailure() =>
    await "failure message"
        .Error<string, string>()
        .Async()
        .BindAsync(res => res.Ok<string, string>())
        .TapAsync(res =>
            res.Match(
                _ => throw new ShouldAssertException("Shouldn't have passed"),
                failure => res.Tap(_ => failure.ShouldBe("failure message"))));

    [TestMethod]
    public async Task ItShouldBind() =>
        await "yay"
            .Ok<string, string>()
            .Async()
            .BindAsync(res => res.Ok<string, string>().Async())
            .TapAsync(res =>
                res.Match(
                    success => success.Tap(s => s.ShouldBe("yay")),
                    _ => throw new ShouldAssertException("Shouldn't have failed")));

    [TestMethod]
    public async Task ItShouldBindFailures() =>
        await "failure message"
            .Error<string, string>()
            .Async()
            .BindAsync(res => res.Ok<string, string>().Async())
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
            .Async()
            .MapAsync(one => one.ToString())
            .TapAsync(res => res.ShouldBeEquivalentTo("1".Ok<string, string>()));

    [TestMethod]
    public async Task ItShouldNotMapFailureResultsAsync() =>
        await "error".Error<int, string>()
            .Async()
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
                .Async()
                .MapAsync(intValue => intValue.ToString().Async());

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
                .Async()
                .MapAsync(intValue => intValue.ToString().Async());

        var awaited = await result;
        awaited.ShouldBeOfType<Result<string, int>>();

        awaited.IsError.ShouldBeTrue();
        awaited.UnwrapError().ShouldBe(404);
    }

    [TestMethod]
    public async Task ItShouldBindSuccessAsyncResults() =>
        await 1.Ok<int, string>()
            .Async()
            .BindAsync(one => one.ToString().Ok<string, string>().Async())
            .ReduceAsync("error")
            .TapAsync(str => str.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldBindFailureAsyncResults() =>
    await "error".Error<int, string>()
        .Async()
        .BindAsync(one => one.ToString().Ok<string, string>().Async())
        .ReduceAsync("error")
        .TapAsync(str => str.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsync() =>
        await 1
            .Ok<int, string>()
            .Async()
            .MatchAsync(
                success => success.ToString(),
                 failure => failure)
            .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsync() =>
        await "error"
            .Error<int, string>()
            .Async()
            .MatchAsync(
                success => success.ToString(),
                failure => failure)
            .TapAsync(res => res.ShouldBe("error"));

    [TestMethod]
    public async Task ItShouldMatchSuccessesAsyncWithAsyncMapping() =>
    await 1
        .Ok<int, string>()
        .Async()
        .MatchAsync(
            success => success.ToString().Async(),
             failure => failure.Async())
        .TapAsync(res => res.ShouldBe("1"));

    [TestMethod]
    public async Task ItShouldMatchFailuresAsyncWithAsyncMapping() =>
        await "error"
            .Error<int, string>()
            .Async()
            .MatchAsync(
                success => success.ToString().Async(),
                failure => failure.Async())
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

        void SuccessAction(string _) => successEffect = true;
        void FailureAction(string _) => failureEffect = true;

        "success".Ok<string, string>()
            .Effect(SuccessAction, FailureAction);

        successEffect.ShouldBeTrue();
        failureEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformFailureEffect()
    {
        var successEffect = false;
        var failureEffect = false;
        void FailureAction(string _) => failureEffect = true;
        void SuccessAction(string _) => successEffect = true;

        "failure".Error<string, string>()
            .Effect(SuccessAction, FailureAction);

        successEffect.ShouldBeFalse();
        failureEffect.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldDoEffectsAsyncWhenOk()
    {
        var msg = "";

        await "123"
            .Pipe(Result.Ok<string, Exception>)
            .Async()
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
            .Async()
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
                .Async()
                .MatchAsync(
                    ok => ok,
                    err => err.ToString().Async());

        (await result).ShouldBe("ok");
    }

    [TestMethod]
    public async Task ItShouldMatchAsyncWhenOkSyncAndErrorAsyncWhenError()
    {
        var result =
            1
                .Error<string, int>()
                .Async()
                .MatchAsync(
                    ok => ok,
                    err => err.ToString().Async());

        (await result).ShouldBe("1");
    }

    [TestMethod]
    public async Task ItShouldMatchAsyncWhenOkAsyncAndErrorSyncWhenOk()
    {
        var result =
            "ok"
                .Ok<string, int>()
                .Async()
                .MatchAsync(
                    ok => ok.Async(),
                    err => err.ToString());

        (await result).ShouldBe("ok");
    }

    [TestMethod]
    public async Task ItShouldMatchAsyncWhenOkAsyncAndErrorSyncWhenError()
    {
        var result =
            1
                .Error<string, int>()
                .Async()
                .MatchAsync(
                    ok => ok.Async(),
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
                .Async()
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
                .Async()
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
                .Async()
                .MapErrorAsync(errInt => errInt.ToString().Async());

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
                .Async()
                .MapErrorAsync(errCode => errCode.ToString().Async());

        var contents = await result;

        contents.ShouldBeOfType<Result<string, string>>();
        contents.IsError.ShouldBeTrue();
        contents.UnwrapError().ShouldBe("404");
    }

    [TestMethod]
    public void ItShouldMapErrorsThatAreExceptions() =>
        new Exception("Something bad happened")
            .Pipe(Result.Error<string>)
            .Tap(result => result.ShouldBeOfType<Result<string, Exception>>());

    [TestMethod]
    public void ItShouldMapOkResultsThatAreHaveExceptionErrorTypes() =>
        "valid"
            .Pipe(Result.Ok)
            .Tap(result => result.ShouldBeOfType<Result<string, Exception>>());

    [TestMethod]
    public void ItShouldHandleEffectForOkOnly()
    {
        var okResult = false;
        var errorResult = false;

        Result.Error<string>(new Exception("It's a problem"))
            .EffectOk(ok => okResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeFalse();

        okResult = false;
        errorResult = false;

        Result.Ok("ok")
            .EffectOk(ok => okResult = true);

        okResult.ShouldBeTrue();
        errorResult.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldHandleEffectForErrorOnly()
    {
        var okResult = false;
        var errorResult = false;

        Result.Error<string>(new Exception("It's a problem"))
            .EffectError(error => errorResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeTrue();

        okResult = false;
        errorResult = false;

        Result.Ok("ok")
            .EffectError(error => errorResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithOkPlainAction()
    {
        var okResult = false;
        var errorResult = false;

        await Result.Ok<string, string>("It's ok")
            .Async()
            .EffectAsync(() => okResult = true, error => throw new ShouldAssertException("It shouldn't have executed this branch."));

        okResult.ShouldBeTrue();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithErrorPlainAction()
    {
        var okResult = false;
        var errorResult = false;

        await Result.Error<string>(new Exception("It's an error!"))
            .Async()
            .EffectAsync(ok => throw new ShouldAssertException("It shouldn't have been ok."), () => errorResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectAsyncWithPlainActions()
    {
        var okResult = false;
        var errorResult = false;

        await Result.Error<string>(new Exception("It's an error!"))
            .Async()
            .EffectAsync(() => okResult = true, () => errorResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeTrue();

        okResult = false;
        errorResult = false;

        await Result.Ok<string, Exception>("It's ok")
            .Async()
            .EffectAsync(() => okResult = true, () => errorResult = true);

        okResult.ShouldBeTrue();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectOkAsyncWhenOk()
    {
        var okResult = false;
        var okContents = string.Empty;
        var errorResult = false;

        await Result.Ok<string, Exception>("it's okay")
            .Async()
            .EffectOkAsync(ok =>
            {
                okContents = ok;
                okResult = true;
            });

        okResult.ShouldBeTrue();
        okContents.ShouldBe("it's okay");
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectOkAsyncWhenOkNoInput()
    {
        var okResult = false;
        var errorResult = false;

        await Result.Ok<string, Exception>("it's okay")
            .Async()
            .EffectOkAsync(() => okResult = true);

        okResult.ShouldBeTrue();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectOkAsyncWhenError()
    {
        var okResult = false;
        var okContents = string.Empty;
        var errorResult = false;

        await Result.Error<string>(new Exception("It's an error"))
            .Async()
            .EffectOkAsync(ok =>
            {
                okContents = ok;
                okResult = true;
            });

        okResult.ShouldBeFalse();
        okContents.ShouldBe(string.Empty);
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectOkAsyncWithFuncTask()
    {
        var okResult = false;

        Task doWork() => Effect(() => okResult = true).Pipe(Task.CompletedTask);

        await Result.Ok(true)
            .Async()
            .EffectOkAsync(() => doWork())
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        okResult.ShouldBeTrue();

        okResult = false;

        await Result.Exception<bool>("error")
            .Async()
            .EffectOkAsync(() => doWork())
            .TapAsync(output => output.ShouldBeOfType<Unit>())
            .IgnoreAsync();

        okResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectOkAsyncWhenErrorNoInput()
    {
        var okResult = false;
        var errorResult = false;

        await Result.Error<string>(new Exception("It's an error"))
            .Async()
            .EffectOkAsync(() => okResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectErrorAsyncWhenError()
    {
        var okResult = false;
        var errorContents = string.Empty;
        var errorResult = false;

        await Result.Error<string>(new Exception("It's an error"))
            .Async()
            .EffectErrorAsync(err =>
            {
                errorContents = err.Message;
                errorResult = true;
            });

        okResult.ShouldBeFalse();
        errorContents.ShouldBe("It's an error");
        errorResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectErrorAsyncWhenErrorNoInput()
    {
        var okResult = false;
        var errorResult = false;

        await Result.Error<string>(new Exception("It's an error"))
            .Async()
            .EffectErrorAsync(() => errorResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeTrue();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectErrorAsyncWhenOk()
    {
        var okResult = false;
        var errorContents = string.Empty;
        var errorResult = false;

        await Result.Ok<string, Exception>("it's okay")
            .Async()
            .EffectErrorAsync(err =>
            {
                errorContents = err.Message;
                errorResult = true;
            });

        okResult.ShouldBeFalse();
        errorContents.ShouldBe(string.Empty);
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectErrorAsyncWhenOkNoInput()
    {
        var okResult = false;
        var errorResult = false;

        await Result.Ok<string, Exception>("it's okay")
            .Async()
            .EffectErrorAsync(() => errorResult = true);

        okResult.ShouldBeFalse();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public async Task ItShouldHandleEffectErrorAsyncWhenFuncTask()
    {
        var errorResult = false;

        Task doWork() => Effect(() => errorResult = true).Pipe(Task.CompletedTask);

        await Result.Exception<bool>("error")
            .Async()
            .EffectErrorAsync(() => doWork())
            .TapAsync(output => output.ShouldBe(Unit.Default))
            .IgnoreAsync();

        errorResult.ShouldBeTrue();

        errorResult = false;

        await Result.Ok(true)
            .Async()
            .EffectErrorAsync(() => doWork())
            .TapAsync(output => output.ShouldBe(Unit.Default))
            .IgnoreAsync();

        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldHandleEffectWithPlainOkAction()
    {
        var okResult = false;
        var errorResult = false;

        Result.Ok("value")
            .Effect(() => okResult = true, error => errorResult = true)
            .ShouldBeOfType<Unit>();

        okResult.ShouldBeTrue();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldHandleErrorEffectWithPlainOkAction()
    {
        var okResult = false;
        var errorResult = false;

        Result.Exception<string>("value")
            .Effect(() => okResult = true, error => errorResult = true)
            .ShouldBeOfType<Unit>();

        okResult.ShouldBeFalse();
        errorResult.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldHandleEffectWithPlainErrorAction()
    {
        var okResult = false;
        var errorResult = false;

        Result.Error<string>(new Exception("error"))
            .Effect(ok => okResult = true, () => errorResult = true)
            .ShouldBeOfType<Unit>();

        okResult.ShouldBeFalse();
        errorResult.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldHandleOkEffectWithPlainErrorAction()
    {
        var okResult = false;
        var errorResult = false;

        Result.Ok("value")
            .Effect(ok => okResult = true, () => errorResult = true)
            .ShouldBeOfType<Unit>();

        okResult.ShouldBeTrue();
        errorResult.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldHandleEffectWithPlainActions()
    {
        var okResult = false;
        var errorResult = false;

        Result.Ok("value")
            .Effect(() => okResult = true, () => errorResult = true)
            .ShouldBeOfType<Unit>();

        okResult.ShouldBeTrue();
        errorResult.ShouldBeFalse();

        okResult = false;
        errorResult = false;

        Result.Error<string>(new Exception("error"))
            .Effect(() => okResult = true, () => errorResult = true)
            .ShouldBeOfType<Unit>();

        okResult.ShouldBeFalse();
        errorResult.ShouldBeTrue();
    }

    [TestMethod]
    public void ItShouldCreateExceptionsWithMessages() =>
        Result.Exception<int>("an error occurred")
            .UnwrapError()
            .ShouldBeOfType<Exception>();

    [TestMethod]
    public void ItShouldTapOffOfResult()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Result.Ok("value")
            .Tap(ok => okResult = ok, err => errorResult = err.Message)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe("value");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Exception<string>("error")
            .Tap(ok => okResult = ok, exn => errorResult = exn.Message)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public void ItShouldTapWithVariousActions()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Result.Ok("value")
            .Tap(() => okResult = "new", err => errorResult = err.Message)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe("new");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Exception<string>("error")
            .Tap(ok => okResult = ok, () => errorResult = "new error")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("new error");

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Ok("value")
            .Tap(ok => okResult = ok, () => errorResult = "new error")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe("value");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Exception<string>("error")
            .Tap(() => okResult = "new", exn => errorResult = exn.Message)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Ok("value")
            .Tap(() => okResult = "new", () => errorResult = "new error")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe("new");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Exception<string>("error")
            .Tap(() => okResult = "new", () => errorResult = "new error")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("new error");
    }

    [TestMethod]
    public void ItShouldTapOk()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Result.Ok("something")
            .TapOk(value => okResult = value)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe("something");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Ok("something")
            .TapOk(() => okResult = "something new")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe("something new");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Exception<string>("something")
            .TapOk(value => okResult = value)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Exception<string>("something")
            .TapOk(() => okResult = "something new")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe(string.Empty);
    }


    [TestMethod]
    public void ItShouldTapError()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Result.Exception<string>("something")
            .TapError(value => errorResult = value.Message)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("something");

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Exception<string>("something")
            .TapError(() => errorResult = "something new")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("something new");

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Ok("something")
            .TapError(value => errorResult = value.Message)
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        Result.Ok("something")
            .TapError(() => errorResult = "something new")
            .ShouldBeOfType<Result<string, Exception>>();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithTwoActionsWithInput()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        await Result.Ok("ok")
            .Async()
            .TapAsync(ok => okResult = ok, exn => errorResult = exn.Message)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(ok => okResult = ok, exn => errorResult = exn.Message)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkActionTErrorAction()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        await Result.Ok("ok")
            .Async()
            .TapAsync(ok => okResult = ok, () => errorResult = "error")
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(ok => okResult = ok, () => errorResult = "error")
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkActionTErrorFuncTask()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doWork() => Effect(() => errorResult = "error").Pipe(Task.CompletedTask);

        await Result.Ok("ok")
            .Async()
            .TapAsync(ok => okResult = ok, doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(ok => okResult = ok, doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkActionTErrorFuncErrorTask()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doWork(Exception input) => Effect(() => errorResult = input.Message).Pipe(Task.CompletedTask);

        await Result.Ok("ok")
            .Async()
            .TapAsync(ok => okResult = ok, doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(ok => okResult = ok, doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkActionErrorActionT()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        await Result.Ok("value")
            .Async()
            .TapAsync(() => okResult = "ok", exn => errorResult = exn.Message)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(() => okResult = "ok", exn => errorResult = exn.Message)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkActionErrorAction()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        await Result.Ok("value")
            .Async()
            .TapAsync(() => okResult = "ok", () => errorResult = "error")
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(() => okResult = "ok", () => errorResult = "error")
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkActionErrorFuncTask()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doWork() => Effect(() => errorResult = "error").Pipe(Task.CompletedTask);

        await Result.Ok("value")
            .Async()
            .TapAsync(() => okResult = "ok", doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(() => okResult = "ok", doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkActionErrorFuncTTask()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doWork(Exception exn) => Effect(() => errorResult = exn.Message).Pipe(Task.CompletedTask);

        await Result.Ok("value")
            .Async()
            .TapAsync(() => okResult = "ok", doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(() => okResult = "ok", doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTaskErrorActionT()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doWork() => Effect(() => okResult = "ok").Pipe(Task.CompletedTask);

        await Result.Ok("value")
            .Async()
            .TapAsync(doWork, exn => errorResult = exn.Message)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(doWork, exn => errorResult = exn.Message)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTaskErrorAction()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doWork() => Effect(() => okResult = "ok").Pipe(Task.CompletedTask);

        await Result.Ok("value")
            .Async()
            .TapAsync(doWork, () => errorResult = "error")
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(doWork, () => errorResult = "error")
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTaskErrorFuncTask()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doOkWork() => Effect(() => okResult = "ok").Pipe(Task.CompletedTask);
        Task doErrorWork() => Effect(() => errorResult = "error").Pipe(Task.CompletedTask);

        await Result.Ok("value")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTaskErrorFuncTTask()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doOkWork() => Effect(() => okResult = "ok").Pipe(Task.CompletedTask);
        Task doErrorWork(Exception input) => Effect(() => errorResult = input.Message).Pipe(Task.CompletedTask);

        await Result.Ok("value")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTTaskErrorActionT()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doOkWork(string input) => Effect(() => okResult = input).Pipe(Task.CompletedTask);
        void doErrorWork(Exception input) => errorResult = input.Message;

        await Result.Ok("ok")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTTaskErrorAction()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doOkWork(string input) => Effect(() => okResult = input).Pipe(Task.CompletedTask);
        void doErrorWork() => errorResult = "error";

        await Result.Ok("ok")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTTaskErrorFuncTask()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doOkWork(string input) => EffectAsync(() => okResult = input);
        Task doErrorWork() => EffectAsync(() => errorResult = "error");

        await Result.Ok("ok")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("original error")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapAsyncWithOkFuncTTaskErrorFuncTaskT()
    {
        var okResult = string.Empty;
        var errorResult = string.Empty;

        Task doOkWork(string input) => EffectAsync(() => okResult = input);
        Task doErrorWork(Exception input) => EffectAsync(() => errorResult = input.Message);

        await Result.Ok("ok")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");
        errorResult.ShouldBe(string.Empty);

        okResult = string.Empty;
        errorResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapAsync(doOkWork, doErrorWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapOkAsyncWithActionT()
    {
        var okResult = string.Empty;

        void doOkWork(string input) => okResult = input;

        await Result.Ok("ok")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");

        okResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapOkAsyncWithAction()
    {
        var okResult = string.Empty;

        void doOkWork() => okResult = "ok";

        await Result.Ok("value")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");

        okResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapOkAsyncWithFuncTask()
    {
        var okResult = string.Empty;

        Task doOkWork() => EffectAsync(() => okResult = "ok");

        await Result.Ok("value")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");

        okResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapOkAsyncWithFuncTTask()
    {
        var okResult = string.Empty;

        Task doOkWork(string input) => EffectAsync(() => okResult = input);

        await Result.Ok("ok")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe("ok");

        okResult = string.Empty;

        await Result.Exception<string>("error")
            .Async()
            .TapOkAsync(doOkWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        okResult.ShouldBe(string.Empty);
    }

    [TestMethod]
    public async Task ItShouldTapErrorAsyncWithActionT()
    {
        var errorResult = string.Empty;

        void doWork(Exception exn) => errorResult = exn.Message;

        await Result.Ok("ok")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe(string.Empty);

        await Result.Exception<string>("error")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapErrorAsyncWithAction()
    {
        var errorResult = string.Empty;

        void doWork() => errorResult = "error";

        await Result.Ok("ok")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe(string.Empty);

        await Result.Exception<string>("original error")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapErrorAsyncWithFuncTask()
    {
        var errorResult = string.Empty;

        Task doWork() => EffectAsync(() => errorResult = "error");

        await Result.Ok("ok")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe(string.Empty);

        await Result.Exception<string>("original error")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldTapErrorAsyncWithFuncTTask()
    {
        var errorResult = string.Empty;

        Task doWork(Exception exn) => EffectAsync(() => errorResult = exn.Message);

        await Result.Ok("ok")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe(string.Empty);

        await Result.Exception<string>("error")
            .Async()
            .TapErrorAsync(doWork)
            .TapAsync(result => result.ShouldBeOfType<Result<string, Exception>>())
            .IgnoreAsync();

        errorResult.ShouldBe("error");
    }

    [TestMethod]
    public async Task ItShouldThrowWhenUnwrappingAsync()
    {
        static Task callback() => Result.Exception<bool>("error").Async().UnwrapAsync();
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(callback);
    }

    [TestMethod]
    public async Task ItShouldNotThrowWhenUnwrappingAsync() =>
        await Result.Ok(true)
            .Async()
            .UnwrapAsync()
            .EffectAsync(result => result.ShouldBeTrue());

    [TestMethod]
    public async Task ItShouldThrowWhenUnwrappingErrorAsync()
    {
        static Task callback() => Result.Ok(true).Async().UnwrapErrorAsync();
        await Assert.ThrowsExceptionAsync<InvalidOperationException>(callback);
    }

    [TestMethod]
    public async Task ItShouldNotThrowExceptionWhenUnwrappingErrorAsync() =>
        await Result.Exception<bool>("error message")
            .Async()
            .UnwrapErrorAsync()
            .EffectAsync(output => output.Message.ShouldBe("error message"));

}

