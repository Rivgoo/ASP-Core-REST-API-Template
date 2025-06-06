using Domain.Abstractions;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

/// <summary>
/// Represents an application user, extending <see cref="IdentityUser"/> with custom properties and implementing <see cref="IBaseEntity{TId}"/> for auditing.
/// </summary>
public class User : IdentityUser, IBaseEntity<string>
{
	/// <summary>
	/// Initializes a new instance of the <see cref="User"/> class.
	/// </summary>
	/// <remarks>This constructor is typically used by ORM frameworks like Entity Framework Core.</remarks>
	public User() : base() { }

	/// <summary>
	///	Gets or sets the first name of the user.
	/// </summary>
	/// <value>The user's first name. This field is required and has a maximum length of 255 characters.</value>
	[Required]
	[MaxLength(255)]
	public string FirstName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the last name of the user.
	/// </summary>
	/// <value>The user's last name. This field is required and has a maximum length of 255 characters.</value>
	[Required]
	[MaxLength(255)]
	public string LastName { get; set; } = string.Empty;

	/// <summary>
	/// Gets or sets the date and time (UTC) when the user account was created.
	/// </summary>
	/// <value>The <see cref="DateTime"/> of creation. This is a required field, typically set automatically.</value>
	[Required]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the date and time (UTC) when the user account was last updated.
	/// </summary>
	/// <value>The <see cref="DateTime"/> of the last update. This is a required field, typically set automatically.</value>
	[Required]
	public DateTime UpdatedAt { get; set; }

	/// <summary>
	/// Gets or sets the date and time (UTC) when the user last logged in.
	/// </summary>
	/// <value>A nullable <see cref="DateTime"/> representing the last login. Null if the user has never logged in.</value>
	public DateTime? LastLoginAt { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the user account is blocked.
	/// </summary>
	/// <value><see langword="true"/> if the user account is blocked; otherwise, <see langword="false"/>. This is a required field.</value>
	[Required]
	public bool IsBlocked { get; set; }
}