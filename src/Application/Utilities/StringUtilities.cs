using Microsoft.AspNetCore.Identity;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Application.Utilities;

/// <summary>
/// Provides static utility methods for string manipulation and common data validation tasks.
/// </summary>
public static class StringUtilities
{
	private static readonly Regex _emailRegex = new(
		@"^[^@\s]+@[^@\s]+\.[^@\s]+$",
		RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture,
		TimeSpan.FromSeconds(1));

	private static readonly Regex _validPhoneNumberCharsRegex =
			new(@"^[0-9\+\-\(\)\s]+$", RegexOptions.Compiled, TimeSpan.FromSeconds(1));

	/// <summary>
	/// Trims leading and trailing whitespace from all public, readable, and writable string properties of the specified object.
	/// </summary>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <param name="obj">The object whose string properties are to be trimmed. Must not be null.</param>
	/// <exception cref="ArgumentNullException">Thrown if <paramref name="obj"/> is null.</exception>
	/// <remarks>
	/// This method uses reflection to access properties. Null string property values remain null.
	/// </remarks>
	public static void TrimStringProperties<T>(T obj)
		where T : class
	{
		ArgumentNullException.ThrowIfNull(obj);

		IEnumerable<PropertyInfo> stringProperties =
			obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
			.Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

		foreach (PropertyInfo property in stringProperties)
		{
			string? value = property.GetValue(obj) as string;

			property.SetValue(obj, value?.Trim());
		}
	}

	/// <summary>
	/// Validates a phone number string based on allowed characters and digit count.
	/// </summary>
	/// <param name="phoneNumber">The phone number string to validate.</param>
	/// <param name="minimumDigitLength">The minimum number of digits required after removing non-digit characters.</param>
	/// <param name="maximumDigitLength">The maximum number of digits allowed after removing non-digit characters.</param>
	/// <returns><see langword="true"/> if the phone number is considered valid; otherwise, <see langword="false"/>.</returns>
	/// <remarks>
	/// Performs basic validation: checks for null/emptiness, allowed characters (digits, space, hyphen, parentheses, plus),
	/// and digit count within the specified range. Does not verify country-specific formats.
	/// </remarks>
	public static bool ValidatePhoneNumber(string? phoneNumber, int minimumDigitLength, int maximumDigitLength)
	{
		if (string.IsNullOrEmpty(phoneNumber))
			return false;

		if (!_validPhoneNumberCharsRegex.IsMatch(phoneNumber))
			return false;

		string digitsOnly = Regex.Replace(phoneNumber, @"[^0-9]", "");

		if (digitsOnly.Length < minimumDigitLength || digitsOnly.Length > maximumDigitLength)
			return false;

		return true;
	}

	/// <summary>
	/// Validates the format of an email address string using a regular expression.
	/// </summary>
	/// <param name="email">The email address string to validate. If valid, it's not null.</param>
	/// <returns><see langword="true"/> if the string is a non-null, non-whitespace, validly formatted email address; otherwise, <see langword="false"/>.</returns>
	/// <remarks>
	/// Returns <see langword="false"/> for null or whitespace strings.
	/// </remarks>
	public static bool ValidateEmail([NotNullWhen(true)] string? email)
	{
		if (string.IsNullOrWhiteSpace(email))
			return false;

		return _emailRegex.IsMatch(email);
	}

	/// <summary>
	/// Validates if a password meets the criteria specified in <see cref="PasswordOptions"/>.
	/// </summary>
	/// <param name="password">The password string to validate. If valid, it's not null.</param>
	/// <param name="passwordOptions">The <see cref="PasswordOptions"/> defining the policy (e.g., length, required characters).</param>
	/// <returns><see langword="true"/> if the password meets all enabled policy requirements; otherwise, <see langword="false"/>.</returns>
	/// <remarks>
	/// Checks for length, unique characters, and presence of digits, lowercase, uppercase, and non-alphanumeric characters as configured in <paramref name="passwordOptions"/>.
	/// Returns <see langword="false"/> for null or whitespace passwords.
	/// </remarks>
	public static bool ValidatePassword([NotNullWhen(true)] string? password, PasswordOptions passwordOptions)
	{
		if (string.IsNullOrWhiteSpace(password))
			return false;

		if (passwordOptions.RequiredLength > 0 && password.Length < passwordOptions.RequiredLength)
			return false;

		if (passwordOptions.RequiredUniqueChars > 0 &&
			new string([.. password.Distinct()]).Length < passwordOptions.RequiredUniqueChars)
			return false;

		bool hasDigit = false;
		bool hasLower = false;
		bool hasUpper = false;
		bool hasNonAlphanumeric = false;

		foreach (char c in password)
		{
			if (char.IsDigit(c)) hasDigit = true;
			else if (char.IsLower(c)) hasLower = true;
			else if (char.IsUpper(c)) hasUpper = true;
			else if (!char.IsLetterOrDigit(c)) hasNonAlphanumeric = true;

			if ((!passwordOptions.RequireDigit || hasDigit) &&
				(!passwordOptions.RequireLowercase || hasLower) &&
				(!passwordOptions.RequireUppercase || hasUpper) &&
				(!passwordOptions.RequireNonAlphanumeric || hasNonAlphanumeric))
				break;
		}

		if (passwordOptions.RequireDigit && !hasDigit)
			return false;

		if (passwordOptions.RequireLowercase && !hasLower)
			return false;

		if (passwordOptions.RequireUppercase && !hasUpper)
			return false;

		if (passwordOptions.RequireNonAlphanumeric && !hasNonAlphanumeric)
			return false;

		return true;
	}
}