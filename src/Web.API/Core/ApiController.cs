using Application.Filters.Abstractions;
using Application.Filters;
using Application.Results;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc; 

namespace Web.API.Core;

/// <summary>
/// Provides a common abstract base class for all API controllers in the application.
/// </summary>
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public abstract class ApiController : ControllerBase
{
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