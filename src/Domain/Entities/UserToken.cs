using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Represents an authentication token for a user, extending <see cref="IdentityUserToken{TKey}"/> and implementing <see cref="IAuditableEntity"/>.
/// </summary>
public class UserToken : IdentityUserToken<string>, IAuditableEntity
{
	/// <summary>
	/// Gets or sets the UTC creation timestamp for this user token.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when the token was created. This is a required field.</value>
	[Required]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the UTC last modification timestamp for this user token.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when the token record was last updated. This is a required field.</value>
	[Required]
	public DateTime UpdatedAt { get; set; }
}