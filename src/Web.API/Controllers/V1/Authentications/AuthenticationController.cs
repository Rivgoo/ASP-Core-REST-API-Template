using Application.Authentication.Abstractions;
using Application.Results;
using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.API.Core;
using Web.API.Core.Jwt;

namespace Web.API.Controllers.V1.Authentications;

/// <summary>
/// API Controller for handling user authentication and token issuance.
/// </summary>
/// <remarks>
/// This controller provides endpoints for user login to obtain JWT and refresh tokens.
/// It interacts with user services to authenticate credentials and JWT services
/// to generate tokens based on the configured policy.
/// </remarks>
/// <remarks>
/// Initializes a new instance of the <see cref="AuthenticationController"/> class.
/// </remarks>
/// <param name="jwtAuthentication">The JWT authentication service for token operations.</param>
/// <param name="authenticationService">The authentication service for user login operations.</param>
/// <param name="logger">The logger for logging authentication-related events.</param>
/// <param name="configuration">The application configuration.</param>
[ApiVersion("1")]
[ApiController]
[Route("api/v{version:apiVersion}/auth")]
public class AuthenticationController(
	JwtAuthentication jwtAuthentication,
	IAuthenticationService authenticationService,
	ILogger<AuthenticationController> logger,
	IConfiguration configuration) : ApiController
{
	private readonly JwtAuthentication _jwtAuthentication = jwtAuthentication;
	private readonly IConfiguration _configuration = configuration;
	private readonly IAuthenticationService _authenticationService = authenticationService;
	private readonly ILogger<AuthenticationController> _logger = logger;

	/// <summary>
	/// Authenticates a user and issues a JWT access token upon successful login.
	/// </summary>
	/// <remarks>
	/// This endpoint handles user login requests. It first checks if API authentication
	/// is enabled via configuration. If enabled, it attempts to authenticate the user
	/// using the provided email and password via the user service. Upon successful
	/// authentication, it generates a JWT access token.
	///
	/// Authentication failures (e.g., invalid credentials, locked out account) are communicated
	/// via an appropriate HTTP status code (e.g., <see cref="StatusCodes.Status401Unauthorized"/> or <see cref="StatusCodes.Status403Forbidden"/>)
	/// with an <see cref="Error"/> object in the response body detailing the failure.
	///
	/// This endpoint does NOT require previous authentication.
	/// </remarks>
	/// <param name="request">The authentication request containing user credentials (email and password).</param>
	/// <returns>
	/// <see cref="IActionResult"/> which can be:
	/// <list type="bullet">
	/// <item><term><see cref="OkObjectResult"/></term><description>with <see cref="AuthenticationResponse"/> upon success (HTTP 200).</description></item>
	/// <item><term><see cref="ObjectResult"/></term><description>with <see cref="Error"/> for various failure scenarios (HTTP 401 or 403).</description></item>
	/// </list>
	/// </returns>
	/// <response code="200">Returns user information and tokens upon successful authentication.</response>
	/// <response code="401">Returns object detailing invalid credentials.</response>
	/// <response code="403">Returns object detailing various access errors: API disabled, account locked out, or email not confirmed.</response>
	[HttpPost()]
	[AllowAnonymous]
	[ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(Error), StatusCodes.Status403Forbidden)]
	[ProducesResponseType(typeof(Error), StatusCodes.Status401Unauthorized)]
	[ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> Token([FromBody] AuthenticationRequest request)
	{
		if (!_configuration.GetValue<bool>("EnableAPIAuth"))
		{
			_logger.LogWarning("Authentication attempt while API authentication is disabled.");
			return Result.Bad(AuthenticationErrors.APIDisabled).ToActionResult();
		}

		var authenticationResult = await _authenticationService.AuthenticateAsync(request.Email, request.Password);

		if (authenticationResult.Succeeded == false)
		{
			if (authenticationResult.IsInvalidCredentials)
				return Result.Bad(AuthenticationErrors.InvalidCredentials).ToActionResult();

			if (authenticationResult.IsLockedOut)
				return Result.Bad(AuthenticationErrors.UserLockedOut).ToActionResult();

			if (authenticationResult.IsEmailNotConfirmed)
				return Result.Bad(AuthenticationErrors.UserEmailNotConfirmed).ToActionResult();

			if (authenticationResult.IsBlocked)
				return Result.Bad(AuthenticationErrors.UserBlocked).ToActionResult();

			_logger.LogError("Authentication failed for email {Email} with Succeeded=false but no specific error flag set in AuthenticationResult.", request.Email);
			return Result.Bad(AuthenticationErrors.InvalidCredentials).ToActionResult();
		}

		if (authenticationResult.User == null)
		{
			_logger.LogError("Authentication succeeded for email {Email} but User object in AuthenticationResult is null.", request.Email);
			return Result.Bad(AuthenticationErrors.ServiceError).ToActionResult();
		}

		var response = new AuthenticationResponse()
		{
			AccessToken = _jwtAuthentication.GenerateToken(authenticationResult.User!.Id, authenticationResult.User.Role)
		};

		return Ok(response);
	}
}