using Application.Abstractions.Services;
using Application.Results;
using Application.Users.Models;
using Domain.Entities;

namespace Application.Users.Abstractions;

/// <summary>
/// Defines service operations specific to managing <see cref="User"/> entities,
/// extending the generic <see cref="IEntityService{TEntity, TId}"/>.
/// </summary>
/// <remarks>
/// This interface provides functionalities beyond standard CRUD operations for users,
/// such as retrieving user information by ID or email, checking email existence, and updating last login timestamps.
/// All operations return <see cref="Result"/> or <see cref="Result{TValue}"/> to indicate outcomes.
/// </remarks>
/// <seealso cref="IEntityService{User, String}"/>
/// <seealso cref="User"/>
/// <seealso cref="UserInfo"/>
public interface IUserService : IEntityService<User, string>
{
	/// <summary>
	/// Asynchronously retrieves basic user information (<see cref="UserInfo"/> DTO) for a user by their ID.
	/// </summary>
	/// <param name="id">The unique identifier of the user.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a <see cref="Result{TValue}"/> of <see cref="UserInfo"/>.
	/// If successful, it's <see cref="Result.Ok()"/> with the <see cref="UserInfo"/> object.
	/// If the user is not found, it's <see cref="Result.Bad(Error)"/> with an <see cref="EntityErrors{User, String}.NotFoundById(string)"/> error.
	/// </returns>
	Task<Result<UserInfo>> GetUserInfoByIdAsync(string id, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously checks if a user with the specified email address exists.
	/// </summary>
	/// <param name="email">The email address to check. Must be a valid email format.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a <see cref="Result{TValue}"/> of <see cref="bool"/>.
	/// If successful, it's <see cref="Result.Ok()"/> with <see langword="true"/> if the email exists, or <see langword="false"/> otherwise.
	/// If the email format is invalid, it's <see cref="Result.Bad(Error)"/> with a <see cref="UserErrors.InvalidEmail"/> error.
	/// </returns>
	Task<Result<bool>> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously retrieves a <see cref="User"/> entity by its email address.
	/// </summary>
	/// <param name="email">The email address of the user to retrieve.</param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous operation.
	/// The task result contains a <see cref="Result{TValue}"/> of <see cref="User"/>.
	/// If successful, it's <see cref="Result.Ok()"/> with the <see cref="User"/> entity.
	/// If the user is not found, it's <see cref="Result.Bad(Error)"/> with an <see cref="EntityErrors{User, String}.NotFoundById(string)"/> error (using email as the identifier in the error message).
	/// </returns>
	Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

	/// <summary>
	/// Asynchronously updates the last login timestamp for the specified user to the current UTC time.
	/// </summary>
	/// <param name="userId">The unique identifier of the user whose last login time is to be updated.</param>
	/// <returns>A task that represents the asynchronous operation.</returns>
	/// <remarks>
	/// This operation typically updates the <see cref="User.LastLoginAt"/> property and saves the change.
	/// It doesn't return a <see cref="Result"/> object as it's generally considered a fire-and-forget operation
	/// from the caller's perspective, or errors are handled internally (e.g., logging).
	/// </remarks>
	Task UpdateLastLogin(string userId);
}