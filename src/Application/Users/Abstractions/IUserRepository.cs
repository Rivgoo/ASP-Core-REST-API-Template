using Application.Abstractions.Repositories;
using Application.Users.Models;
using Domain.Entities;

namespace Application.Users.Abstractions;

/// <summary>
/// Defines repository operations specific to <see cref="User"/> entities, extending common entity operations.
/// </summary>
/// <remarks>
/// This interface inherits from <see cref="IEntityOperations{TEntity, TId}"/> to provide standard CRUD
/// and existence check functionalities for <see cref="User"/> entities (where TId is <see cref="string"/>).
/// It adds user-specific query methods such as finding users by email and retrieving user information DTOs.
/// </remarks>
/// <seealso cref="IEntityOperations{User, String}"/>
/// <seealso cref="User"/>
/// <seealso cref="UserInfo"/>
public interface IUserRepository : IEntityOperations<User, string>
{
	/// <summary>
	/// Asynchronously checks if a <see cref="User"/> with the specified email address exists.
	/// </summary>
	/// <param name="email">The email address to check for existence. Must not be null or empty.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result is <see langword="true"/> if a user with the given email exists; otherwise, <see langword="false"/>.
	/// </returns>
	Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously retrieves a <see cref="User"/> entity by its email address.
	/// </summary>
	/// <param name="email">The email address of the user to retrieve. Must not be null or empty.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains the <see cref="User"/> entity if found; otherwise, <see langword="null"/>.
	/// The retrieved entity is typically not tracked by the change tracker for performance (<c>AsNoTracking()</c>).
	/// </returns>
	Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously retrieves basic user information (<see cref="UserInfo"/> DTO) for a user with the specified ID.
	/// </summary>
	/// <param name="id">The unique identifier of the user.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a <see cref="UserInfo"/> object with the user's first name, last name, and email if found;
	/// otherwise, <see langword="null"/>.
	/// The query is typically configured not to track entities for performance (<c>AsNoTracking()</c>).
	/// </returns>
	Task<UserInfo?> GetUserInfoByIdAsync(string id, CancellationToken cancellationToken);

	/// <summary>
	/// Asynchronously updates the <see cref="User.LastLoginAt"/> timestamp for the specified user.
	/// </summary>
	/// <param name="userId">The unique identifier of the user whose last login time is to be updated.</param>
	/// <param name="lastLoginAt">The <see cref="DateTime"/> (UTC) representing the new last login time.</param>
	/// <returns>A task that represents the asynchronous update operation.</returns>
	/// <remarks>
	/// This method typically performs a direct database update for the <see cref="User.LastLoginAt"/> field
	/// without loading the entire entity, often using <c>ExecuteUpdateAsync</c> for efficiency.
	/// </remarks>
	Task SetLastLoginAt(string userId, DateTime lastLoginAt);
}