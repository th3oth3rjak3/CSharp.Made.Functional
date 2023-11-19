using System.Diagnostics.CodeAnalysis;

using Functional.Unions;

namespace Functional.Test.Unions;

[TestClass]
[ExcludeFromCodeCoverage]
public class Union9Tests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeOne())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeTwo())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldMatchCase3() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeThree())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type three");

    [TestMethod]
    public void ItShouldMatchCase4() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeFour())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type four");

    [TestMethod]
    public void ItShouldMatchCase5() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeFive())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type five");

    [TestMethod]
    public void ItShouldMatchCase6() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeSix())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type six");

    [TestMethod]
    public void ItShouldMatchCase7() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeSeven())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type seven");

    [TestMethod]
    public void ItShouldMatchCase8() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeEight())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type eight");

    [TestMethod]
    public void ItShouldMatchCase9() =>
        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeNine())
            .Match(
                _ => "type one",
                _ => "type two",
                _ => "type three",
                _ => "type four",
                _ => "type five",
                _ => "type six",
                _ => "type seven",
                _ => "type eight",
                _ => "type nine")
            .ShouldBe("type nine");

    [TestMethod]
    public void ItShouldPerformTypeOneEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeOne())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeTrue();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeFalse();
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
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeTwo())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeTrue();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeFalse();

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
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeThree())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeTrue();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeFalse();
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
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeFour())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeTrue();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeFalse();

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
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeFive())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeTrue();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeFalse();
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
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeSix())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeTrue();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeSevenEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeSeven())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeTrue();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeEightEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeEight())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeTrue();
        typeNineEffect.ShouldBeFalse();

    }

    [TestMethod]
    public void ItShouldPerformTypeNineEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        var typeThreeEffect = false;
        var typeFourEffect = false;
        var typeFiveEffect = false;
        var typeSixEffect = false;
        var typeSevenEffect = false;
        var typeEightEffect = false;
        var typeNineEffect = false;

        new Union9<TypeOne, TypeTwo, TypeThree, TypeFour, TypeFive, TypeSix, TypeSeven, TypeEight, TypeNine>(new TypeNine())
            .Effect(
                _ => typeOneEffect = true,
                _ => typeTwoEffect = true,
                _ => typeThreeEffect = true,
                _ => typeFourEffect = true,
                _ => typeFiveEffect = true,
                _ => typeSixEffect = true,
                _ => typeSevenEffect = true,
                _ => typeEightEffect = true,
                _ => typeNineEffect = true);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeFalse();
        typeThreeEffect.ShouldBeFalse();
        typeFourEffect.ShouldBeFalse();
        typeFiveEffect.ShouldBeFalse();
        typeSixEffect.ShouldBeFalse();
        typeSevenEffect.ShouldBeFalse();
        typeEightEffect.ShouldBeFalse();
        typeNineEffect.ShouldBeTrue();

    }
}
