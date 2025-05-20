using Application.Abstractions;
using Application.Abstractions.Repositories;
using Application.Filters.Abstractions;
using Application.Users;
using Application.Users.Abstractions;
using Domain.Entities;
using Infrastructure.Core;
using Infrastructure.Filters.Selectors;
using Infrastructure.Filters.Sorters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Infrastructure;

public static class Dependency
{
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		#region Database  
		var connectionString = configuration.GetConnectionString("DataBaseConnection");

		if (string.IsNullOrWhiteSpace(connectionString))
			throw new ArgumentNullException(nameof(configuration), "Connection string is not configurationured.");

		services.AddDbContext<CoreDbContext>(
			options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
			options => options.EnableRetryOnFailure(
						maxRetryCount: 3,
						maxRetryDelay: TimeSpan.FromSeconds(5),
						errorNumbersToAdd: null))
			.UseSnakeCaseNamingConvention());

		var serviceProvider = services.BuildServiceProvider();

		using (var scope = serviceProvider.CreateScope())
		{
			var scopedServices = scope.ServiceProvider;

			var dbContext = scopedServices.GetRequiredService<CoreDbContext>();
			var logger = scopedServices.GetService<ILogger<CoreDbContext>>();

			var applyMigrations = configuration.GetValue<bool>("ApplyMigrations");

			if (applyMigrations)
			{
				try
				{
					dbContext.Database.Migrate();

					logger?.LogInformation("Database migrations applied successfully during service configurationuration.");
				}
				catch (Exception ex)
				{
					logger?.LogError(ex, "An error occurred while applying database migrations during service configurationuration.");
				}
			}
			else
				logger?.LogInformation("Automatic database migration is disabled by configurationuration.");
		}
		#endregion

		services.AddScoped<IUnitOfWork, UnitOfWork>();

		#region Identity
		services.AddIdentity<User, Role>(options =>
			{
				configuration.Bind("IdentitySettings:SignIn", options.SignIn);
				configuration.Bind("IdentitySettings:User", options.User);
				configuration.Bind("IdentitySettings:Lockout", options.Lockout);
				configuration.Bind("IdentitySettings:Password", options.Password);
			})
			.AddDefaultTokenProviders()
			.AddEntityFrameworkStores<CoreDbContext>();
		#endregion

		#region Repositories
		var infrastructureAssembly = typeof(CoreDbContext).Assembly;

		var repositoryTypes = infrastructureAssembly.GetTypes()
			.Where(type => type.IsClass && !type.IsAbstract)
			.Where(type => typeof(IRepository).IsAssignableFrom(type))
			.ToList();

		foreach (var repositoryType in repositoryTypes)
		{
			var implementedInterfaces = repositoryType.GetInterfaces()
				.Where(i => i != typeof(IRepository) && i != typeof(IDisposable))
				.ToList();

			if (implementedInterfaces.Count != 0)
				foreach (var interfaceType in implementedInterfaces)
					services.AddScoped(interfaceType, repositoryType);
		}
		#endregion

		#region Filters
		services.AddScoped<ISorter<User, UserFilter>, UserSorter>();
		services.AddScoped<IUserSelector, UserSelector>();
		#endregion

		return services;
	}
}