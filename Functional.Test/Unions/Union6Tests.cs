using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Test.Unions;

[TestClass]
[ExcludeFromCodeCoverage]
public class Union6Tests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
    new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeOne())
        .Match(
            _ => "type one",
            _ => "type two",
            _ => "type three",
            _ => "type four",
            _ => "type five",
            _ => "type six")
        .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeTwo())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldMatchCase3() =>
        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeThree())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six")
            .ShouldBe("type three");

    [TestMethod]
    public void ItShouldMatchCase4() =>
        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeFour())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six")
            .ShouldBe("type four");

    [TestMethod]
    public void ItShouldMatchCase5() =>
        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeFive())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six")
            .ShouldBe("type five");

    [TestMethod]
    public void ItShouldMatchCase6() =>
        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeSix())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six")
            .ShouldBe("type six");

    [TestMethod]
    public void ItShouldPerformTypeOneEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;

        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeOne())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true);

        typeOneEffect.ShouldBeTrue();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeTwoEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;

        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeTwo())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeTrue();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeThreeEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;

        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeThree())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeTrue();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeFourEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;

        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeFour())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeTrue();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeFiveEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;

        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeFive())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeTrue();
        typeSixEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeSixEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;

        new Union6<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix>(new TypeSix())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeTrue();

    }
}
