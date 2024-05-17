//namespace Functional.Unions;

///// <summary>
///// Represents a Discriminated Union which contains seven Variants.
///// </summary>
///// <typeparam name="A">The type of the first variant.</typeparam>
///// <typeparam name="B">The type of the second variant.</typeparam>
///// <typeparam name="C">The type of the third variant.</typeparam>
///// <typeparam name="D">The type of the fourth variant.</typeparam>
///// <typeparam name="E">The type of the fifth variant.</typeparam>
///// <typeparam name="F">The type of the sixth variant.</typeparam>
///// <typeparam name="G">The type of the seventh variant.</typeparam>
//public sealed class Union7<A, B, C, D, E, F, G>
//{
//    private A Item1 { get; init; } = default!;
//    private B Item2 { get; init; } = default!;
//    private C Item3 { get; init; } = default!;
//    private D Item4 { get; init; } = default!;
//    private E Item5 { get; init; } = default!;
//    private F Item6 { get; init; } = default!;
//    private G Item7 { get; init; } = default!;
//    private readonly int tag;

//    /// <summary>
//    /// Construct a new Union from <typeparamref name="A"/>
//    /// </summary>
//    /// <param name="item">The value of the Union.</param>
//    public Union7(A item) { Item1 = item; tag = 1; }

//    /// <summary>
//    /// Construct a new Union from <typeparamref name="B"/>
//    /// </summary>
//    /// <param name="item">The value of the Union.</param>
//    public Union7(B item) { Item2 = item; tag = 2; }

//    /// <summary>
//    /// Construct a new Union from <typeparamref name="C"/>
//    /// </summary>
//    /// <param name="item">The value of the Union.</param>
//    public Union7(C item) { Item3 = item; tag = 3; }

//    /// <summary>
//    /// Construct a new Union from <typeparamref name="D"/>
//    /// </summary>
//    /// <param name="item">The value of the Union.</param>
//    public Union7(D item) { Item4 = item; tag = 4; }

//    /// <summary>
//    /// Construct a new Union from <typeparamref name="E"/>
//    /// </summary>
//    /// <param name="item">The value of the Union.</param>
//    public Union7(E item) { Item5 = item; tag = 5; }

//    /// <summary>
//    /// Construct a new Union from <typeparamref name="F"/>
//    /// </summary>
//    /// <param name="item">The value of the Union.</param>
//    public Union7(F item) { Item6 = item; tag = 6; }

//    /// <summary>
//    /// Construct a new Union from <typeparamref name="G"/>
//    /// </summary>
//    /// <param name="item">The value of the Union.</param>
//    public Union7(G item) { Item7 = item; tag = 7; }

//    /// <summary>
//    /// Match the inner value of one of the variants and perform a mapping function.
//    /// </summary>
//    /// <typeparam name="T">The mapping output type.</typeparam>
//    /// <param name="caseOne">The function to execute if the type is <typeparamref name="A"/></param>
//    /// <param name="caseTwo">The function to execute if the type is <typeparamref name="B"/></param>
//    /// <param name="caseThree">The function to execute if the type is <typeparamref name="C"/></param>
//    /// <param name="caseFour">The function to execute if the type is <typeparamref name="D"/></param>
//    /// <param name="caseFive">The function to execute if the type is <typeparamref name="E"/></param>
//    /// <param name="caseSix">The function to execute if the type is <typeparamref name="F"/></param>
//    /// <param name="caseSeven">The function to execute if the type is <typeparamref name="G"/></param>
//    /// <returns>The result of the mapping function that was executed.</returns>
//    [ExcludeFromCodeCoverage]
//    public T Match<T>(
//        Func<A, T> caseOne,
//        Func<B, T> caseTwo,
//        Func<C, T> caseThree,
//        Func<D, T> caseFour,
//        Func<E, T> caseFive,
//        Func<F, T> caseSix,
//        Func<G, T> caseSeven) =>
//        tag switch
//        {
//            1 => caseOne(Item1),
//            2 => caseTwo(Item2),
//            3 => caseThree(Item3),
//            4 => caseFour(Item4),
//            5 => caseFive(Item5),
//            6 => caseSix(Item6),
//            7 => caseSeven(Item7),
//            _ => throw new NotImplementedException()
//        };

//    /// <summary>
//    /// Match the inner value of one of the variants and perform an action based on its type.
//    /// </summary>
//    /// <param name="caseOne">The action to execute when the inner type is <typeparamref name="A"/></param>
//    /// <param name="caseTwo">The action to execute when the inner type is <typeparamref name="B"/></param>
//    /// <param name="caseThree">The action to execute when the inner type is <typeparamref name="C"/></param>
//    /// <param name="caseFour">The action to execute when the inner type is <typeparamref name="D"/></param>
//    /// <param name="caseFive">The action to execute when the inner type is <typeparamref name="E"/></param>
//    /// <param name="caseSix">The action to execute when the inner type is <typeparamref name="F"/></param>
//    /// <param name="caseSeven">The action to execute when the inner type is <typeparamref name="G"/></param>
//    /// <returns>Unit.</returns>
//    public Unit Effect(
//        Action<A> caseOne,
//        Action<B> caseTwo,
//        Action<C> caseThree,
//        Action<D> caseFour,
//        Action<E> caseFive,
//        Action<F> caseSix,
//        Action<G> caseSeven)
//    {
//        switch (tag)
//        {
//            case 1:
//                caseOne(Item1);
//                break;
//            case 2:
//                caseTwo(Item2);
//                break;
//            case 3:
//                caseThree(Item3);
//                break;
//            case 4:
//                caseFour(Item4);
//                break;
//            case 5:
//                caseFive(Item5);
//                break;
//            case 6:
//                caseSix(Item6);
//                break;
//            case 7:
//                caseSeven(Item7);
//                break;
//        }

//        return Unit.Default;
//    }
//}