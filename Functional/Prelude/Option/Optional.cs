namespace Functional;

public static partial class Prelude
{
    // TODO: Examples
    /// <summary>
    /// Create an Option that represents some value.
    /// </summary>
    /// <typeparam name="T">The type of the inner content.</typeparam>
    /// <param name="entity">The contents to store.</param>
    /// <returns>A new Option with some data inside.</returns>
    public static Option<T> Some<T>(T entity) where T : notnull =>
        new(entity);

    // TODO: Examples
    /// <summary>
    /// Create an Option that represents no value.
    /// </summary>
    /// <typeparam name="T">The type of the contents if they had been present.</typeparam>
    /// <returns>A new Option that represents a lack of contents.</returns>
    public static Option<T> None<T>() where T : notnull =>
        new();

    // TODO: Examples
    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static Option<T> Optional<T>(this T? entity) where T : notnull =>
        entity switch
        {
            null => None<T>(),
            _ => new(entity)
        };

    // TODO: Examples
    // TODO: Finish documentation
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="entity"></param>
    /// <returns></returns>
    public static Option<T> Optional<T>(this T? entity) where T : struct =>
        entity.HasValue switch
        {
            true => entity.Value.Pipe(Some),
            false => None<T>(),
        };

    // TODO: Examples
    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this Task<T?> entity) where T : notnull
    {
        var result = await entity;
        return result.Optional();
    }

    // TODO: Examples
    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this ValueTask<T?> entity) where T : notnull
    {
        var result = await entity;
        return result.Optional();
    }

    // TODO: Examples
    /// <summary>
    /// Convert any value to an Option type. When null, it will become None, otherwise Some.
    /// </summary>
    /// <typeparam name="T">The type of the original entity.</typeparam>
    /// <param name="entity">The entity to convert to an Option.</param>
    /// <returns>A new option.</returns>
    public static async Task<Option<T>> Optional<T>(this ValueTask<T?> entity) where T : struct
    {
        var result = await entity;
        return result.Optional();
    }
}