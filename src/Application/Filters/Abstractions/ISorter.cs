using Domain.Abstractions;

namespace Application.Filters.Abstractions;

/// <summary>
/// Defines a contract for applying sorting logic to a query of entities based on filter criteria.
/// </summary>
/// <typeparam name="TEntity">The type of the entity being queried. Must implement <see cref="IEntity"/>.</typeparam>
/// <typeparam name="TFilter">The type of the filter object that provides sorting parameters. Must implement <see cref="IFilter"/>.</typeparam>
/// <remarks>
/// Implementations of this interface are responsible for constructing an <see cref="IQueryable{T}"/>
/// that incorporates specific filtering and potentially initial sorting logic based on the properties
/// defined in the <typeparamref name="TFilter"/> object. The returned queryable can then be further
/// refined with additional ordering specified by <see cref="IFilter.GetOrders"/>.
/// </remarks>
public interface ISorter<TEntity, TFilter>
	where TEntity : IEntity
	where TFilter : IFilter
{
	/// <summary>
	/// Applies filtering and potentially initial sorting logic to the base entity set based on the provided filter.
	/// </summary>
	/// <param name="filter">The filter object of type <typeparamref name="TFilter"/> containing the criteria to apply.</param>
	/// <returns>An <see cref="IQueryable{T}"/> of <typeparamref name="TEntity"/> representing the filtered (and possibly pre-sorted) set of entities.</returns>
	/// <remarks>
	/// This method typically builds the WHERE clause of a query based on the <paramref name="filter"/> properties.
	/// It should not apply the explicit ordering directives from <see cref="IFilter.GetOrders"/>; that is usually handled
	/// by the <see cref="IFilterService{TEntity, TFilter}"/>.
	/// The returned queryable is not executed by this method.
	/// </remarks>
	IQueryable<TEntity> GetSort(TFilter filter);
}