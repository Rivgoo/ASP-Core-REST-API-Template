using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Represents the link between a user and a role, extending <see cref="IdentityUserRole{TKey}"/> and implementing <see cref="IAuditableEntity"/>.
/// </summary>
public class UserRole : IdentityUserRole<string>, IAuditableEntity
{
	/// <summary>
	/// Gets or sets the UTC creation timestamp for this user-role assignment.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when the user was assigned to the role. This is a required field.</value>
	[Required]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the UTC last modification timestamp for this user-role assignment.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when this user-role assignment record was last updated. This is a required field.</value>
	[Required]
	public DateTime UpdatedAt { get; set; }
}