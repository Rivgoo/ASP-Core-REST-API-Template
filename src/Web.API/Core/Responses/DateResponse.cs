namespace Web.API.Core.Responses;

/// <summary>
/// Represents a generic API response containing a single date and time value.
/// </summary>
/// <remarks>
/// This class can be used for API endpoints that need to return a specific <see cref="System.DateTime"/> value as their primary response data.
/// </remarks>
/// <param name="date">The initial <see cref="System.DateTime"/> value for the response.</param>
public class DateResponse(DateTime date)
{
	/// <summary>
	/// Gets or sets the date and time value.
	/// </summary>
	/// <value>The <see cref="System.DateTime"/> object representing the date and time in the response.</value>
	public DateTime Date { get; set; } = date;
}