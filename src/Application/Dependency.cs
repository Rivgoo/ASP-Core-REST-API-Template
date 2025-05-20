using Application.Abstractions.Services;
using Application.Filters.Abstractions;
using Application.Filters.Services;
using Application.Options;
using Application.Users;
using Application.Users.Abstractions;
using Application.Users.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Dependency
{
	public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
	{
		#region Configuration
		services.Configure<PhoneNumberValidationOptions>(configuration.GetSection(PhoneNumberValidationOptions.SectionName));
		services.Configure<FilterSettingsOptions>(configuration.GetSection(FilterSettingsOptions.SectionName));
		services.Configure<UserFilterSettingsOptions>(configuration.GetSection(UserFilterSettingsOptions.SectionName));
		#endregion

		#region Auto Services discovery
		var applicationAssembly = typeof(Dependency).Assembly;

		var serviceTypes = applicationAssembly.GetTypes()
			.Where(type => type.IsClass && !type.IsAbstract)
			.Where(type => type.GetInterfaces().Any(
				implementedInterface => implementedInterface.IsGenericType &&
										implementedInterface.GetGenericTypeDefinition() == typeof(IEntityService<,>)));

		foreach (var serviceType in serviceTypes)
		{
			var implementedInterfaces = serviceType.GetInterfaces()
				.Where(i => !(i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityService<,>)) && i != typeof(IDisposable))
				.ToList();

			if (implementedInterfaces.Count != 0)
				foreach (var interfaceType in implementedInterfaces)
					services.AddScoped(interfaceType, serviceType);
		}

		services.AddScoped<IUserRegistrator, UserRegistrator>();
		#endregion

		services.AddScoped(typeof(IFilterService<,>), typeof(FilterService<,>));

		return services;
	}
}