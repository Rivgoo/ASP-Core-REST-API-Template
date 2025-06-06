using Application.Results;
using Domain.Abstractions;

namespace Application.Filters.Abstractions;

/// <summary>
/// Provides an abstract base class for filter criteria, managing pagination and ordering for entities of type <typeparamref name="TEntity"/>.
/// </summary>
/// <typeparam name="TEntity">The type of the entity to which the filter applies. Must implement <see cref="IEntity"/>.</typeparam>
/// <remarks>
/// This class implements <see cref="IFilter"/> and offers common functionality for derived filter classes,
/// including page index management and methods for adding sort orders.
/// It also provides static utility methods for validating search terms and lists.
/// </remarks>
public abstract class BaseFilter<TEntity> : IFilter where TEntity : IEntity
{
	/// <summary>
	/// Gets or sets the current page index for pagination.
	/// </summary>
	/// <value>The 1-based page number. Defaults to 1.</value>
	public int PageIndex { get; set; } = 1;

	private List<QueryableOrder> _orders = [];

	/// <summary>
	/// Adds an ordering directive to the filter.
	/// </summary>
	/// <param name="orderType">The type of ordering (e.g., OrderBy, ThenByDescending).</param>
	/// <param name="propertyName">The name of the property on <typeparamref name="TEntity"/> to order by. Case-sensitive.</param>
	/// <returns>
	/// A <see cref="Result"/>. <see cref="Result.Ok()"/> if successful.
	/// <see cref="Result.Bad(Error)"/> if <paramref name="propertyName"/> is invalid for <typeparamref name="TEntity"/>,
	/// if the property is already added, or if <paramref name="orderType"/> is a 'ThenBy' type without a preceding 'OrderBy'.
	/// </returns>
	/// <seealso cref="QueryableOrderType"/>
	/// <seealso cref="FilterErrors"/>
	public Result AddOrdering(QueryableOrderType orderType, string propertyName)
	{
		if (typeof(TEntity).GetProperty(propertyName) == null)
			return Result.Bad(FilterErrors.InvalidOrderField(propertyName));

		if (_orders.Any(x => x.PropertyName == propertyName))
			return Result.Bad(FilterErrors.OrderFieldAlreadyExists(propertyName));

		if ((orderType == QueryableOrderType.ThenBy || orderType == QueryableOrderType.ThenByDescending) && _orders.Count == 0)
			return Result.Bad(FilterErrors.InvalidSortOrder);

		_orders.Add(new QueryableOrder(propertyName, orderType));

		return Result.Ok();
	}

	/// <summary>
	/// Retrieves the list of currently applied ordering directives.
	/// </summary>
	/// <returns>A <see cref="List{T}"/> of <see cref="QueryableOrder"/> objects specifying the sorting criteria.</returns>
	public List<QueryableOrder> GetOrders() => _orders;

	/// <summary>
	/// Validates and sanitizes a search term by trimming whitespace and replacing '>' with '&gt;'.
	/// </summary>
	/// <param name="searchTerm">The search term to validate and sanitize.</param>
	/// <returns>The sanitized search term, or <see cref="string.Empty"/> if the input is null or empty.</returns>
	public static string ValidateSearchTerms(string? searchTerm)
		=> string.IsNullOrEmpty(searchTerm) ? string.Empty : searchTerm.Trim().Replace(">", "&gt;");

	/// <summary>
	/// Validates, sanitizes, and applies length constraints to a search term.
	/// </summary>
	/// <param name="searchTerm">The search term to process.</param>
	/// <param name="minLength">The minimum allowed length for the search term after sanitization.</param>
	/// <param name="maxLength">The maximum allowed length for the search term after sanitization. If longer, it will be truncated.</param>
	/// <returns>
	/// The validated, sanitized, and length-adjusted search term.
	/// Returns <see cref="string.Empty"/> if the sanitized term is shorter than <paramref name="minLength"/> or if the initial term was null/empty.
	/// </returns>
	public static string ValidateSearchTerms(string? searchTerm, int minLength, int maxLength)
	{
		searchTerm = ValidateSearchTerms(searchTerm);

		if (searchTerm.Length < minLength)
			return string.Empty;

		if (searchTerm.Length > maxLength)
			return searchTerm[..maxLength];

		return searchTerm;
	}

	/// <summary>
	/// Validates a list of strings by filtering out null or empty entries.
	/// </summary>
	/// <param name="list">The list of strings to validate.</param>
	/// <returns>A new <see cref="List{T}"/> containing only the non-null and non-empty strings from the input list.</returns>
	public static List<string> ValidateList(List<string> list)
		=> [.. list.Where(x => !string.IsNullOrEmpty(x))];

	/// <summary>
	/// Validates a list of integers by filtering out entries that are less than zero.
	/// </summary>
	/// <param name="list">The list of integers to validate.</param>
	/// <returns>A new <see cref="List{T}"/> containing only the non-negative integers (>= 0) from the input list.</returns>
	public static List<int> ValidateList(List<int> list)
		=> [.. list.Where(x => x >= 0)];
}