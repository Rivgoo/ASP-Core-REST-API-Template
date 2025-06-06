using Domain.Abstractions;

namespace Application.Filters.Abstractions;

/// <summary>
/// Defines a contract for selecting and transforming entities of type <typeparamref name="TEntity"/> into result objects of type <typeparamref name="TResult"/>.
/// </summary>
/// <typeparam name="TEntity">The source entity type. Must implement <see cref="IEntity"/>.</typeparam>
/// <typeparam name="TResult">The target result type after projection.</typeparam>
/// <remarks>
/// Implementations of this interface are responsible for defining the projection logic
/// from a queryable source of entities to a queryable source of result objects.
/// This is typically used to shape data for DTOs or specific view models.
/// </remarks>
public interface ISelector<TEntity, TResult>
	where TEntity : IEntity
{
	/// <summary>
	/// Applies a projection to the source queryable of entities.
	/// </summary>
	/// <param name="source">The <see cref="IQueryable{T}"/> of <typeparamref name="TEntity"/> to transform.</param>
	/// <returns>An <see cref="IQueryable{T}"/> of <typeparamref name="TResult"/> representing the projected data.</returns>
	/// <remarks>
	/// The implementation should define how each <typeparamref name="TEntity"/> is mapped to a <typeparamref name="TResult"/>.
	/// This method does not execute the query; it builds upon the existing <see cref="IQueryable{T}"/>.
	/// </remarks>
	IQueryable<TResult> Select(IQueryable<TEntity> source);
}