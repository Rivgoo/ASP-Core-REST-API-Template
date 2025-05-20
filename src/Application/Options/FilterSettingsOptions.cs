namespace Application.Options;

/// <summary>
/// Configuration options for filtering and pagination.
/// </summary>
public class FilterSettingsOptions
{
	public const string SectionName = "FilterSettings";

	/// <summary>
	/// Default page size to be used if no specific page size is requested.
	/// Defaults to 10.
	/// </summary>
	public int DefaultPageSize { get; set; } = 10;

	/// <summary>
	/// Maximum allowed page size to prevent excessive data retrieval.
	/// Defaults to 1000.
	/// </summary>
	public int MaxPageSize { get; set; } = 1000;
}