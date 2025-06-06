namespace Web.API.Core.Options;

/// <summary>
/// Defines configuration options for creating an initial administrator user account during application startup.
/// </summary>
/// <remarks>
/// These options are typically bound from application configuration (e.g., appsettings.json under a section like "InitialAdmin").
/// If an email and password are provided, the application may attempt to create this admin user if one doesn't already exist with the specified email.
/// </remarks>
public class InitialAdminOptions
{
	/// <summary>
	/// Gets or sets the first name for the initial administrator user.
	/// </summary>
	/// <value>The first name string. Defaults to <see cref="string.Empty"/>.</value>
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the last name for the initial administrator user.
	/// </summary>
	/// <value>The last name string. Defaults to <see cref="string.Empty"/>.</value>
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the email address for the initial administrator user. This is often used as the username.
	/// </summary>
	/// <value>The email address string. Defaults to <see cref="string.Empty"/>.</value>
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the password for the initial administrator user.
	/// </summary>
	/// <value>The password string. Defaults to <see cref="string.Empty"/>.</value>
	/// <remarks>Ensure this password meets the application's configured password policies.</remarks>
	public string Password { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the phone number for the initial administrator user.
	/// </summary>
	/// <value>The phone number string. Defaults to <see cref="string.Empty"/>.</value>
	public string PhoneNumber { get; set; } = string.Empty;
}