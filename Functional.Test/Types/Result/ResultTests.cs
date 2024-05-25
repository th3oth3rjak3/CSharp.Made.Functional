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
            .IsOk
            .ShouldBeTrue();

        new Result<string, Exception>(new Exception("Error"))
            .AssertInstanceOfType(typeof(Result<string, Exception>))
            .IsError
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ResultShouldHaveAnImplicitOperator()
    {
        static Result<string, Exception> DoWork(int input)
        {
            if (input < 20) return input.ToString();
            return new Exception("input too large");
        }

        DoWork(10).IsOk.ShouldBeTrue();
        DoWork(30).IsError.ShouldBeTrue();
    }

    [TestMethod]
    public void ResultShouldUnwrap()
    {
        new Result<string, Exception>("hello world")
            .Unwrap()
            .ShouldBe("hello world");

        Assert.ThrowsException<InvalidOperationException>(() =>
            new Result<string, Exception>(new Exception("Error"))
                .Unwrap());

        new Result<string, Exception>(new Exception("Error"))
            .UnwrapError()
            .Message
            .ShouldBe("Error");

        Assert.ThrowsException<InvalidOperationException>(() =>
            new Result<string, Exception>("success")
                .UnwrapError());
    }

    [TestMethod]
    public void ResultShouldMatch()
    {
        Ok(1)
            .Match(value => value, _ => -1)
            .ShouldBe(1);

        Error<int>(new Exception("error"))
            .Match(value => value, _ => -1)
            .ShouldBe(-1);

        Ok(1)
            .Match(() => 2, _ => -1)
            .ShouldBe(2);

        Error<int>(new Exception("error"))
            .Match(() => 2, _ => -1)
            .ShouldBe(-1);

        Ok(1)
            .Match(value => value, () => -1)
            .ShouldBe(1);

        Error<int>(new Exception("error"))
            .Match(value => value, () => -1)
            .ShouldBe(-1);

        Ok(1)
            .Match(() => 1, () => -1)
            .ShouldBe(1);

        Error<int>(new Exception("error"))
            .Match(() => 1, () => -1)
            .ShouldBe(-1);
    }

    [TestMethod]
    public void ResultShouldMapSuccesses()
    {
        Ok(1)
            .Map(value => value.ToString())
            .Unwrap()
            .ShouldBe("1");

        Ok(1)
            .Map(() => "something")
            .Unwrap()
            .ShouldBe("something");

        Error<int, string>("fail")
            .Map(value => value.ToString())
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsError
            .ShouldBeTrue();

        Error<int, string>("fail")
            .Map(() => "something")
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsError
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ResultShouldMapErrors()
    {
        Ok<int, string>(1)
            .MapError(err => new Exception(err))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .Unwrap()
            .ShouldBe(1);

        Ok<int, string>(1)
            .MapError(() => new Exception("something else"))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .Unwrap()
            .ShouldBe(1);

        Error<int, string>("Error")
            .MapError(err => new Exception(err))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .UnwrapError()
            .Message
            .ShouldBe("Error");

        Error<int, string>("ignored")
            .MapError(() => new Exception("expected"))
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .UnwrapError()
            .Message
            .ShouldBe("expected");
    }

    [TestMethod]
    public void ResultShouldBindToSuccesses()
    {
        Ok<int, string>(1)
            .Bind(value => Ok<string, string>(value.ToString()))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .Unwrap()
            .ShouldBe("1");

        Ok<int, string>(1)
            .Bind(() => Ok<string, string>("new value"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .Unwrap()
            .ShouldBe("new value");

        Error<int, string>("Error")
            .Bind(value => Ok<string, string>(value.ToString()))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsError
            .ShouldBeTrue();

        Error<int, string>("Error")
            .Bind(() => Ok<string, string>("new value"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .IsError
            .ShouldBeTrue();
    }

    [TestMethod]
    public void ResultShouldBindToErrors()
    {
        Ok<int, string>(1)
            .Bind(value => Error<string, string>("Error!"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapError()
            .ShouldBe("Error!");

        Ok<int, string>(1)
            .Bind(() => Error<string, string>("Error!"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapError()
            .ShouldBe("Error!");

        Error<int, string>("original")
            .Bind(value => Error<string, string>("ignored"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapError()
            .ShouldBe("original");

        Error<int, string>("original")
            .Bind(() => Error<string, string>("ignored"))
            .AssertInstanceOfType(typeof(Result<string, string>))
            .UnwrapError()
            .ShouldBe("original");
    }

    [TestMethod]
    public void ResultShouldPerformEffects_1()
    {
        var successResult = string.Empty;
        var ErrorResult = string.Empty;

        Ok(1)
            .Effect(
                value => successResult = value.ToString(),
                exn => ErrorResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("1");
        ErrorResult.ShouldBe(string.Empty);

        Reset();

        Error<int>(new Exception("error"))
            .Effect(
                value => successResult = value.ToString(),
                exn => ErrorResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        ErrorResult.ShouldBe("error");

        return;

        void Reset()
        {
            successResult = string.Empty;
            ErrorResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffects_2()
    {
        var successResult = string.Empty;
        var ErrorResult = string.Empty;

        Ok(1)
            .Effect(
                () => successResult = "success",
                exn => ErrorResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("success");
        ErrorResult.ShouldBe(string.Empty);

        Reset();

        Error<int>(new Exception("error"))
            .Effect(
                () => successResult = "success",
                exn => ErrorResult = exn.Message)
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        ErrorResult.ShouldBe("error");

        return;

        void Reset()
        {
            successResult = string.Empty;
            ErrorResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffects_3()
    {
        var successResult = string.Empty;
        var ErrorResult = string.Empty;

        Ok(1)
            .Effect(
                value => successResult = value.ToString(),
                () => ErrorResult = "Error")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("1");
        ErrorResult.ShouldBe(string.Empty);

        Reset();

        Error<int>(new Exception("error"))
            .Effect(
                value => successResult = value.ToString(),
                () => ErrorResult = "Error")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        ErrorResult.ShouldBe("Error");

        return;

        void Reset()
        {
            successResult = string.Empty;
            ErrorResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffects_4()
    {
        var successResult = string.Empty;
        var ErrorResult = string.Empty;

        Ok(1)
            .Effect(
                () => successResult = "success",
                () => ErrorResult = "Error")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe("success");
        ErrorResult.ShouldBe(string.Empty);

        Reset();

        Error<int>(new Exception("error"))
            .Effect(
                () => successResult = "success",
                () => ErrorResult = "Error")
            .AssertInstanceOfType(typeof(Unit));

        successResult.ShouldBe(string.Empty);
        ErrorResult.ShouldBe("Error");


        return;

        void Reset()
        {
            successResult = string.Empty;
            ErrorResult = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldPerformEffecTOk()
    {
        string result = string.Empty;

        Error<int, string>("error")
            .EffectOk(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Error<int, string>("error")
            .EffectOk(() => result = "success")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Ok(1)
            .EffectOk(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("1");

        Ok(1)
            .EffectOk(() => result = "success")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("success");
    }

    [TestMethod]
    public void ResultShouldPerformEffecTError()
    {
        string result = string.Empty;

        Ok(1)
            .EffectError(value => result = value.Message)
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Ok(1)
            .EffectError(() => result = "Error")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBeEmpty();

        Error<int, string>("fail")
            .EffectError(value => result = value)
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("fail");

        Error<int, string>("fail")
            .EffectError(() => result = "Error")
            .AssertInstanceOfType(typeof(Unit));

        result.ShouldBe("Error");
    }

    [TestMethod]
    public void ResultShouldTap_1()
    {
        var success = string.Empty;
        var Error = string.Empty;

        Ok(1)
            .Tap(value => success = value.ToString(), err => Error = err.Message)
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsOk
            .ShouldBeTrue();

        success.ShouldBe("1");
        Error.ShouldBeEmpty();

        Reset();

        Error<int, string>("error")
            .Tap(value => success = value.ToString(), err => Error = err)
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsError
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        Error.ShouldBe("error");

        return;

        void Reset()
        {
            success = string.Empty;
            Error = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTap_2()
    {
        var success = string.Empty;
        var Error = string.Empty;

        Ok(1)
            .Tap(() => success = "success", err => Error = err.Message)
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsOk
            .ShouldBeTrue();

        success.ShouldBe("success");
        Error.ShouldBeEmpty();

        Reset();

        Error<int, string>("error")
            .Tap(() => success = "success", err => Error = err)
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsError
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        Error.ShouldBe("error");

        return;

        void Reset()
        {
            success = string.Empty;
            Error = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTap_3()
    {
        var success = string.Empty;
        var Error = string.Empty;

        Ok(1)
            .Tap(value => success = value.ToString(), () => Error = "Error")
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsOk
            .ShouldBeTrue();

        success.ShouldBe("1");
        Error.ShouldBeEmpty();

        Reset();

        Error<int, string>("error")
            .Tap(value => success = value.ToString(), () => Error = "Error")
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsError
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        Error.ShouldBe("Error");

        return;

        void Reset()
        {
            success = string.Empty;
            Error = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTap_4()
    {
        var success = string.Empty;
        var Error = string.Empty;

        Ok(1)
            .Tap(() => success = "success", () => Error = "Error")
            .AssertInstanceOfType(typeof(Result<int, Exception>))
            .IsOk
            .ShouldBeTrue();

        success.ShouldBe("success");
        Error.ShouldBeEmpty();

        Reset();

        Error<int, string>("error")
            .Tap(() => success = "success", () => Error = "Error")
            .AssertInstanceOfType(typeof(Result<int, string>))
            .IsError
            .ShouldBeTrue();

        success.ShouldBeEmpty();
        Error.ShouldBe("Error");

        return;

        void Reset()
        {
            success = string.Empty;
            Error = string.Empty;
        }
    }

    [TestMethod]
    public void ResultShouldTapSuccess()
    {
        var result = string.Empty;

        Ok(1)
            .TapOk(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBe("1");

        result = string.Empty;

        Error<string, string>("error")
            .TapOk(value => result = value.ToString())
            .AssertInstanceOfType(typeof(Result<string, string>));

        result.ShouldBeEmpty();

        Ok(1)
            .TapOk(() => result = "success")
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBe("success");

        result = string.Empty;

        Error<string, string>("error")
            .TapOk(() => result = "success")
            .AssertInstanceOfType(typeof(Result<string, string>));

        result.ShouldBeEmpty();
    }

    [TestMethod]
    public void ResultShouldTapError()
    {
        var result = string.Empty;

        Ok(1)
            .TapError(exn => result = exn.Message)
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBeEmpty();

        Ok(1)
            .TapError(() => result = "Error")
            .AssertInstanceOfType(typeof(Result<int, Exception>));

        result.ShouldBeEmpty();

        Error<string, Exception>(new Exception("error"))
            .TapError(exn => result = exn.Message)
            .AssertInstanceOfType(typeof(Result<string, Exception>));

        result.ShouldBe("error");

        result = string.Empty;

        Error<string, Exception>(new Exception("error"))
            .TapError(() => result = "Error")
            .AssertInstanceOfType(typeof(Result<string, Exception>));

        result.ShouldBe("Error");
    }



    [TestMethod]
    public void ItShouldReduceSuccesses() =>
        Ok("success")
            .Reduce("Error")
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFunctions() =>
        Ok("success")
            .Reduce(() => "something else")
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceSuccessWithFunctionsAndErrorResults() =>
        Ok("success")
            .Reduce(Error => Error.Message)
            .ShouldBe("success");

    [TestMethod]
    public void ItShouldReduceErrors() =>
        Error<string, string>("Error message")
            .Reduce("another message")
            .ShouldBe("another message");

    [TestMethod]
    public void ItShouldReduceErrorsWithFunctions() =>
        Error<string, string>("Error message")
            .Reduce(() => "something else")
            .ShouldBe("something else");

    [TestMethod]
    public void ItShouldReduceErrorsWithFunctionsAndErrorResults() =>
        Error<string, string>("Error message")
            .Reduce(Error => Error)
            .ShouldBe("Error message");

    [TestMethod]
    public Task ItShouldReduceSuccessesAsync() =>
        Ok("success")
            .Async()
            .ReduceAsync("alternate")
            .EffectAsync(result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFunctionsAsync() =>
        Ok("success")
            .Async()
            .ReduceAsync(() => "alternate")
            .EffectAsync(result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceSuccessWithFunctionsAsyncError() =>
        Ok("success")
            .Async()
            .ReduceAsync(exn => exn.Message)
            .EffectAsync(result => result.ShouldBe("success"));

    [TestMethod]
    public Task ItShouldReduceErrorsAsync() =>
        Error<string, string>("Error message")
            .Async()
            .ReduceAsync("alternate")
            .EffectAsync(result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceErrorsWithFunctionsAsync() =>
        Error<string, string>("Error message")
            .Async()
            .ReduceAsync(() => "alternate")
            .EffectAsync(result => result.ShouldBe("alternate"));

    [TestMethod]
    public Task ItShouldReduceErrorsWithFunctionsAsyncError() =>
        Error<string, string>("Error message")
            .Async()
            .ReduceAsync(Error => Error)
            .EffectAsync(result => result.ShouldBe("Error message"));

}