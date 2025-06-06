using System.ComponentModel.DataAnnotations;

namespace Web.API.Controllers.V1.Authentications;

/// <summary>
/// Represents the request payload for user authentication.
/// </summary>
/// <remarks>
/// This Data Transfer Object (DTO) is used by clients to send user credentials (email and password)
/// to an authentication endpoint for login.
/// </remarks>
public class AuthenticationRequest
{
	/// <summary>
	/// Gets or sets the user's email address.
	/// </summary>
	/// <value>The email address used for authentication. This field is required and has a maximum length of 255 characters.</value>
	[Required]
	[MaxLength(255)]
	[EmailAddress]
	public string Email { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the user's password.
	/// </summary>
	/// <value>The password for the user account. This field is required and has a maximum length of 255 characters.</value>
	/// <remarks>
	/// The actual password complexity rules are enforced by the authentication service and Identity options,
	/// not by attributes on this DTO. MaxLength is a general input sanitation measure.
	/// </remarks>
	[Required]
	[MaxLength(255)]
	public string Password { get; set; } = string.Empty;
}