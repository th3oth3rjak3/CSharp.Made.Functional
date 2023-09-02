using System.Collections.Immutable;

using Functional.Monadic;
using Functional.Results;
using Functional.Validation;

using static Functional.Validation.ValidationExtensions;

namespace Functional.Test.Validation;

[TestClass]
public class ValidationResultTests
{
	[TestMethod]
	public void ItShouldReturnValidationSuccess() =>
		ValidationResult
			.Success("something")
			.Match(
				success => success.Tap(s => s.ShouldBe("something")),
				failure => throw new ShouldAssertException("Shouldn't have failed."));

	[TestMethod]
	public void ItShouldReturnValidationFailure() =>
		ValidationResult
			.Failure<string>("failure message")
			.Match(
				success => throw new ShouldAssertException("Shouldn't have succeeded"),
				failure =>
					failure
						.Tap(
							f => f.FailureMessages.Count.ShouldBe(1),
							f => f.FailureMessages.First().ShouldBe("failure message")));

	[TestMethod]
	public void ItShouldReturnValidationFailureWithMessages() =>
		ImmutableList<string>
			.Empty
			.Add("zero")
			.Add("one")
			.Pipe(ValidationResult.Failure<string>)
			.Match(
				success => throw new ShouldAssertException("Shouldn't have succeeded"),
				failure =>
					failure.Tap(
						f => f.FailureMessages.Count.ShouldBe(2),
						f => f.FailureMessages.ElementAt(0).ShouldBe("zero"),
						f => f.FailureMessages.ElementAt(1).ShouldBe("one")));

	[TestMethod]
	public void ItShouldMapSuccessWithSuccessToSuccess() =>
		ValidationResult
			.Success("something")
			.Pipe(something =>
				something
					.Bind(something, _ => ValidationResult.Success("anything else successful")))
			.Match(
				success => success.Tap(s => s.ShouldBe("anything else successful")),
				failure => throw new ShouldAssertException("Shouldn't have been invalid."));

	[TestMethod]
	public void ItShouldMapSuccessWithFailuresToFailures() =>
		ValidationResult
			.Success("something")
			.Pipe(something =>
				something
					.Bind(something, _ => ValidationResult.Failure<string>("It failed")))
			.Match(
				success => throw new ShouldAssertException("It should have failed."),
				failure =>
					failure
						.Tap(
							f => f.FailureMessages.Count.ShouldBe(1),
							f => f.FailureMessages.First().ShouldBe("It failed")));

	[TestMethod]
	public void ItShouldMapFailureWithSuccesssToFailures() =>
		ValidationResult
			.Failure<string>("failure message")
			.Pipe(something =>
				something
					.Bind(something, _ => ValidationResult.Success("something else")))
			.Match(
				success => throw new ShouldAssertException("It should have failed."),
				failure =>
					failure
						.Tap(
							f => f.FailureMessages.Count.ShouldBe(1),
							f => f.FailureMessages.First().ShouldBe("failure message")));

	[TestMethod]
	public void ItShouldJoinMultipleFailureMessages() =>
		ValidationResult
			.Failure<string>("failure one")
			.Pipe(something =>
				something
					.Bind(something, _ => ValidationResult.Failure<string>("failure two")))
			.Match(
				success => throw new ShouldAssertException("It should have failed."),
				failure =>
					failure
						.Tap(
							f => f.FailureMessages.Count.ShouldBe(2),
							f => f.FailureMessages.ElementAt(0).ShouldBe("failure one"),
							f => f.FailureMessages.ElementAt(1).ShouldBe("failure two")));

	[TestMethod]
	public void ItShouldBeAValidationResult() =>
		"something"
			.AsValidationResult()
			.ShouldBe(ValidationResult.Success("something"));

	[TestMethod]
	public void MatchSuccessShouldMapValidationResultToResult() =>
		"something"
			.AsValidationResult()
			.Match(
				MatchSuccess,
				MatchFailure)
			.ShouldBe(Result.Success("something"));

	[TestMethod]
	public void MatchShouldMatchSuccessAndFailureToResult() =>
		ValidationResult
			.Failure<string>("failure message")
			.Match(
				MatchSuccess,
				MatchFailure)
			.Match(
				success => throw new ShouldAssertException("shouldn't have succeeded"),
				failure => failure.Tap(
					f => f.FailureMessages.Count.ShouldBe(1),
					f => f.FailureMessages.First().ShouldBe("failure message")));

	[TestMethod]
	public void MatchDefaultShouldMapValidationResultToResult() =>
	"something"
		.AsValidationResult()
		.MatchDefault()
		.ShouldBe(Result.Success("something"));

	[TestMethod]
	public void MatchDefaultMatchSuccessAndFailureToResult() =>
		ValidationResult
			.Failure<string>("failure message")
			.MatchDefault()
			.Match(
				success => throw new ShouldAssertException("shouldn't have succeeded"),
				failure => failure.Tap(
					f => f.FailureMessages.Count.ShouldBe(1),
					f => f.FailureMessages.First().ShouldBe("failure message")));

	[TestMethod]
	public void ItShouldMatchSuccess() =>
		ValidationResult
			.Success(1)
			.Match(
				success => success.ToString(),
				failure => failure.FailureMessages.First())
			.ShouldBe("1");

	[TestMethod]
	public void ItShouldMatchFailures() =>
		ValidationResult
			.Failure<int>("error")
			.Match(
				success => success.ToString(),
				failure => failure.FailureMessages.First())
			.ShouldBe("error");

	[TestMethod]
	public void ItShouldThrowForFakeValidationResults() =>
		Assert.ThrowsException<InvalidOperationException>(
			() =>
				new FakeValidationResult<int>(1)
					.Match(
						success => success.ToString(),
						failure => failure.FailureMessages.First())
					.Ignore());

	[TestMethod]
	public async Task ItShouldMatchSuccessAsync() =>
		await ValidationResult
			.Success(1)
			.AsAsync()
			.MatchAsync(
				success => success.ToString(),
				failure => failure.FailureMessages.First())
			.TapAsync(res => res.ShouldBe("1"));

	[TestMethod]
	public async Task ItShouldMatchFailuresAsync() =>
		await ValidationResult
			.Failure<int>("error")
			.AsAsync()
			.MatchAsync(
				success => success.ToString(),
				failure => failure.FailureMessages.First())
			.TapAsync(res => res.ShouldBe("error"));
}

internal record FakeValidationResult<T>(T Contents) : ValidationResult<T>;