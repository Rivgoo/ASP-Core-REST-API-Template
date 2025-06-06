using Microsoft.EntityFrameworkCore;

namespace Application.Filters.Abstractions;

/// <summary>
/// Represents a single page of query results, including items and pagination metadata.
/// </summary>
/// <typeparam name="TResult">The type of items contained in the paginated list.</typeparam>
public class PaginatedList<TResult>
{
	/// <summary>
	/// Gets the list of items for the current page.
	/// </summary>
	public List<TResult> Items { get; private set; }

	/// <summary>
	/// Gets the current page index (1-based).
	/// </summary>
	public int PageIndex { get; private set; }

	/// <summary>
	/// Gets the number of items per page.
	/// </summary>
	public int PageSize { get; private set; }

	/// <summary>
	/// Gets the total number of items across all pages in the original source.
	/// </summary>
	public int TotalCount { get; private set; }

	/// <summary>
	/// Gets the total number of pages based on <see cref="TotalCount"/> and <see cref="PageSize"/>.
	/// </summary>
	public int TotalPages => (int)Math.Ceiling(TotalCount / (float)PageSize);

	/// <summary>
	/// Gets a value indicating whether there is a previous page.
	/// </summary>
	public bool HasPreviousPage => PageIndex > 1 && TotalPages > 1;

	/// <summary>
	/// Gets a value indicating whether there is a next page.
	/// </summary>
	public bool HasNextPage => PageIndex < TotalPages;

	/// <summary>
	/// Gets an empty paginated list.
	/// </summary>
	/// <value>A <see cref="PaginatedList{TResult}"/> with no items, page index 1, and page size 1.</value>
	public static PaginatedList<TResult> Empty => new([], 1, 1);

	/// <summary>
	/// Initializes a new instance of the <see cref="PaginatedList{TResult}"/> class.
	/// </summary>
	/// <param name="items">The collection of items for the current page.</param>
	/// <param name="pageIndex">The current page index (1-based). Defaults to 1.</param>
	/// <param name="pageSize">The number of items per page. Defaults to 10. If less than or equal to 0, it will be set to 1.</param>
	public PaginatedList(List<TResult> items, int pageIndex = 1, int pageSize = 10)
	{
		if (pageSize <= 0)
			pageSize = 1; // Ensure pageSize is at least 1 to prevent division by zero or negative.

		PageIndex = pageIndex;
		PageSize = pageSize;
		Items = items;
	}

	/// <summary>
	/// Sets the total count of items in the original source and returns the current instance.
	/// </summary>
	/// <param name="totalCount">The total number of items available in the data source before pagination.</param>
	/// <returns>The current <see cref="PaginatedList{TResult}"/> instance for fluent chaining.</returns>
	public PaginatedList<TResult> SetTotalCount(int totalCount)
	{
		TotalCount = totalCount;
		return this;
	}

	/// <summary>
	/// Creates a new <see cref="PaginatedList{TResult}"/> instance with the same pagination metadata but with a new set of items.
	/// </summary>
	/// <param name="items">The new list of items to use for the cloned paginated list.</param>
	/// <returns>A new <see cref="PaginatedList{TResult}"/> instance with the provided items and copied pagination info.</returns>
	public PaginatedList<TResult> Clone(List<TResult> items)
	{
		return new PaginatedList<TResult>(items, PageIndex, PageSize)
		{
			TotalCount = TotalCount
		};
	}

	/// <summary>
	/// Creates a <see cref="PaginatedList{TResult}"/> from an existing list of items, assuming these items represent a single page.
	/// </summary>
	/// <param name="items">The list of items for the page.</param>
	/// <param name="pageIndex">The current page index (1-based). Defaults to 1.</param>
	/// <param name="pageSize">The number of items per page. Defaults to 10.</param>
	/// <returns>A new <see cref="PaginatedList{TResult}"/> instance.</returns>
	/// <remarks>The <see cref="TotalCount"/> is not set by this method; it typically defaults to 0 unless explicitly set later.</remarks>
	public static PaginatedList<TResult> FromList(List<TResult> items, int pageIndex = 1, int pageSize = 10)
	{
		return new PaginatedList<TResult>(items, pageIndex, pageSize);
	}

	/// <summary>
	/// Asynchronously counts the total number of items in the provided queryable source.
	/// </summary>
	/// <param name="source">The <see cref="IQueryable{T}"/> source to count items from.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the total number of items in the source.</returns>
	public static async Task<int> CountAsync(IQueryable<TResult> source, CancellationToken cancellationToken = default)
		=> await source.CountAsync(cancellationToken);

	/// <summary>
	/// Asynchronously creates a <see cref="PaginatedList{TResult}"/> by taking a specific page from the queryable source.
	/// </summary>
	/// <param name="source">The <see cref="IQueryable{T}"/> source to paginate.</param>
	/// <param name="pageIndex">The 1-based page index to retrieve. If less than 1, it will be treated as 1.</param>
	/// <param name="pageSize">The number of items per page.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a <see cref="PaginatedList{TResult}"/>
	/// with items for the specified page. The <see cref="TotalCount"/> is not set by this method.
	/// </returns>
	/// <remarks>
	/// This method retrieves a specific slice of data corresponding to the requested page.
	/// To get a fully populated <see cref="PaginatedList{TResult}"/> with <see cref="TotalCount"/>,
	/// you typically need to call <see cref="CountAsync(IQueryable{TResult}, CancellationToken)"/> separately and then <see cref="SetTotalCount(int)"/>.
	/// </remarks>
	public static async Task<PaginatedList<TResult>> CreateAsync(
		IQueryable<TResult> source,
		int pageIndex,
		int pageSize,
		CancellationToken cancellationToken = default)
	{
		if (pageIndex < 1) pageIndex = 1;
		var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);

		return new PaginatedList<TResult>(items, pageIndex, pageSize);
	}
}