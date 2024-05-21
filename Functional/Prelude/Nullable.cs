namespace Functional;

public static partial class Prelude
{
    public static T? AsNullable<T>(this T value) where T : notnull => value;

    public static async Task<T?> AsNullable<T>(this Task<T> value) where T : notnull =>
        await value;

    public static async Task<Option<T>> FromNullable<T>(this Task<T?> value) where T : notnull
    {
        T? result = await value;
        Option<T> option = new Option<T>(result);
        return option;
    }
}