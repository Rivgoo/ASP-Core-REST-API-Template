using Application.Filters.Abstractions;
using Application.Filters;
using Application.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Core;

/// <summary>
/// Provides an abstract base class for API controllers in the application.
/// It enforces JWT Bearer token authentication by default for all derived controllers and offers common utilities.
/// </summary>
/// <remarks>
/// This class is decorated with <see cref="Microsoft.AspNetCore.Mvc.ApiControllerAttribute"/> to enable API-specific behaviors
/// and <see cref="Microsoft.AspNetCore.Authorization.AuthorizeAttribute"/> to require authentication using the
/// <see cref="Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme"/>.
/// Derived controllers inherit these attributes but can override authorization behavior (e.g., using <see cref="Microsoft.AspNetCore.Authorization.AllowAnonymousAttribute"/>).
/// </remarks>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public abstract class ApiController : ControllerBase
{
	/// <summary>
	/// Applies sorting criteria, received typically from request parameters, to the provided filter instance.
	/// </summary>
	/// <param name="filter">The <see cref="IFilter"/> instance to which the ordering directives will be added.</param>
	/// <param name="orderField">An array of strings representing the property names to sort by (e.g., "LastName", "CreatedAt").</param>
	/// <param name="orderType">A list of <see cref="QueryableOrderType"/> values corresponding to each <paramref name="orderField"/>, specifying the sort direction and sequence.</param>
	/// <returns>
	/// A <see cref="Result"/> indicating the outcome:
	/// <list type="bullet">
	///   <item><description><see cref="Result.Ok()"/> if all ordering directives were successfully added to the <paramref name="filter"/>.</description></item>
	///   <item><description><see cref="Result.Bad(Error)"/> if the input is invalid. Possible errors include:
	///     <list type="bullet">
	///       <item><description><see cref="FilterErrors.OrderFieldCountMismatch"/>: If the number of <paramref name="orderField"/> elements does not match the number of <paramref name="orderType"/> elements.</description></item>
	///       <item><description><see cref="FilterErrors.InvalidOrderInput"/>: If an <paramref name="orderType"/> is missing for a corresponding <paramref name="orderField"/> during iteration.</description></item>
	///       <item><description>Errors from <see cref="IFilter.AddOrdering(QueryableOrderType, string)"/>, such as <see cref="FilterErrors.InvalidOrderField(string)"/> or <see cref="FilterErrors.OrderFieldAlreadyExists(string)"/>.</description></item>
	///     </list>
	///   </description></item>
	/// </list>
	/// </returns>
	/// <remarks>
	/// This method iterates through the <paramref name="orderField"/> and <paramref name="orderType"/> collections.
	/// For each pair, it attempts to add an ordering directive to the <paramref name="filter"/> using <see cref="IFilter.AddOrdering(QueryableOrderType, string)"/>.
	/// If any step fails, it returns the corresponding <see cref="Error"/>.
	/// </remarks>
	protected Result ApplyOrdering(IFilter filter, string[] orderField, List<QueryableOrderType> orderType)
	{
		if (orderField.Length != orderType.Count)
			return Result.Bad(FilterErrors.OrderFieldCountMismatch);

		for (var i = 0; i < orderField.Length; i++)
		{
			var field = orderField[i];

			if (orderType.Count <= i)
				return Result.Bad(FilterErrors.InvalidOrderInput);

			var type = orderType[i];
			var result = filter.AddOrdering(type, field);

			if (result.IsFailure)
				return result; 
		}

		return Result.Ok();
	}
}