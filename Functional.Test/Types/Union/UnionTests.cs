using System.Diagnostics.CodeAnalysis;

namespace Functional.Test.Unions;

[TestClass]
[ExcludeFromCodeCoverage]
public class UnionTests
{
    [TestMethod]
    public void ItShouldMatchCase1() =>
        new Union<TypeOne, TypeTwo>(new TypeOne())
            .Match(_ => "type one", _ => "type two")
            .ShouldBe("type one");

    [TestMethod]
    public void ItShouldMatchCase2() =>
        new Union<TypeOne, TypeTwo>(new TypeTwo())
            .Match(_ => "type one", _ => "type two")
            .ShouldBe("type two");

    [TestMethod]
    public void ItShouldPerformTypeOneEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        void typeOneAction(TypeOne _) => typeOneEffect = true;
        void twoTwoAction(TypeTwo _) => typeTwoEffect = true;

        new Union<TypeOne, TypeTwo>(new TypeOne())
            .Effect(typeOneAction, twoTwoAction);

        typeOneEffect.ShouldBeTrue();
        typeTwoEffect.ShouldBeFalse();
    }

    [TestMethod]
    public void ItShouldPerformTypeTwoEffect()
    {
        var typeOneEffect = false;
        var typeTwoEffect = false;
        void typeOneAction(TypeOne _) => typeOneEffect = true;
        void typeTwoAction(TypeTwo _) => typeTwoEffect = true;

        new Union<TypeOne, TypeTwo>(new TypeTwo())
            .Effect(typeOneAction, typeTwoAction);

        typeOneEffect.ShouldBeFalse();
        typeTwoEffect.ShouldBeTrue();
    }
}

