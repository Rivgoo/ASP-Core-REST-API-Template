namespace Application.Options;

/// <summary>
/// Configuration options for phone number validation.
/// </summary>
public class PhoneNumberValidationOptions
{
	public const string SectionName = "PhoneNumberValidation";

	/// <summary>
	/// Minimum number of digits required for a valid phone number.
	/// Defaults to 7.
	/// </summary>
	public int MinDigits { get; set; } = 7;

	/// <summary>
	/// Maximum number of digits allowed for a valid phone number.
	/// Defaults to 15.
	/// </summary>
	public int MaxDigits { get; set; } = 15;
}