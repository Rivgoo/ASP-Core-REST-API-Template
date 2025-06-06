using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

/// <summary>
/// Represents a role within the application, extending the ASP.NET Core Identity <see cref="IdentityRole"/>.
/// </summary>
public class Role : IdentityRole
{
	/// <summary>
	/// Initializes a new instance of the <see cref="Role"/> class.
	/// </summary>
	/// <remarks>This constructor is typically used by ORM frameworks like Entity Framework Core.</remarks>
	public Role() : base() { }

	/// <summary>
	/// Initializes a new instance of the <see cref="Role"/> class with the specified role name.
	/// </summary>
	/// <param name="roleName">The name of the role.</param>
	/// <remarks>The <see cref="IdentityRole{TKey}.NormalizedName"/> property is set to the uppercase version of <paramref name="roleName"/>.</remarks>
	public Role(string roleName) : base(roleName)
	{
		NormalizedName = roleName.ToUpperInvariant();
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="Role"/> class with the specified ID and role name.
	/// </summary>
	/// <param name="id">The unique identifier for the role.</param>
	/// <param name="roleName">The name of the role.</param>
	/// <remarks>
	/// This constructor is useful for seeding roles with predefined identifiers.
	/// The <see cref="IdentityRole{TKey}.Id"/> property is set to the provided <paramref name="id"/>.
	/// The <see cref="IdentityRole{TKey}.NormalizedName"/> property is set to the uppercase version of <paramref name="roleName"/>.
	/// </remarks>
	public Role(string id, string roleName) : base(roleName)
	{
		Id = id;
		NormalizedName = roleName.ToUpperInvariant();
	}
}