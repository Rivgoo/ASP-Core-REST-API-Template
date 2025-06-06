using Application.Results;
using Domain.Abstractions;
using System.Linq.Expressions;

namespace Application.Filters.Abstractions;

/// <summary>
/// Defines a service for building and applying filtered, sorted, and paginated queries to entities.
/// </summary>
/// <typeparam name="TEntity">The type of the domain entity being queried. Must be a class and implement <see cref="IEntity"/>.</typeparam>
/// <typeparam name="TFilter">The type of the filter object used to specify query criteria. Must implement <see cref="IFilter"/>.</typeparam>
/// <remarks>
/// This service facilitates the creation of complex queries by chaining filter, sorter, and pagination settings
/// before finally executing the query and retrieving results as a <see cref="PaginatedList{TResult}"/>.
/// </remarks>
public interface IFilterService<TEntity, TFilter>
	where TEntity : class, IEntity
	where TFilter : IFilter
{
	/// <summary>
	/// Constructs an <see cref="IQueryable{TResult}"/> based on the configured filter, sorter, and a custom selector expression.
	/// </summary>
	/// <typeparam name="TResult">The type of the result objects projected from the entity. Must be a class.</typeparam>
	/// <param name="selector">The LINQ expression used to project <typeparamref name="TEntity"/> instances to <typeparamref name="TResult"/> instances.</param>
	/// <returns>An <see cref="IQueryable{TResult}"/> representing the constructed query, ready for further composition or execution.</returns>
	/// <remarks>This method applies sorting and filtering but does not execute the query or apply pagination.</remarks>
	IQueryable<TResult> GetQuery<TResult>(Expression<Func<TEntity, TResult>> selector)
		where TResult : class;

	/// <summary>
	/// Constructs an <see cref="IQueryable{TResult}"/> based on the configured filter, sorter, and a specified <see cref="ISelector{TEntity, TResult}"/> implementation.
	/// </summary>
	/// <typeparam name="TResult">The type of the result objects. Must be a class.</typeparam>
	/// <typeparam name="TSelector">The type of the <see cref="ISelector{TEntity, TResult}"/> used for projection.</typeparam>
	/// <returns>An <see cref="IQueryable{TResult}"/> representing the constructed query.</returns>
	/// <remarks>This method applies sorting and filtering but does not execute the query or apply pagination.</remarks>
	IQueryable<TResult> GetQuery<TResult, TSelector>()
		where TResult : class
		where TSelector : ISelector<TEntity, TResult>;

	/// <summary>
	/// Adds the filter instance to be used for query construction.
	/// </summary>
	/// <param name="filter">The filter object implementing <typeparamref name="TFilter"/> containing the criteria.</param>
	/// <returns>The current <see cref="IFilterService{TEntity, TFilter}"/> instance to allow fluent chaining.</returns>
	IFilterService<TEntity, TFilter> AddFilter(TFilter filter);

	/// <summary>
	/// Specifies the sorter implementation to be used for ordering the query results.
	/// </summary>
	/// <typeparam name="TSorter">The type of the sorter, which must implement <see cref="ISorter{TEntity, TFilter}"/>.</typeparam>
	/// <returns>The current <see cref="IFilterService{TEntity, TFilter}"/> instance to allow fluent chaining.</returns>
	IFilterService<TEntity, TFilter> AddSorter<TSorter>() where TSorter : ISorter<TEntity, TFilter>;

	/// <summary>
	/// Sets the page size for pagination of the query results.
	/// </summary>
	/// <param name="pageSize">The number of items to include in each page. Values may be clamped based on application settings.</param>
	/// <returns>The current <see cref="IFilterService{TEntity, TFilter}"/> instance to allow fluent chaining.</returns>
	IFilterService<TEntity, TFilter> SetPageSize(int pageSize);

	/// <summary>
	/// Configures whether Entity Framework Core should use split queries.
	/// </summary>
	/// <param name="splitQuery">If <see langword="true"/>, enables split queries for potentially better performance with related data; otherwise, <see langword="false"/>. Defaults to <see langword="true"/>.</param>
	/// <returns>The current <see cref="IFilterService{TEntity, TFilter}"/> instance to allow fluent chaining.</returns>
	/// <remarks>Split queries can help avoid Cartesian explosion issues when loading collections.</remarks>
	IFilterService<TEntity, TFilter> SplitQuery(bool splitQuery = true);

	/// <summary>
	/// Asynchronously constructs, executes, and paginates a query using a specified <see cref="ISelector{TEntity, TResult}"/>, returning a <see cref="PaginatedList{TResult}"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of items in the resulting paginated list. Must be a class.</typeparam>
	/// <typeparam name="TSelector">The type of the <see cref="ISelector{TEntity, TResult}"/> used for projection.</typeparam>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> of <see cref="PaginatedList{TResult}"/>.
	/// On success, the <see cref="Result{TValue}"/> is Ok with the <see cref="PaginatedList{TResult}"/>.
	/// On failure, it is Bad with an <see cref="Error"/>.
	/// </returns>
	Task<Result<PaginatedList<TResult>>> ApplyAsync<TResult, TSelector>(CancellationToken cancellationToken = default)
		where TResult : class
		where TSelector : ISelector<TEntity, TResult>;

	/// <summary>
	/// Asynchronously constructs, executes, and paginates a query using a custom selector expression, returning a <see cref="PaginatedList{TResult}"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of items in the resulting paginated list. Must be a class.</typeparam>
	/// <param name="selector">The LINQ expression used to project <typeparamref name="TEntity"/> instances to <typeparamref name="TResult"/> instances.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> of <see cref="PaginatedList{TResult}"/>.
	/// On success, the <see cref="Result{TValue}"/> is Ok with the <see cref="PaginatedList{TResult}"/>.
	/// On failure, it is Bad with an <see cref="Error"/>.
	/// </returns>
	Task<Result<PaginatedList<TResult>>> ApplyAsync<TResult>(
		Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
		where TResult : class;

	/// <summary>
	/// Asynchronously constructs a query using an <see cref="ISelector{TEntity, TResult}"/>, unions it with another <see cref="IQueryable{TResult}"/>,
	/// then executes and paginates the combined result. Allows optional custom ordering for the final unioned query.
	/// </summary>
	/// <typeparam name="TResult">The type of items in the resulting paginated list. Must be a class.</typeparam>
	/// <typeparam name="TSelector">The type of the <see cref="ISelector{TEntity, TResult}"/> used for projecting the primary query.</typeparam>
	/// <param name="union">The <see cref="IQueryable{TResult}"/> to union with the primary query results.</param>
	/// <param name="resultOrder">An optional list of <see cref="QueryableOrder"/> to apply to the final unioned result set before pagination. If null or empty, the existing order (if any) from the primary query or union query might be preserved, or default database ordering might apply.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> of <see cref="PaginatedList{TResult}"/>.
	/// On success, the <see cref="Result{TValue}"/> is Ok with the <see cref="PaginatedList{TResult}"/>.
	/// On failure, it is Bad with an <see cref="Error"/>.
	/// </returns>
	Task<Result<PaginatedList<TResult>>> ApplyWithUnionAsync<TResult, TSelector>(
		IQueryable<TResult> union,
		List<QueryableOrder>? resultOrder = null,
		CancellationToken cancellationToken = default)
		where TResult : class
		where TSelector : ISelector<TEntity, TResult>;

	/// <summary>
	/// Asynchronously constructs a query using a selector expression, unions it with another <see cref="IQueryable{TResult}"/>,
	/// then executes and paginates the combined result. Allows optional custom ordering for the final unioned query.
	/// </summary>
	/// <typeparam name="TResult">The type of items in the resulting paginated list. Must be a class.</typeparam>
	/// <param name="union">The <see cref="IQueryable{TResult}"/> to union with the primary query results.</param>
	/// <param name="selector">The LINQ expression used to project <typeparamref name="TEntity"/> instances to <typeparamref name="TResult"/> for the primary query.</param>
	/// <param name="resultOrder">An optional list of <see cref="QueryableOrder"/> to apply to the final unioned result set before pagination. If null or empty, the existing order (if any) from the primary query or union query might be preserved, or default database ordering might apply.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation. The task result contains a <see cref="Result{TValue}"/> of <see cref="PaginatedList{TResult}"/>.
	/// On success, the <see cref="Result{TValue}"/> is Ok with the <see cref="PaginatedList{TResult}"/>.
	/// On failure, it is Bad with an <see cref="Error"/>.
	/// </returns>
	Task<Result<PaginatedList<TResult>>> ApplyWithUnionAsync<TResult>(
		IQueryable<TResult> union,
		Expression<Func<TEntity, TResult>> selector,
		List<QueryableOrder>? resultOrder = null,
		CancellationToken cancellationToken = default)
		where TResult : class;
}