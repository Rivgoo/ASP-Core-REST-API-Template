namespace Web.API.Core.Responses;

/// <summary>
/// Represents a generic API response that conveys a status using an enumeration type.
/// </summary>
/// <typeparam name="TStatusEnum">The enumeration type that defines the possible status values. Must be an <see cref="System.Enum"/>.</typeparam>
/// <remarks>
/// This class is useful for returning a simple status indication from an API endpoint,
/// where the status is well-defined by an enumeration.
/// </remarks>
/// <param name="status">The initial status value of type <typeparamref name="TStatusEnum"/>.</param>
public class StatusResponse<TStatusEnum>(TStatusEnum status) where TStatusEnum : Enum
{
	/// <summary>
	/// Gets or sets the status value.
	/// </summary>
	/// <value>The current status, represented by an instance of the <typeparamref name="TStatusEnum"/> enumeration.</value>
	public TStatusEnum Status { get; set; } = status;
}