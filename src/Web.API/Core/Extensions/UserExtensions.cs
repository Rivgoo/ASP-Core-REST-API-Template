using Domain;
using System.Security.Claims;

namespace Web.API.Core.Extensions;

public static class UserExtensions
{
	/// <summary>
	/// Gets the entity ID of the user.
	/// </summary>
	/// <returns>The entity ID of the user, or an empty string if not found.</returns>
	public static string GetEntityId(this ClaimsPrincipal user)
		=> user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

	/// <summary>
	/// Checks if the user is an Admin.
	/// </summary>
	/// <param name="user">The user to check.</param>
	/// <returns>True if the user is an Admin, otherwise false.</returns>
	public static bool IsAdmin(this ClaimsPrincipal user)
		=> user.IsInRole(RoleList.Admin);

	/// <summary>
	/// Checks if the user is an Admin.
	/// </summary>
	/// <param name="user">The user to check.</param>
	/// <returns>True if the user is an Admin, otherwise false.</returns>
	public static bool IsCustomer(this ClaimsPrincipal user)
		=> user.IsInRole(RoleList.Customer);
}