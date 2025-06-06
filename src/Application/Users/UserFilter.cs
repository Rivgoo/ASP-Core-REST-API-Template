using Application.Filters.Abstractions;
using Domain.Entities;

namespace Application.Users;

/// <summary>
/// Represents filtering criteria specific to querying <see cref="User"/> entities.
/// </summary>
/// <remarks>
/// This class extends <see cref="BaseFilter{TEntity}"/> for the <see cref="User"/> type,
/// providing properties to filter users by various attributes such as name, email, account status,
/// creation date, login status, and role.
/// </remarks>
public class UserFilter : BaseFilter<User>
{
	/// <summary>
	/// Gets or sets a term to filter users by their first name.
	/// The match is typically case-insensitive and partial (e.g., "contains").
	/// </summary>
	public string? FirstName { get; set; }

	/// <summary>
	/// Gets or sets a term to filter users by their last name.
	/// The match is typically case-insensitive and partial.
	/// </summary>
	public string? LastName { get; set; }

	/// <summary>
	/// Gets or sets a term to filter users by their email address.
	/// The match is typically case-insensitive and partial.
	/// </summary>
	public string? Email { get; set; }

	/// <summary>
	/// Gets or sets a term to filter users by their username.
	/// The match is typically case-insensitive and partial.
	/// </summary>
	public string? UserName { get; set; }

	/// <summary>
	/// Gets or sets a term to filter users by their phone number.
	/// The match is typically case-insensitive and partial.
	/// </summary>
	public string? PhoneNumber { get; set; }

	/// <summary>
	/// Gets or sets a filter for user account block status.
	/// If <see langword="true"/>, returns only blocked users.
	/// If <see langword="false"/>, returns only non-blocked users.
	/// If <see langword="null"/>, this filter is not applied.
	/// </summary>
	public bool? IsBlocked { get; set; }

	/// <summary>
	/// Gets or sets a filter for user email confirmation status.
	/// If <see langword="true"/>, returns only users with confirmed emails.
	/// If <see langword="false"/>, returns only users with non-confirmed emails.
	/// If <see langword="null"/>, this filter is not applied.
	/// </summary>
	public bool? EmailConfirmed { get; set; }

	/// <summary>
	/// Gets or sets the minimum creation date (inclusive) to filter users by.
	/// If set, only users created on or after this date/time will be returned.
	/// </summary>
	public DateTime? MinCreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the maximum creation date (inclusive) to filter users by.
	/// If set, only users created on or before this date/time will be returned.
	/// </summary>
	/// <remarks>
	/// When implementing, care should be taken with the time component. For example,
	/// to include all users created on a specific day, this might be set to the end of that day.
	/// The <see cref="UserSorter"/> implementation adds one day to this value and uses a less than comparison
	/// to effectively include the entire MaxCreatedAt day.
	/// </remarks>
	public DateTime? MaxCreatedAt { get; set; }

	/// <summary>
	/// Gets or sets a filter based on whether a user has ever logged in (i.e., <see cref="User.LastLoginAt"/> is not null).
	/// If <see langword="true"/>, returns users who have a last login date.
	/// If <see langword="false"/>, returns users who have never logged in (last login date is null).
	/// If <see langword="null"/>, this filter is not applied.
	/// </summary>
	public bool? HasLastLogin { get; set; }

	/// <summary>
	/// Gets or sets the ID of a specific role to filter users by.
	/// If set, only users assigned to this role will be returned.
	/// </summary>
	/// <remarks>
	/// The comparison is typically done against the <see cref="Role.Id"/> or <see cref="Role.NormalizedName"/>
	/// associated with the user's roles.
	/// The <see cref="UserSorter"/> implementation uses the RoleId directly.
	/// </remarks>
	public string? RoleId { get; set; }
}