using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Represents a claim associated with a role, extending <see cref="IdentityRoleClaim{TKey}"/> and implementing <see cref="IBaseEntity{TId}"/> for auditing.
/// </summary>
public class RoleClaim : IdentityRoleClaim<string>, IBaseEntity<int>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="RoleClaim"/> class.
	/// </summary>
	/// <remarks>This constructor is typically used by ORM frameworks like Entity Framework Core.</remarks>
	public RoleClaim() : base() { }

	/// <summary>
	/// Gets or sets the UTC creation timestamp for this role claim.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when the role claim was created. This is a required field.</value>
	[Required]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the UTC last modification timestamp for this role claim.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when the role claim was last updated. This is a required field.</value>
	[Required]
	public DateTime UpdatedAt { get; set; }
}