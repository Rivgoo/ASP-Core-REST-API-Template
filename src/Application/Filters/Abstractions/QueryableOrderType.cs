namespace Application.Filters.Abstractions;

/// <summary>
/// Specifies the type of ordering operation to be applied to a queryable collection.
/// </summary>
/// <remarks>
/// This enumeration defines the standard LINQ ordering methods. While marked with <see cref="FlagsAttribute"/>,
/// in typical usage for query building, these values are applied sequentially rather than combined as bit flags for a single operation.
/// For example, a query might first use <see cref="OrderBy"/> and then <see cref="ThenByDescending"/>.
/// </remarks>
[Flags]
public enum QueryableOrderType
{
	/// <summary>
	/// Specifies an initial sort in ascending order. Corresponds to LINQ's <c>OrderBy()</c>.
	/// </summary>
	OrderBy,

	/// <summary>
	/// Specifies an initial sort in descending order. Corresponds to LINQ's <c>OrderByDescending()</c>.
	/// </summary>
	OrderByDescending,

	/// <summary>
	/// Specifies a subsequent sort in ascending order, applied after a primary sort. Corresponds to LINQ's <c>ThenBy()</c>.
	/// </summary>
	ThenBy,

	/// <summary>
	/// Specifies a subsequent sort in descending order, applied after a primary sort. Corresponds to LINQ's <c>ThenByDescending()</c>.
	/// </summary>
	ThenByDescending
}