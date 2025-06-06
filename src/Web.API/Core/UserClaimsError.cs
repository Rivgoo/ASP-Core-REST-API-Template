using Application.Results;

namespace Web.API.Core;

public class UserClaimsError
{
	public const string BaseKey = "UserClaims";

	public static Error IdNotFoundInClaims()
		=> Error.Unauthorized($"{BaseKey}.{nameof(IdNotFoundInClaims)}", "The user id was not found in the user's claims. This is required to identify the user.");
}