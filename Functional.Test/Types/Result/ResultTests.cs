namespace Functional.Test.Types.Result;

[TestClass]
[ExcludeFromCodeCoverage]
public class ResultTests
{
    [TestMethod]
    public void ResultShouldConstructCorrectly()
    {
        new Result<string, Exception>("hello world")
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsSuccess
            .ShouldBeTrue();

        new Result<string, Exception>(new Exception("failure"))
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsFailure
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ResultShouldUnwrap()
    {
        new Result<string, Exception>("hello world")
            .Unwrap()
            .ShouldBe("hello world");

        Assert.ThrowsException<InvalidOperationException>(() =>
            new Result<string, Exception>(new Exception("failure"))
                .Unwrap());

        new Result<string, Exception>(new Exception("failure"))
            .UnwrapFailure()
            .Message
            .ShouldBe("failure");

        Assert.ThrowsException<InvalidOperationException>(() =>
            new Result<string, Exception>("success")
                .UnwrapFailure());
    }

    [TestMethod]
    public void ResultShouldMatch()
    {
        Success(1)
            .Match(value => value, _ => -1)
            .ShouldBe(1);

        Failure<int>(new Exception("error"))
            .Match(value => value, _ => -1)
            .ShouldBe(-1);

        Success(1)
            .Match(() => 2, _ => -1)
            .ShouldBe(2);

        Failure<int>(new Exception("error"))
            .Match(() => 2, _ => -1)
            .ShouldBe(-1);

        Success(1)
            .Match(value => value, () => -1)
            .ShouldBe(1);

        Failure<int>(new Exception("error"))
            .Match(value => value, () => -1)
            .ShouldBe(-1);

        Success(1)
            .Match(() => 1, () => -1)
            .ShouldBe(1);

        Failure<int>(new Exception("error"))
            .Match(() => 1, () => -1)
            .ShouldBe(-1);
    }

    [TestMethod]
    public void ResultShouldMapSuccesses()
    {
        Success(1)
            .Map(value => value.ToString())
            .Unwrap()
            .ShouldBe("1");

        Success(1)
            .Map(() => "something")
            .Unwrap()
            .ShouldBe("something");

        Failure<int, string>("fail")
            .Map(value => value.ToString())
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsFailure
            .ShouldBeTrue();

        Failure<int, string>("fail")
            .Map(() => "something")
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsFailure
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ResultShouldMapFailures()
    {
        Success<int, string>(1)
            .MapFailure(err => new Exception(err))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .Unwrap()
            .ShouldBe(1);

        Success<int, string>(1)
            .MapFailure(() => new Exception("something else"))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .Unwrap()
            .ShouldBe(1);

        Failure<int, string>("failure")
            .MapFailure(err => new Exception(err))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .UnwrapFailure()
            .Message
            .ShouldBe("failure");

        Failure<int, string>("ignored")
            .MapFailure(() => new Exception("expected"))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .UnwrapFailure()
            .Message
            .ShouldBe("expected");
    }

    [TestMethod]
    public void ResultShouldBindToSuccesses()
    {
        Success<int, string>(1)
            .Bind(value => Success<string, string>(value.ToString()))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .Unwrap()
            .ShouldBe("1");

        Success<int, string>(1)
            .Bind(() => Success<string, string>("new value"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .Unwrap()
            .ShouldBe("new value");

        Failure<int, string>("failure")
            .Bind(value => Success<string, string>(value.ToString()))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsFailure
            .ShouldBeTrue();

        Failure<int, string>("failure")
            .Bind(() => Success<string, string>("new value"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsFailure
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ResultShouldBindToFailures()
    {
        Success<int, string>(1)
            .Bind(value => Failure<string, string>("failure!"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapFailure()
            .ShouldBe("failure!");

        Success<int, string>(1)
            .Bind(() => Failure<string, string>("failure!"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapFailure()
            .ShouldBe("failure!");

        Failure<int, string>("original")
            .Bind(value => Failure<string, string>("ignored"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapFailure()
            .ShouldBe("original");

        Failure<int, string>("original")
            .Bind(() => Failure<string, string>("ignored"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapFailure()
            .ShouldBe("original");
    }

    [TestMethod]
    public void ResultShouldPerformEffects_1()
    {
        var successResult = string.Empty;
        var failureResult = string.Empty;

        Success(1)
            .Effect(
                value => successResult = value.ToString(),
                exn => failureResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("1");
        failureResult.ShouldBe(string.Empty);

        Reset();

        Failure<int>(new Exception("error"))
            .Effect(
                value => successResult = value.ToString(),
                exn => failureResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        failureResult.ShouldBe("error");

        return;

        void Reset()
        {
            successResult = string.Empty;
            failureResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffects_2()
    {
        var successResult = string.Empty;
        var failureResult = string.Empty;

        Success(1)
            .Effect(
                () => successResult = "success",
                exn => failureResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("success");
        failureResult.ShouldBe(string.Empty);

        Reset();

        Failure<int>(new Exception("error"))
            .Effect(
                () => successResult = "success",
                exn => failureResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        failureResult.ShouldBe("error");

        return;

        void Reset()
        {
            successResult = string.Empty;
            failureResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffects_3()
    {
        var successResult = string.Empty;
        var failureResult = string.Empty;

        Success(1)
            .Effect(
                value => successResult = value.ToString(),
                () => failureResult = "failure")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("1");
        failureResult.ShouldBe(string.Empty);

        Reset();

        Failure<int>(new Exception("error"))
            .Effect(
                value => successResult = value.ToString(),
                () => failureResult = "failure")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        failureResult.ShouldBe("failure");

        return;

        void Reset()
        {
            successResult = string.Empty;
            failureResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffects_4()
    {
        var successResult = string.Empty;
        var failureResult = string.Empty;

        Success(1)
            .Effect(
                () => successResult = "success",
                () => failureResult = "failure")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("success");
        failureResult.ShouldBe(string.Empty);

        Reset();

        Failure<int>(new Exception("error"))
            .Effect(
                () => successResult = "success",
                () => failureResult = "failure")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        failureResult.ShouldBe("failure");


        return;

        void Reset()
        {
            successResult = string.Empty;
            failureResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffectSuccess()
    {
        string result = string.Empty;

        Failure<int, string>("error")
            .EffectSuccess(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Failure<int, string>("error")
            .EffectSuccess(() => result = "success")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Success(1)
            .EffectSuccess(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("1");

        Success(1)
            .EffectSuccess(() => result = "success")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("success");
    }

    [TestMethod]
    public void ResultShouldPerformEffectFailure()
    {
        string result = string.Empty;

        Success(1)
            .EffectFailure(value => result = value.Message)
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Success(1)
            .EffectFailure(() => result = "failure")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Failure<int, string>("fail")
            .EffectFailure(value => result = value)
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("fail");

        Failure<int, string>("fail")
            .EffectFailure(() => result = "failure")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("failure");
    }

    [TestMethod]
    public void ResultShouldTap_1()
    {
        var success = string.Empty;
        var failure = string.Empty;

        Success(1)
            .Tap(value => success = value.ToString(), err => failure = err.Message)
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsSuccess
            .ShouldBeTrue();

        success.ShouldBe("1");
        failure.ShouldBeEmpty();

        Reset();

        Failure<int, string>("error")
            .Tap(value => success = value.ToString(), err => failure = err)
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsFailure
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        failure.ShouldBe("error");

        return;

        void Reset()
        {
            success = string.Empty;
            failure = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTap_2()
    {
        var success = string.Empty;
        var failure = string.Empty;

        Success(1)
            .Tap(() => success = "success", err => failure = err.Message)
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsSuccess
            .ShouldBeTrue();

        success.ShouldBe("success");
        failure.ShouldBeEmpty();

        Reset();

        Failure<int, string>("error")
            .Tap(() => success = "success", err => failure = err)
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsFailure
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        failure.ShouldBe("error");

        return;

        void Reset()
        {
            success = string.Empty;
            failure = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTap_3()
    {
        var success = string.Empty;
        var failure = string.Empty;

        Success(1)
            .Tap(value => success = value.ToString(), () => failure = "failure")
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsSuccess
            .ShouldBeTrue();

        success.ShouldBe("1");
        failure.ShouldBeEmpty();

        Reset();

        Failure<int, string>("error")
            .Tap(value => success = value.ToString(), () => failure = "failure")
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsFailure
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        failure.ShouldBe("failure");

        return;

        void Reset()
        {
            success = string.Empty;
            failure = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTap_4()
    {
        var success = string.Empty;
        var failure = string.Empty;

        Success(1)
            .Tap(() => success = "success", () => failure = "failure")
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsSuccess
            .ShouldBeTrue();

        success.ShouldBe("success");
        failure.ShouldBeEmpty();

        Reset();

        Failure<int, string>("error")
            .Tap(() => success = "success", () => failure = "failure")
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsFailure
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        failure.ShouldBe("failure");

        return;

        void Reset()
        {
            success = string.Empty;
            failure = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTapSuccess()
    {
        var result = string.Empty;
        
        Success(1)
            .TapSuccess(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBe("1");

        result = string.Empty;

        Failure<string, string>("error")
            .TapSuccess(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Result<string, string>));
        
        result.ShouldBeEmpty();

        Success(1)
            .TapSuccess(() => result = "success")
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBe("success");

        result = string.Empty;

        Failure<string, string>("error")
            .TapSuccess(() => result = "success")
            .AssertInstanceOfType(typeof(Result<string, string>));

        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void ResultShouldTapFailure()
    {
        var result = string.Empty;

        Success(1)
            .TapFailure(exn => result = exn.Message)
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBeEmpty();

        Success(1)
            .TapFailure(() => result = "failure")
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBeEmpty();

        Failure<string, Exception>(new Exception("error"))
            .TapFailure(exn => result = exn.Message)
            .AssertInstanceOfType(typeof(Result<string, Exception>));

        result.ShouldBe("error");

        result = string.Empty;

        Failure<string, Exception>(new Exception("error"))
            .TapFailure(() => result = "failure")
            .AssertInstanceOfType(typeof(Result<string, Exception>));

        result.ShouldBe("failure");
    }
    
    
    
    [TestMethod]
    public void ItShouldReduceSuccesses() =>
        Success("success")
            .Reduce("failure")
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFunctions() =>
        Success("success")
            .Reduce(() => "something else")
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFunctionsAndFailureResults() =>
        Success("success")
            .Reduce(failure => failure.Message)
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceFailures() =>
        Failure<string, string>("failure message")
            .Reduce("another message")
            .ShouldBe("another message");

    [TestMethod]
    public void ItShouldReduceFailuresWithFunctions() =>
        Failure<string, string>("failure message")
            .Reduce(() => "something else")
            .ShouldBe("something else");

    [TestMethod]
    public void ItShouldReduceFailuresWithFunctionsAndFailureResults() =>
        Failure<string, string>("failure message")
            .Reduce(failure => failure)
            .ShouldBe("failure message");

    [TestMethod]
    public Task ItShouldReduceSuccessesAsync() =>
        Success("success")
            .Async()
            .ReduceAsync("alternate")
            .EffectAsync(result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFunctionsAsync() =>
        Success("success")
            .Async()
            .ReduceAsync(() => "alternate")
            .EffectAsync(result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFunctionsAsyncFailure() =>
        Success("success")
            .Async()
            .ReduceAsync(exn => exn.Message)
            .EffectAsync(result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceFailuresAsync() =>
        Failure<string, string>("failure message")
            .Async()
            .ReduceAsync("alternate")
            .EffectAsync(result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFunctionsAsync() =>
        Failure<string, string>("failure message")
            .Async()
            .ReduceAsync(() => "alternate")
            .EffectAsync(result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceFailuresWithFunctionsAsyncFailure() =>
        Failure<string, string>("failure message")
            .Async()
            .ReduceAsync(failure => failure)
            .EffectAsync(result => result.ShouldBe("failure message"));

}