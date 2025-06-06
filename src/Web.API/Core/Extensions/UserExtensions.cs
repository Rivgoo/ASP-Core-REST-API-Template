using Domain;
using System.Security.Claims;

namespace Web.API.Core.Extensions;

/// <summary>
/// Provides extension methods for the <see cref="System.Security.Claims.ClaimsPrincipal"/> class,
/// offering convenient ways to access common user-related information from claims.
/// </summary>
public static class UserExtensions
{
	/// <summary>
	/// Gets the entity identifier (typically the user ID) of the currently authenticated user from their claims.
	/// </summary>
	/// <param name="user">The <see cref="System.Security.Claims.ClaimsPrincipal"/> representing the current user. This parameter is extended.</param>
	/// <returns>
	/// The entity ID (user ID) as a string if the <see cref="System.Security.Claims.ClaimTypes.NameIdentifier"/> claim is present;
	/// otherwise, returns <see cref="string.Empty"/>.
	/// </returns>
	public static string GetEntityId(this ClaimsPrincipal user)
		=> user.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

	/// <summary>
	/// Checks if the currently authenticated user is assigned the 'Admin' role.
	/// </summary>
	/// <param name="user">The <see cref="System.Security.Claims.ClaimsPrincipal"/> representing the current user. This parameter is extended.</param>
	/// <returns>
	/// <see langword="true"/> if the user has the 'Admin' role (as defined in <see cref="Domain.RoleNames.Admin"/>);
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <seealso cref="Domain.RoleNames.Admin"/>
	public static bool IsAdmin(this ClaimsPrincipal user)
		=> user.IsInRole(RoleNames.Admin);

	/// <summary>
	/// Checks if the currently authenticated user is assigned the 'Customer' role.
	/// </summary>
	/// <param name="user">The <see cref="System.Security.Claims.ClaimsPrincipal"/> representing the current user. This parameter is extended.</param>
	/// <returns>
	/// <see langword="true"/> if the user has the 'Customer' role (as defined in <see cref="Domain.RoleNames.Customer"/>);
	/// otherwise, <see langword="false"/>.
	/// </returns>
	/// <seealso cref="Domain.RoleNames.Customer"/>
	public static bool IsCustomer(this ClaimsPrincipal user)
		=> user.IsInRole(RoleNames.Customer);
}