using System.ComponentModel.DataAnnotations;

namespace Domain.Abstractions;

/// <summary>
/// Provides a base for domain entities with a typed ID and audit properties, implementing <see cref="IBaseEntity{TId}"/>.
/// </summary>
/// <typeparam name="TId">The type of the unique identifier. Must be a non-nullable <see cref="IComparable{T}"/>.</typeparam>
/// <remarks>
/// Intended for inheritance by domain entities.
/// <see cref="Id"/> is the primary key (<see cref="KeyAttribute"/>).
/// <see cref="CreatedAt"/> and <see cref="UpdatedAt"/> are required (<see cref="RequiredAttribute"/>) audit timestamps, usually auto-populated.
/// </remarks>
/// <seealso cref="IBaseEntity{TId}"/>
/// <seealso cref="IAuditableEntity"/>
/// <seealso cref="IEntity"/>
public abstract class BaseEntity<TId> : IBaseEntity<TId>
	where TId : notnull, IComparable<TId>
{
	/// <summary>
	/// Gets or sets the entity's unique identifier.
	/// </summary>
	/// <value>The unique identifier of type <typeparamref name="TId"/>, typically the primary key.</value>
	[Key]
	public TId Id { get; set; }

	/// <summary>
	/// Gets or sets the UTC creation timestamp.
	/// </summary>
	/// <value>The <see cref="DateTime"/> the entity was created. Usually set automatically on initial save.</value>
	[Required]
	public DateTime CreatedAt { get; set; }

	/// <summary>
	/// Gets or sets the UTC last modification timestamp.
	/// </summary>
	/// <value>The <see cref="DateTime"/> the entity was last updated. Usually set automatically on save.</value>
	[Required]
	public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// Provides a base for domain entities with an <see cref="int"/> ID.
/// </summary>
/// <remarks>
/// Specializes <see cref="BaseEntity{TId}"/> for entities with <see cref="int"/> primary keys. 
/// Inherits <see cref="BaseEntity{TId}.Id"/>, <see cref="BaseEntity{TId}.CreatedAt"/>, and <see cref="BaseEntity{TId}.UpdatedAt"/>.
/// Abstract; for inheritance.
/// </remarks>
/// <seealso cref="BaseEntity{TId}"/>
public abstract class BaseEntity : BaseEntity<int>
{
}