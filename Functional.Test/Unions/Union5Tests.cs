using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Test.Unions;

[TestClass]
[ExcludeFromCodeCoverage]
public class Union5Tests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
    new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeOne())
        .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
        .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeTwo())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldMatchCase3() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeThree())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type three");

    [TestMethod]
    public void ItShouldMatchCase4() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeFour())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type four");

    [TestMethod]
    public void ItShouldMatchCase5() =>
        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeFive())
            .Match(_ => "type one", _ => "type two", _ => "type three", _ => "type four", _ => "type five")
            .ShouldBe("type five");

    [TestMethod]
    public void ItShouldPerformTypeOneEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;

        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeOne())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true);

        typeOneEffect.ShouldBeTrue();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeTwoEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;

        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeTwo())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeTrue();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeThreeEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;

        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeThree())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeTrue();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeFourEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;

        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeFour())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeTrue();
        typeFiveEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeFiveEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;

        new Union5<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive>(new TypeFive())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeTrue();

    }
}
