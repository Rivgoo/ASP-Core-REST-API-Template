using System.Diagnostics.CodeAnalysis;

namespace Application;

/// <summary>
/// Provides static helper methods for performing common validation checks, returning boolean results.
/// </summary>
/// <remarks>
/// This utility class offers simple guard clauses for input validation, suitable when a boolean outcome is preferred over exceptions.
/// </remarks>
internal static class Guard
{
	/// <summary>
	/// Checks if the specified value is null.
	/// </summary>
	/// <typeparam name="T">The type of the value.</typeparam>
	/// <param name="value">The value to check.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is null; otherwise, <see langword="false"/>.</returns>
	public static bool Null<T>([NotNullWhen(false)] T? value) => value is null;

	/// <summary>
	/// Checks if the specified string is null or an empty string ("").
	/// </summary>
	/// <param name="value">The string to check.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is null or empty; otherwise, <see langword="false"/>.</returns>
	public static bool NullOrEmpty([NotNullWhen(false)] string? value) => string.IsNullOrEmpty(value);

	/// <summary>
	/// Checks if the specified string is null, empty, or consists only of white-space characters.
	/// </summary>
	/// <param name="value">The string to check.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is null, empty, or whitespace; otherwise, <see langword="false"/>.</returns>
	public static bool NullOrWhiteSpace([NotNullWhen(false)] string? value) => string.IsNullOrWhiteSpace(value);

	/// <summary>
	/// Checks if the length of the specified string exceeds a maximum length.
	/// </summary>
	/// <param name="value">The string to check. Null or whitespace strings are not considered to exceed the length.</param>
	/// <param name="maxLength">The maximum allowed length.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is not null/whitespace and its length is greater than <paramref name="maxLength"/>; otherwise, <see langword="false"/>.</returns>
	/// <remarks>
	/// Returns <see langword="false"/> if <paramref name="value"/> is null, empty, or consists only of white-space.
	/// </remarks>
	public static bool MaxLength(string? value, int maxLength) => !NullOrWhiteSpace(value) && value!.Length > maxLength;


	/// <summary>
	/// Checks if the specified string's length is less than a minimum length or if the string is null or whitespace.
	/// </summary>
	/// <param name="value">The string to check.</param>
	/// <param name="minLength">The minimum required length.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is null, empty, whitespace, or its length is less than <paramref name="minLength"/>; otherwise, <see langword="false"/>.</returns>
	/// <remarks>
	/// Considers null, empty, or whitespace strings as failing the minimum length check if <paramref name="minLength"/> > 0.
	/// </remarks>
	public static bool MinLength([NotNullWhen(false)] string? value, int minLength) => NullOrWhiteSpace(value) || value!.Length < minLength;


	/// <summary>
	/// Checks if the specified integer is less than a minimum value.
	/// </summary>
	/// <param name="value">The integer to check.</param>
	/// <param name="min">The minimum allowed value.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is less than <paramref name="min"/>; otherwise, <see langword="false"/>.</returns>
	public static bool Min(int value, int min) => value < min;

	/// <summary>
	/// Checks if the specified integer is greater than a maximum value.
	/// </summary>
	/// <param name="value">The integer to check.</param>
	/// <param name="max">The maximum allowed value.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is greater than <paramref name="max"/>; otherwise, <see langword="false"/>.</returns>
	public static bool Max(int value, int max) => value > max;

	/// <summary>
	/// Checks if the specified decimal is less than a minimum value.
	/// </summary>
	/// <param name="value">The decimal to check.</param>
	/// <param name="min">The minimum allowed value.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is less than <paramref name="min"/>; otherwise, <see langword="false"/>.</returns>
	public static bool Min(decimal value, decimal min) => value < min;

	/// <summary>
	/// Checks if the specified decimal is greater than a maximum value.
	/// </summary>
	/// <param name="value">The decimal to check.</param>
	/// <param name="max">The maximum allowed value.</param>
	/// <returns><see langword="true"/> if <paramref name="value"/> is greater than <paramref name="max"/>; otherwise, <see langword="false"/>.</returns>
	public static bool Max(decimal value, decimal max) => value > max;
}