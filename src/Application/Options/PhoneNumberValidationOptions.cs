namespace Application.Options;

/// <summary>
/// Defines configuration options for validating phone numbers.
/// </summary>
/// <remarks>
/// These options are typically bound from application configuration (e.g., appsettings.json under the section defined by <see cref="SectionName"/>)
/// and used by validation logic to ensure phone numbers meet specified digit length criteria.
/// </remarks>
public class PhoneNumberValidationOptions
{
	/// <summary>
	/// The name of the configuration section for these options.
	/// </summary>
	/// <value>"PhoneNumberValidation"</value>
	public const string SectionName = "PhoneNumberValidation";

	/// <summary>
	/// Gets or sets the minimum number of digits required for a phone number to be considered valid after non-digit characters are removed.
	/// </summary>
	/// <value>The minimum required digit count. Defaults to 7 if not configured.</value>
	public int MinDigits { get; set; } = 7;

	/// <summary>
	/// Gets or sets the maximum number of digits allowed for a phone number after non-digit characters are removed.
	/// </summary>
	/// <value>The maximum allowed digit count. Defaults to 15 if not configured.</value>
	public int MaxDigits { get; set; } = 15;
}