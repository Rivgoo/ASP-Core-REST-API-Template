using Application.Abstractions.Services;
using Application.Authentication;
using Application.Authentication.Abstractions;
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

		#region Auto Entity Services discovery
		var applicationAssembly = typeof(Dependency).Assembly;

		var serviceTypes = applicationAssembly.GetTypes()
			.Where(type => type.IsClass && !type.IsAbstract && type.Name.EndsWith("Service"));

		foreach (var serviceType in serviceTypes)
		{
			var specificInterfaces = serviceType.GetInterfaces()
				.Where(i => !(i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IEntityService<,>))
						&& i != typeof(IDisposable)
						&& i.Namespace != null && i.Namespace.StartsWith("Application"))
				.ToList();

			if (specificInterfaces.Count != 0)
				foreach (var interfaceType in specificInterfaces)
					services.AddScoped(interfaceType, serviceType);
		}
		#endregion

		#region Services
		services.AddScoped<IUserRegistrator, UserRegistrator>();
		services.AddScoped<IAuthenticationService, AuthenticationService>();
		#endregion

		services.AddScoped(typeof(IFilterService<,>), typeof(FilterService<,>));

		return services;
	}
}