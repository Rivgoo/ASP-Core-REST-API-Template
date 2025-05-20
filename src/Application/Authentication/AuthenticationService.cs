using Application.Authentication.Abstractions;
using Application.Users.Abstractions;
using Application.Users.Models;
using Application.Utilities;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Authentication;

internal class AuthenticationService(
	UserManager<User> userManager,
	SignInManager<User> signInManager,
	IUserService userService,
	ILogger<AuthenticationService> logger) : IAuthenticationService
{
	private readonly UserManager<User> _userManager = userManager;
	private readonly SignInManager<User> _signInManager = signInManager;
	private readonly IUserService _userService = userService;
	private readonly ILogger<AuthenticationService> _logger = logger;

	public async Task<AuthenticationResult> AuthenticateAsync(string email, string password)
	{
		var result = new AuthenticationResult();

		if (!StringUtilities.ValidateEmail(email))
		{
			_logger.LogWarning("Authentication attempt with invalid email format: {Email}", email);
			result.IsInvalidCredentials = true;

			return result;
		}

		var userResult = await _userService.GetByEmailAsync(email);

		if (userResult.IsFailure || userResult.Value == null)
		{
			_logger.LogWarning("Authentication failed: User not found with email {Email}", email);
			result.IsInvalidCredentials = true;

			return result;
		}

		var user = userResult.Value;

		if (user.IsBlocked)
		{
			_logger.LogWarning("Authentication failed: User {Email} is blocked.", email);
			result.IsBlocked = true;

			return result;
		}

		if (_signInManager.UserManager.Options.SignIn.RequireConfirmedEmail && !user.EmailConfirmed)
		{
			_logger.LogWarning("Authentication failed: User {Email} email not confirmed and confirmation is required.", email);
			result.IsEmailNotConfirmed = true;

			return result;
		}

		var signInResult = await _signInManager.PasswordSignInAsync(user, password, isPersistent: false, lockoutOnFailure: true);

		if (signInResult.Succeeded)
		{
			_logger.LogInformation("User {Email} authenticated successfully.", email);

			await _userService.UpdateLastLogin(user.Id);

			var roles = await _userManager.GetRolesAsync(user);
			var role = roles.FirstOrDefault() ?? string.Empty;

			if (string.IsNullOrEmpty(role))
				_logger.LogWarning("User {Email} authenticated successfully but has no roles assigned.", email);

			result.User = new UserAuthenticationInfo(user.Id, role);
			result.Succeeded = true;
		}
		else if (signInResult.IsLockedOut)
		{
			_logger.LogWarning("Authentication failed: User {Email} is locked out.", email);
			result.IsLockedOut = true;
		}
		else if (signInResult.IsNotAllowed)
		{
			_logger.LogWarning("Authentication failed: User {Email} is not allowed to sign in. RequireConfirmedEmail: {RequireConfirmedEmail}, EmailConfirmed: {EmailConfirmed}",
			   email, _signInManager.UserManager.Options.SignIn.RequireConfirmedEmail, user.EmailConfirmed);

			result.IsInvalidCredentials = true;
		}
		else
		{
			_logger.LogWarning("Authentication failed: Invalid credentials for user {Email}.", email);
			result.IsInvalidCredentials = true;
		}

		return result;
	}
}