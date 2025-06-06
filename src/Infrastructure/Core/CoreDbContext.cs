using Domain;
using Domain.Abstractions;
using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Core;

/// <summary>
/// Represents the main database context for the application, integrating with ASP.NET Core Identity and providing custom entity configurations.
/// </summary>
/// <remarks>
/// This DbContext is derived from <see cref="IdentityDbContext{TUser, TRole, TKey, TUserClaim, TUserRole, TUserLogin, TRoleClaim, TUserToken}"/>
/// using <see cref="User"/>, <see cref="Role"/>, and <see cref="string"/> as the key type for Identity entities.
/// It is responsible for defining entity mappings, applying audit information automatically during save operations, and seeding initial data.
/// </remarks>
/// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
public sealed class CoreDbContext(DbContextOptions<CoreDbContext> options) : IdentityDbContext
	<User, Role, string, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>(options)
{
	#region Set up audit information
	/// <summary>
	/// Asynchronously saves all changes made in this context to the database, applying audit information before saving.
	/// </summary>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous save operation. The task result contains the
	/// number of state entries written to the database.
	/// </returns>
	/// <remarks>This override calls <see cref="ApplyAuditInfo"/> before deferring to the base implementation.</remarks>
	public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		ApplyAuditInfo();
		return await base.SaveChangesAsync(cancellationToken);
	}

	/// <summary>
	/// Asynchronously saves all changes made in this context to the database, applying audit information before saving.
	/// </summary>
	/// <param name="acceptAllChangesOnSuccess">
	/// Indicates whether <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges"/>
	/// is called after the changes have been sent successfully to the database.
	/// </param>
	/// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
	/// <returns>
	/// A task that represents the asynchronous save operation. The task result contains the
	/// number of state entries written to the database.
	/// </returns>
	/// <remarks>This override calls <see cref="ApplyAuditInfo"/> before deferring to the base implementation.</remarks>
	public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
	{
		ApplyAuditInfo();
		return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
	}

	/// <summary>
	/// Saves all changes made in this context to the database, applying audit information before saving.
	/// </summary>
	/// <returns>The number of state entries written to the database.</returns>
	/// <remarks>This override calls <see cref="ApplyAuditInfo"/> before deferring to the base implementation.</remarks>
	public override int SaveChanges()
	{
		ApplyAuditInfo();
		return base.SaveChanges();
	}

	/// <summary>
	/// Saves all changes made in this context to the database, applying audit information before saving.
	/// </summary>
	/// <param name="acceptAllChangesOnSuccess">
	/// Indicates whether <see cref="Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges"/>
	/// is called after the changes have been sent successfully to the database.
	/// </param>
	/// <returns>The number of state entries written to the database.</returns>
	/// <remarks>This override calls <see cref="ApplyAuditInfo"/> before deferring to the base implementation.</remarks>
	public override int SaveChanges(bool acceptAllChangesOnSuccess)
	{
		ApplyAuditInfo();
		return base.SaveChanges(acceptAllChangesOnSuccess);
	}
	#endregion

	/// <summary>
	/// Configures the schema needed for the identity system and other custom entity mappings.
	/// </summary>
	/// <param name="modelBuilder">
	/// The builder being used to construct the model for this context. Databases (and other extensions) typically
	/// define extension methods on this object that allow you to configure aspects of the model that are specific
	/// to a given database.
	/// </param>
	/// <remarks>
	/// This method is called only once when the first instance of a derived context is created.
	/// It sets custom table names for Identity entities (e.g., "users", "roles") and seeds initial roles (<see cref="RoleList.Admin"/>, <see cref="RoleList.Customer"/>).
	/// </remarks>
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);

		#region Set Table Names
		modelBuilder.Entity<User>(b => b.ToTable("users"));
		modelBuilder.Entity<Role>(b => b.ToTable("roles"));
		modelBuilder.Entity<UserClaim>(b => b.ToTable("user_claims"));
		modelBuilder.Entity<UserRole>(b => b.ToTable("user_roles"));
		modelBuilder.Entity<UserLogin>(b => b.ToTable("user_logins"));
		modelBuilder.Entity<RoleClaim>(b => b.ToTable("role_claims"));
		modelBuilder.Entity<UserToken>(b => b.ToTable("user_tokens"));
		#endregion

		#region Set Identity Roles
		modelBuilder.Entity<Role>().HasData(
			new Role("e0ddbbf0-c810-432d-8554-640db86c4443", RoleNames.Admin),
			new Role("0de7a5f6-d02a-4041-9c1f-abeb8ed44c92", RoleNames.Customer));
		#endregion
	}

	/// <summary>
	/// Applies audit information (CreatedAt and UpdatedAt timestamps) to entities that implement <see cref="IAuditableEntity"/>
	/// and are in an Added or Modified state.
	/// </summary>
	/// <remarks>
	/// For newly added entities, both <see cref="IAuditableEntity.CreatedAt"/> and <see cref="IAuditableEntity.UpdatedAt"/> are set to the current UTC time.
	/// For modified entities, only <see cref="IAuditableEntity.UpdatedAt"/> is set to the current UTC time, and <see cref="IAuditableEntity.CreatedAt"/> is explicitly marked as not modified.
	/// This method is called internally by the SaveChanges overrides.
	/// </remarks>
	private void ApplyAuditInfo()
	{
		var entries = ChangeTracker
			.Entries()
			.Where(e => e.Entity is IAuditableEntity &&
						(e.State == EntityState.Added || e.State == EntityState.Modified));

		var now = DateTime.UtcNow;

		foreach (var entry in entries)
		{
			var entity = (IAuditableEntity)entry.Entity;

			if (entry.State == EntityState.Added)
			{
				entity.CreatedAt = now;
				entity.UpdatedAt = now;
			}
			else if (entry.State == EntityState.Modified)
			{
				entity.UpdatedAt = now;
				entry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
			}
		}
	}
}