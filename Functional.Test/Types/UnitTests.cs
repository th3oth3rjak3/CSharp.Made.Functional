using System.Diagnostics.CodeAnalysis;


namespace Functional.Test.Common;

[ExcludeFromCodeCoverage]
[TestClass]
public class UnitTests
{
    [TestMethod]
    public void UnitShouldBeEqualToAnotherUnit() =>
        Assert.AreEqual(new Unit(), new Unit());

    [TestMethod]
    public void CompareToShouldReturnZeroForUnit() =>
        new Unit()
            .CompareTo(new Unit())
            .Tap(result => Assert.AreEqual(result, 0));

    [TestMethod]
    public void GetHashCodeReturnsZeroForUnit() =>
        new Unit()
        .GetHashCode()
        .Tap(hashCode => Assert.AreEqual(hashCode, 0));

    [TestMethod]
    public void EqualsShouldReturnTrueForUnit() =>
        new Unit()
        .Equals(new Unit())
        .Tap(Assert.IsTrue);

    [TestMethod]
    public void EqualsShouldNotReturnTrueForOthers() =>
        new Unit()
        .Equals(new { })
        .Tap(Assert.IsFalse);

    [TestMethod]
    public void ToStringShouldReturnUnitDefinition() =>
        new Unit()
        .ToString()
        .Tap(unit => Assert.AreEqual(unit, "()"));

    [TestMethod]
    public void EqualsOperatorShouldReturnTrueForUnit() =>
        Assert.IsTrue(new Unit() == new Unit());

    [TestMethod]
    public void NotEqualsOperatorShouldReturnFalse() =>
        Assert.IsFalse(new Unit() != new Unit());

    [TestMethod]
    public void GreaterThanShouldReturnFalseForUnit() =>
        Assert.IsFalse(new Unit() > new Unit());

    [TestMethod]
    public void LessThanShouldReturnFalseForUnit() =>
        Assert.IsFalse(new Unit() < new Unit());

    [TestMethod]
    public void GreaterThanOrEqualToShouldReturnTrueForUnit() =>
        Assert.IsTrue(new Unit() >= new Unit());

    [TestMethod]
    public void LessThanOrEqualToShouldReturnTrueForUnit() =>
        Assert.IsTrue(new Unit() <= new Unit());

    [TestMethod]
    public void DefaultShouldReturnNewUnit() =>
        Assert.IsInstanceOfType<Unit>(Unit.Default);

    [TestMethod]
    public void AddingUnitShouldReturnUnit() =>
        Assert.AreEqual(new Unit(), new Unit() + new Unit());

    [TestMethod]
    public void UnitShouldPipeNewValues() =>
        Assert.AreEqual(new Unit().Pipe(1), 1);

    [TestMethod]
    public void UnitShouldPipeNewFunctions() =>
        Assert.AreEqual(new Unit().Pipe(() => 1), 1);

    [TestMethod]
    public void UnitShouldWorkWithOptionalExtension() =>
        (null as Unit?)
            .Optional()
            .ShouldBeOfType<Option<Unit>>();
}
