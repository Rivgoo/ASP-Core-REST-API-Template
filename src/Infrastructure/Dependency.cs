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

/// <summary>
/// Provides extension methods for <see cref="IServiceCollection"/> to register Infrastructure layer specific services and configurations.
/// </summary>
public static class Dependency
{
	/// <summary>
	/// Registers services, DbContext, repositories, and other dependencies defined within the Infrastructure layer into the dependency injection container.
	/// </summary>
	/// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
	/// <param name="configuration">The application's <see cref="IConfiguration"/> instance, used for accessing connection strings and other settings.</param>
	/// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
	/// <exception cref="ArgumentNullException">Thrown if the database connection string is not found in the configuration.</exception>
	/// <remarks>
	/// This method configures:
	/// <list type="bullet">
	///   <item>Database: Registers <see cref="CoreDbContext"/> with MySQL, applying snake case naming convention and retry on failure logic. Optionally applies database migrations based on "ApplyMigrations" configuration.</item>
	///   <item>Unit of Work: Registers <see cref="IUnitOfWork"/> with its implementation <see cref="UnitOfWork"/>.</item>
	///   <item>Identity: Configures ASP.NET Core Identity services for <see cref="User"/> and <see cref="Role"/>, binding settings from configuration and using <see cref="CoreDbContext"/> as the store.</item>
	///   <item>Repositories: Automatically discovers and registers repository implementations from the Infrastructure assembly that implement <see cref="IRepository"/>.</item>
	///   <item>Filters: Registers specific sorter (<see cref="ISorter{User, UserFilter}"/> for <see cref="UserSorter"/>) and selector (<see cref="IUserSelector"/> for <see cref="UserSelector"/>) implementations.</item>
	/// </list>
	/// </remarks>
	public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
	{
		#region Database  
		var connectionString = configuration.GetConnectionString("DataBaseConnection");

		if (string.IsNullOrWhiteSpace(connectionString))
			throw new ArgumentNullException(nameof(configuration), "Connection string ('DataBaseConnection') is not configured.");

		services.AddDbContext<CoreDbContext>(
			options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
			sqlOptions => sqlOptions.EnableRetryOnFailure(
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
					logger?.LogInformation("Database migrations applied successfully during service configuration.");
				}
				catch (Exception ex)
				{
					logger?.LogError(ex, "An error occurred while applying database migrations during service configuration.");
				}
			}
			else
			{
				logger?.LogInformation("Automatic database migration is disabled by configuration.");
			}
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
				.Where(i => i != typeof(IRepository) && i != typeof(IDisposable) && i.IsPublic)
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