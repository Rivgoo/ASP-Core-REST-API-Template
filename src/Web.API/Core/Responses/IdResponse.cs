namespace Web.API.Core.BaseResponses;

/// <summary>
/// Represents a generic response containing an ID.
/// </summary>
/// <typeparam name="TIdType">The type of the identifier.</typeparam>
/// <param name="id">The identifier value.</param>
public class IdResponse<TIdType>(TIdType id)
{
	/// <summary>
	/// The ID value.
	/// </summary>
	public TIdType Id { get; set; } = id;
}