using Application.Abstractions;
using Application.Abstractions.Services;
using Application.Results;
using Application.Users.Abstractions;
using Application.Users.Models;
using Application.Utilities;
using Domain.Entities;

namespace Application.Users;

internal class UserService(
	IUserRepository entityRepository,
	IUnitOfWork unitOfWork) :
	BaseEntityService<User, string, IUserRepository>(entityRepository, unitOfWork), IUserService
{
	public override async Task<Result<User>> UpdateAsync(User changedEntity)
	{
		var alreadyExistsResult = await ExistsByEmailAsync(changedEntity.Email);

		if (alreadyExistsResult.IsFailure)
			return alreadyExistsResult.ToValue<User>();

		if (alreadyExistsResult.Value)
		{
			var existingEntity = await _entityRepository.GetByEmailAsync(changedEntity.Email);

			if (existingEntity != null && changedEntity.Id != existingEntity.Id)
				return Result<User>.Bad(UserErrors.UserWithEmailAlreadyExists(changedEntity.Email));
		}
		else
		{
			changedEntity.NormalizedEmail = changedEntity.Email.ToUpperInvariant();
			changedEntity.NormalizedUserName = changedEntity.Email.ToUpperInvariant();
			changedEntity.UserName = changedEntity.Email;
		}

		return await base.UpdateAsync(changedEntity);
	}

	public async Task<Result<bool>> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		if (Guard.MaxLength(email, 255) || StringUtilities.ValidateEmail(email) == false)
			return Result<bool>.Bad(UserErrors.InvalidEmail);

		return Result<bool>.Ok(await entityRepository.ExistsByEmailAsync(email, cancellationToken));
	}
	public async Task<Result<User>> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
	{
		var user = await entityRepository.GetByEmailAsync(email, cancellationToken);

		if (user == null)
			return Result<User>.Bad(EntityErrors<User, string>.NotFoundById(email));

		return Result<User>.Ok(user);
	}
	public async Task<Result<UserInfo>> GetUserInfoByIdAsync(string id, CancellationToken cancellationToken = default)
	{
		var userInfo = await entityRepository.GetUserInfoByIdAsync(id, cancellationToken);

		if (userInfo == null)
			return Result<UserInfo>.Bad(EntityErrors<User, string>.NotFoundById(id));

		return Result<UserInfo>.Ok(userInfo);
	}
	public async Task UpdateLastLogin(string userId)
		=> await _entityRepository.SetLastLoginAt(userId, DateTime.UtcNow);

	protected override async Task<Result> ValidateEntityAsync(User entity)
	{
		StringUtilities.TrimStringProperties(entity);

		if (Guard.MaxLength(entity.Email, 255) || StringUtilities.ValidateEmail(entity.Email) == false)
			return Result<User>.Bad(UserErrors.InvalidEmail);

		if (Guard.MinLength(entity.FirstName, 1))
			return Result.Bad(EntityErrors<User, string>.StringTooShort(nameof(entity.FirstName), 1));

		if (Guard.MaxLength(entity.FirstName, 255))
			return Result.Bad(EntityErrors<User, string>.StringTooLong(nameof(entity.FirstName), 255));

		if (Guard.MinLength(entity.LastName, 1))
			return Result.Bad(EntityErrors<User, string>.StringTooShort(nameof(entity.LastName), 1));

		if (Guard.MaxLength(entity.LastName, 255))
			return Result.Bad(EntityErrors<User, string>.StringTooLong(nameof(entity.LastName), 255));

		return Result.Ok();
	}
}