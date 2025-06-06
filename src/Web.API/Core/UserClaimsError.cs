using Application.Results;

namespace Web.API.Core;

/// <summary>
/// Provides static definitions for errors related to user claims processing, specifically for API responses.
/// </summary>
/// <remarks>
/// This class centralizes error objects that are returned when issues arise with accessing or interpreting user claims
/// from an authentication token (e.g., JWT).
/// </remarks>
public static class UserClaimsError
{
	/// <summary>
	/// Defines the base key prefix used in error codes for user claims errors.
	/// </summary>
	/// <value>"UserClaims"</value>
	public const string BaseKey = "UserClaims";

	/// <summary>
	/// Creates an error indicating that the user's unique identifier (ID) was not found in their claims.
	/// </summary>
	/// <returns>An <see cref="Error"/> object with <see cref="ErrorType.AccessUnAuthorized"/>, as this typically prevents identifying the user for an authorized action.</returns>
	/// <remarks>
	/// This error is critical because the user ID claim is often essential for identifying the user
	/// to perform operations on their behalf or to fetch their specific data.
	/// </remarks>
	public static Error IdNotFoundInClaims()
		=> Error.Unauthorized($"{BaseKey}.{nameof(IdNotFoundInClaims)}", "The user id was not found in the user's claims. This is required to identify the user.");
}