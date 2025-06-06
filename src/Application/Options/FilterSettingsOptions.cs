namespace Application.Options;

/// <summary>
/// Defines configuration options related to default filtering and pagination behavior within the application.
/// </summary>
/// <remarks>
/// These options are typically bound from application configuration (e.g., appsettings.json)
/// and used by filter services to apply consistent pagination limits.
/// The <see cref="SectionName"/> constant indicates the expected configuration section name.
/// </remarks>
public class FilterSettingsOptions
{
	/// <summary>
	/// The name of the configuration section for these options.
	/// </summary>
	/// <value>"FilterSettings"</value>
	public const string SectionName = "FilterSettings";

	/// <summary>
	/// Gets or sets the default page size to be used for pagination when no specific page size is requested by the client.
	/// </summary>
	/// <value>The default number of items per page. Defaults to 10 if not configured.</value>
	public int DefaultPageSize { get; set; } = 10;

	/// <summary>
	/// Gets or sets the maximum allowed page size for pagination requests.
	/// </summary>
	/// <value>The maximum number of items that can be requested in a single page. Defaults to 1000 if not configured. This helps prevent excessive data retrieval.</value>
	public int MaxPageSize { get; set; } = 1000;
}