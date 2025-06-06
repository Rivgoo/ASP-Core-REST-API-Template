using Application.Results;

namespace Application.Filters.Abstractions;

/// <summary>
/// Defines a contract for filter criteria applied to a query.
/// </summary>
/// <remarks>
/// Implementations of this interface hold filter parameters, including pagination and ordering information,
/// used to refine data retrieval operations.
/// </remarks>
public interface IFilter
{
	/// <summary>
	/// Gets or sets the current page index for pagination.
	/// </summary>
	/// <value>The page number, typically 1-based.</value>
	int PageIndex { get; set; }

	/// <summary>
	/// Retrieves the list of ordering directives currently applied to the filter.
	/// </summary>
	/// <returns>A <see cref="List{T}"/> of <see cref="QueryableOrder"/> objects specifying the sorting criteria.</returns>
	List<QueryableOrder> GetOrders();

	/// <summary>
	/// Adds an ordering directive to the filter.
	/// </summary>
	/// <param name="orderType">The type of ordering to apply (e.g., OrderBy, ThenByDescending).</param>
	/// <param name="propertyName">The name of the property to order by. This property must exist on the target entity.</param>
	/// <returns>
	/// A <see cref="Result"/> indicating the outcome of the operation.
	/// <see cref="Result.Ok()"/> if the ordering was added successfully.
	/// <see cref="Result.Bad(Error)"/> if <paramref name="propertyName"/> is invalid, already exists, or if the <paramref name="orderType"/> is logically incorrect (e.g., ThenBy without a preceding OrderBy).
	/// </returns>
	/// <seealso cref="QueryableOrderType"/>
	/// <seealso cref="FilterErrors"/>
	Result AddOrdering(QueryableOrderType orderType, string propertyName);
}