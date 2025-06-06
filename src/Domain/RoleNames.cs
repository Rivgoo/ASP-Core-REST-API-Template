namespace Domain;

/// <summary>
/// Provides a centralized list of constant string values representing standard application roles.
/// </summary>
public static class RoleNames
{
	/// <summary>
	/// Represents the Administrator role.
	/// </summary>
	/// <value>The string "Admin".</value>
	/// <remarks>Users with this role typically have full access to system administration functionalities.</remarks>
	public const string Admin = nameof(Admin);

	/// <summary>
	/// Represents the Customer role.
	/// </summary>
	/// <value>The string "Customer".</value>
	/// <remarks>Users with this role are standard consumers or clients of the application's services.</remarks>
	public const string Customer = nameof(Customer);
}