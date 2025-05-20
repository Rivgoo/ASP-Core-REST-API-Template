using Application.Options;
using Application.Results;
using Application.Users.Abstractions;
using Application.Users.Models;
using Application.Utilities;
using Domain;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Application.Users;

internal class UserRegistrator(
	IOptions<IdentityOptions> identityOptionsAccessor,
	UserManager<User> userManager,
	IUserService userService,
	IOptions<PhoneNumberValidationOptions> phoneNumberValidationOptions) : IUserRegistrator
{
	private readonly IUserService _userService = userService;
	private readonly UserManager<User> _userManager = userManager;
	private readonly PasswordOptions _passwordOptions = identityOptionsAccessor.Value.Password;
	private readonly PhoneNumberValidationOptions _phoneNumberValidationOptions = phoneNumberValidationOptions.Value;

	public async Task<Result<User>> RegisterAdminAsync(RegistrationUserModel model)
		=> await RegisterUserAsync(model, RoleList.Admin, true, false);
	public async Task<Result<User>> RegisterCustomerAsync(RegistrationUserModel model)
		=> await RegisterUserAsync(model, RoleList.Customer, true, false);

	private async Task<Result<User>> RegisterUserAsync(
		RegistrationUserModel model, string role, bool isEmailConfirmed, bool isPhoneNumberConfirmed)
	{
		if (StringUtilities.ValidatePhoneNumber(model.PhoneNumber, _phoneNumberValidationOptions.MinDigits, _phoneNumberValidationOptions.MaxDigits) == false)
			return Result<User>.Bad(UserErrors.InvalidPhoneNumber);

		var user = new User
		{
			Email = model.Email,
			NormalizedEmail = model.Email.ToUpperInvariant(),
			UserName = model.Email,
			NormalizedUserName = model.Email.ToUpperInvariant(),
			FirstName = model.FirstName,
			LastName = model.LastName,
			PhoneNumber = model.PhoneNumber,
			PhoneNumberConfirmed = isPhoneNumberConfirmed,
			IsBlocked = false,
			EmailConfirmed = isEmailConfirmed,
			LockoutEnabled = true,
		};

		if (StringUtilities.ValidatePassword(model.Password, _passwordOptions) == false)
			return Result<User>.Bad(UserErrors.InvalidPassword);

		if (!StringUtilities.ValidateEmail(model.Email))
			return Result<User>.Bad(UserErrors.InvalidEmail);

		var result = await _userService.CreateAsync(user);

		if (result.IsSuccess)
		{
			await _userManager.AddToRoleAsync(user, role);
			await _userManager.AddPasswordAsync(user, model.Password);

			return Result<User>.Ok(user);
		}

		return result;
	}
}