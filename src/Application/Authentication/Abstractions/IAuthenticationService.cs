using Application.Users.Models;

namespace Application.Authentication.Abstractions;

/// <summary>
/// Defines methods for user authentication.
/// </summary>
public interface IAuthenticationService
{
	/// <summary>
	/// Attempts to authenticate a user based on email and password.
	/// </summary>
	/// <param name="email">The user's email.</param>
	/// <param name="password">The user's password.</param>
	/// <returns>A task that represents the asynchronous authentication operation.
	/// The task result contains an <see cref="AuthenticationResult"/> indicating the outcome.</returns>
	Task<AuthenticationResult> AuthenticateAsync(string email, string password);
}