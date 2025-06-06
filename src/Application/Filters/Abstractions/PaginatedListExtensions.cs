namespace Application.Filters.Abstractions;

/// <summary>
/// Provides extension methods for <see cref="IEnumerable{T}"/> to facilitate the creation of <see cref="PaginatedList{TResult}"/>.
/// </summary>
public static class PaginatedListExtensions
{
	/// <summary>
	/// Converts an <see cref="IEnumerable{T}"/> source to a <see cref="PaginatedList{TResult}"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of elements in the source enumerable and the resulting paginated list.</typeparam>
	/// <param name="source">The <see cref="IEnumerable{T}"/> to convert. This parameter is extended.</param>
	/// <returns>
	/// A <see cref="PaginatedList{TResult}"/> containing all items from the source.
	/// The <see cref="PaginatedList{TResult}.PageIndex"/> and <see cref="PaginatedList{TResult}.PageSize"/> will be default values (typically 1 and 10, or 1 if the source is empty).
	/// The <see cref="PaginatedList{TResult}.TotalCount"/> will not be set and will default to 0.
	/// </returns>
	/// <remarks>
	/// This method is a convenience for creating a paginated list where all items are considered to be on a single page.
	/// For true pagination from a large data source, use <see cref="PaginatedList{TResult}.CreateAsync"/> with an <see cref="IQueryable{T}"/>
	/// and set the total count using <see cref="PaginatedList{TResult}.SetTotalCount(int)"/> after counting the source.
	/// </remarks>
	/// <seealso cref="PaginatedList{TResult}.FromList(List{TResult}, int, int)"/>
	public static PaginatedList<TResult> ToPaginatedListAsync<TResult>(this IEnumerable<TResult> source)
		=> PaginatedList<TResult>.FromList([.. source]);
}