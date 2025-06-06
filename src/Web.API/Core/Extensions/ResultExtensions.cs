using Application.Results;
using Microsoft.AspNetCore.Mvc;

namespace Web.API.Core.Extensions;

/// <summary>
/// Provides extension methods for <see cref="Application.Results.Result"/> and <see cref="Application.Results.Result{TValue}"/> types
/// to convert them into <see cref="Microsoft.AspNetCore.Mvc.IActionResult"/> suitable for API controller responses.
/// </summary>
internal static class ResultExtensions
{
	/// <summary>
	/// Converts a non-generic <see cref="Application.Results.Result"/> into an <see cref="Microsoft.AspNetCore.Mvc.IActionResult"/>.
	/// </summary>
	/// <param name="result">The <see cref="Application.Results.Result"/> to convert. This parameter is extended. Must not be null.</param>
	/// <returns>
	/// An <see cref="Microsoft.AspNetCore.Mvc.OkResult"/> (HTTP 200 OK) if <paramref name="result"/> <see cref="Application.Results.Result.IsSuccess"/> is <see langword="true"/>.
	/// An <see cref="Microsoft.AspNetCore.Mvc.ObjectResult"/> containing the <see cref="Application.Results.Result.Error"/> with an appropriate HTTP status code
	/// (derived from <see cref="Application.Results.Error.ToHttpStatusCode"/>) if <paramref name="result"/> <see cref="Application.Results.Result.IsFailure"/> is <see langword="true"/>.
	/// The content type of the error response is "application/json".
	/// </returns>
	/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
	/// <exception cref="System.NullReferenceException">Thrown if <paramref name="result"/> is a failure but <see cref="Application.Results.Result.Error"/> is null (which should be prevented by <see cref="Application.Results.Result"/>'s constructor logic).</exception>
	public static IActionResult ToActionResult(this Result result)
	{
		ArgumentNullException.ThrowIfNull(result);

		if (result.IsSuccess)
			return new OkResult();

		var error = result.Error ?? Error.Failure("Unknown.Error", "An unknown error occurred.");
		return new ObjectResult(error)
		{
			StatusCode = (int)error.ToHttpStatusCode(),
			ContentTypes = { "application/json" },
		};
	}

	/// <summary>
	/// Converts a generic <see cref="Application.Results.Result{TResult}"/> into an <see cref="Microsoft.AspNetCore.Mvc.IActionResult"/>.
	/// </summary>
	/// <typeparam name="TResult">The type of the value contained in the successful result.</typeparam>
	/// <param name="result">The <see cref="Application.Results.Result{TResult}"/> to convert. This parameter is extended. Must not be null.</param>
	/// <returns>
	/// An <see cref="Microsoft.AspNetCore.Mvc.OkObjectResult"/> (HTTP 200 OK) with the <see cref="Application.Results.Result{TValue}.Value"/> if <paramref name="result"/> <see cref="Application.Results.Result.IsSuccess"/> is <see langword="true"/> and the value is not null.
	/// An <see cref="Microsoft.AspNetCore.Mvc.OkResult"/> (HTTP 200 OK) if <paramref name="result"/> is successful but its <see cref="Application.Results.Result{TValue}.Value"/> is null.
	/// An <see cref="Microsoft.AspNetCore.Mvc.ObjectResult"/> containing the <see cref="Application.Results.Result.Error"/> with an appropriate HTTP status code
	/// (derived from <see cref="Application.Results.Error.ToHttpStatusCode"/>) if <paramref name="result"/> <see cref="Application.Results.Result.IsFailure"/> is <see langword="true"/>.
	/// The content type of the error response is "application/json".
	/// </returns>
	/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
	/// <exception cref="System.NullReferenceException">Thrown if <paramref name="result"/> is a failure but <see cref="Application.Results.Result.Error"/> is null (which should be prevented by <see cref="Application.Results.Result"/>'s constructor logic).</exception>
	public static IActionResult ToActionResult<TResult>(this Result<TResult> result)
	{
		ArgumentNullException.ThrowIfNull(result);

		if (result.IsSuccess)
		{
			if (result.Value == null)
				return new OkResult();
			else
				return new OkObjectResult(result.Value);
		}

		var error = result.Error ?? Error.Failure("Unknown.Error", "An unknown error occurred.");
		return new ObjectResult(error)
		{
			StatusCode = (int)error.ToHttpStatusCode(),
			ContentTypes = { "application/json" },
		};
	}

	/// <summary>
	/// Converts a generic <see cref="Application.Results.Result{TResult}"/> into an <see cref="Microsoft.AspNetCore.Mvc.IActionResult"/>,
	/// primarily focusing on the HTTP status code without returning the successful result's value in the response body.
	/// </summary>
	/// <typeparam name="TResult">The type of the value that would be contained in a successful result (but is not included in the HTTP response body by this method).</typeparam>
	/// <param name="result">The <see cref="Application.Results.Result{TResult}"/> to convert. This parameter is extended. Must not be null.</param>
	/// <returns>
	/// An <see cref="Microsoft.AspNetCore.Mvc.OkResult"/> (HTTP 200 OK) if <paramref name="result"/> <see cref="Application.Results.Result.IsSuccess"/> is <see langword="true"/> (response body will be empty).
	/// An <see cref="Microsoft.AspNetCore.Mvc.ObjectResult"/> containing the <see cref="Application.Results.Result.Error"/> with an appropriate HTTP status code
	/// (derived from <see cref="Application.Results.Error.ToHttpStatusCode"/>) if <paramref name="result"/> <see cref="Application.Results.Result.IsFailure"/> is <see langword="true"/>.
	/// The content type of the error response is "application/json".
	/// </returns>
	/// <exception cref="System.ArgumentNullException">Thrown if <paramref name="result"/> is null.</exception>
	/// <exception cref="System.NullReferenceException">Thrown if <paramref name="result"/> is a failure but <see cref="Application.Results.Result.Error"/> is null (which should be prevented by <see cref="Application.Results.Result"/>'s constructor logic).</exception>
	/// <remarks>
	/// This method is useful for operations like 'Update' or 'Delete' where a successful operation might
	/// return HTTP 200 OK or HTTP 204 No Content, but the primary information is the success status itself,
	/// rather than returning the modified/deleted entity. This specific implementation returns HTTP 200 OK on success.
	/// </remarks>
	public static IActionResult ToHttpStatusResult<TResult>(this Result<TResult> result)
	{
		ArgumentNullException.ThrowIfNull(result);

		if (result.IsSuccess)
			return new OkResult();

		var error = result.Error ?? Error.Failure("Unknown.Error", "An unknown error occurred.");
		return new ObjectResult(error)
		{
			StatusCode = (int)error.ToHttpStatusCode(),
			ContentTypes = { "application/json" },
		};
	}
}