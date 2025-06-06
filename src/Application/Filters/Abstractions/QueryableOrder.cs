using System.ComponentModel.DataAnnotations;

namespace Application.Filters.Abstractions;

/// <summary>
/// Represents a single ordering directive (property name and order type) to be applied to a queryable collection.
/// </summary>
/// <param name="propertyName">The name of the property on the entity to order by. This is required.</param>
/// <param name="orderType">The type of ordering to apply (e.g., Ascending, Descending). This is required.</param>
/// <remarks>
/// This struct is typically used in a list to define multi-level sorting for queries.
/// The <see cref="PropertyName"/> should correspond to a valid, sortable property on the target entity type.
/// </remarks>
public struct QueryableOrder(string propertyName, QueryableOrderType orderType)
{
	/// <summary>
	/// Gets or sets the name of the property to use for ordering.
	/// </summary>
	/// <value>The name of the entity property. This field is required.</value>
	[Required]
	public string PropertyName { get; set; } = propertyName;

	/// <summary>
	/// Gets or sets the ordering type (e.g., OrderBy, OrderByDescending, ThenBy, ThenByDescending).
	/// </summary>
	/// <value>The <see cref="QueryableOrderType"/> specifying the direction and sequence of sorting. This field is required.</value>
	[Required]
	public QueryableOrderType OrderType { get; set; } = orderType;
}