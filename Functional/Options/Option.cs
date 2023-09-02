using System.Diagnostics.CodeAnalysis;

namespace Functional.Options;

[ExcludeFromCodeCoverage]
public abstract record Option<T>();

public static class Option
{
	/// <summary>
	/// Create an Option that represents some value.
	/// </summary>
	/// <typeparam name="T">The type of the inner content.</typeparam>
	/// <param name="entity">The contents to store.</param>
	/// <returns>A new Option with some data inside.</returns>
	public static Option<T> Some<T>(this T entity) =>
		new Some<T>(entity);

	/// <summary>
	/// Create an Option that respresents no value.
	/// </summary>
	/// <typeparam name="T">The type of the contents if they had been present.</typeparam>
	/// <returns>A new Option that represents a lack of contents.</returns>
	public static Option<T> None<T>() =>
		new None<T>();
}

[ExcludeFromCodeCoverage]
public record None<T>() : Option<T>;

[ExcludeFromCodeCoverage]
public record Some<T>(T Contents) : Option<T>;