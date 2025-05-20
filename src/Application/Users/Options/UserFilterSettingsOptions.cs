namespace Application.Users.Options;

/// <summary>
/// Configuration options for default settings of the UserFilter.
/// </summary>
public class UserFilterSettingsOptions
{
	public const string SectionName = "UserFilterSettings";

	/// <summary>
	/// Default field to sort users by if no specific order is requested.
	/// Defaults to "CreatedAt".
	/// </summary>
	public string DefaultSortField { get; set; } = "CreatedAt";

	/// <summary>
	/// Default sort order (e.g., "OrderBy", "OrderByDescending") if no specific order is requested.
	/// Defaults to "OrderByDescending".
	/// </summary>
	public string DefaultSortOrder { get; set; } = "OrderByDescending";
}