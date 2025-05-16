using Application.Users.Dtos;
using Application.Users.Models;
using AutoMapper;
using Domain.Entities;
using Web.API.Core.Options;

namespace Web.API.Controllers.V1.Users;

internal class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<InitialAdminOptions, RegistrationUserModel>();
		CreateMap<RegistrationUserRequest, RegistrationUserModel>();

		CreateMap<User, UserDto>();
	}
}