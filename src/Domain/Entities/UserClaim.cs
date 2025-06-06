using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Represents a claim associated with a user, extending <see cref="IdentityUserClaim{TKey}"/> and implementing <see cref="IBaseEntity{TId}"/> for auditing.
/// </summary>
public class UserClaim : IdentityUserClaim<string>, IBaseEntity<int>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="UserClaim"/> class.
	/// </summary>
	/// <remarks>This constructor is typically used by ORM frameworks like Entity Framework Core.</remarks>
	public UserClaim() : base() { }

	/// <summary>
	/// Gets or sets the UTC creation timestamp for this user claim.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when the user claim was created. This is a required field.</value>
	[Required]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the UTC last modification timestamp for this user claim.
	/// </summary>
	/// <value>The <see cref="DateTime"/> when the user claim was last updated. This is a required field.</value>
	[Required]
	public DateTime UpdatedAt { get; set; }
}