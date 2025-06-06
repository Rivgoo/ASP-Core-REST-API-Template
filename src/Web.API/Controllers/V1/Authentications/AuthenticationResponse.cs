namespace Web.API.Controllers.V1.Authentications;

/// <summary>
/// Represents the response payload for a successful user authentication request.
/// </summary>
/// <remarks>
/// This Data Transfer Object (DTO) is returned by authentication endpoints upon successful login,
/// containing the access token required for subsequent authorized API calls.
/// </remarks>
public class AuthenticationResponse
{
	/// <summary>
	/// Gets or sets the JSON Web Token (JWT) access token issued upon successful authentication.
	/// </summary>
	/// <value>
	/// A string representing the JWT access token. This token should be included in the
	/// Authorization header of subsequent requests to protected API endpoints (e.g., as a Bearer token).
	/// Defaults to <see cref="string.Empty"/> if not set.
	/// </value>
	public string AccessToken { get; set; } = string.Empty;
}