using Application.Localization;
using Application.Users.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jewelry.API.Controllers;

/// <summary>
/// Authentication and user management operations
/// </summary>
public class AuthController : BaseController
{
    private readonly ILocalizationService _localization;

    public AuthController(ILocalizationService localization)
    {
        _localization = localization;
    }
    /// <summary>
    /// Register a new user
    /// </summary>
    /// <param name="command">User registration details</param>
    /// <returns>Created user ID</returns>
    /// <response code="200">User successfully registered</response>
    /// <response code="400">Invalid request data</response>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<Guid>> Register([FromBody] RegisterUserCommand command)
    {
        try
        {
            var userId = await Mediator.Send(command);
            return Ok(new { userId, message = _localization.GetString("User.RegisteredSuccessfully") });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = _localization.GetString("User.AlreadyExists") });
        }
    }

    /// <summary>
    /// Authenticate user and get access token
    /// </summary>
    /// <param name="request">Login credentials</param>
    /// <returns>Authentication result with tokens</returns>
    /// <response code="200">Successfully authenticated</response>
    /// <response code="401">Invalid credentials</response>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResult>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            var command = new AuthenticateUserCommand(
                request.Email,
                request.Password,
                ipAddress,
                userAgent
            );

            var result = await Mediator.Send(command);
            
            // Set refresh token in httpOnly cookie
            SetRefreshTokenCookie(result.RefreshToken, result.ExpiresAt);
            
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = _localization.GetString("User.InvalidCredentials") });
        }
    }

    /// <summary>
    /// Refresh access token using refresh token
    /// </summary>
    /// <param name="request">Refresh token request</param>
    /// <returns>New authentication tokens</returns>
    /// <response code="200">Token refreshed successfully</response>
    /// <response code="401">Invalid or expired refresh token</response>
    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthenticationResult>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        try
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var userAgent = HttpContext.Request.Headers["User-Agent"].ToString();

            // Try to get refresh token from cookie if not provided in body
            var refreshToken = request.RefreshToken ?? Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { error = "Refresh token is required" });
            }

            var command = new RefreshTokenCommand(
                request.Email,
                refreshToken,
                ipAddress,
                userAgent
            );

            var result = await Mediator.Send(command);
            
            // Set new refresh token in cookie
            SetRefreshTokenCookie(result.RefreshToken, result.ExpiresAt);
            
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = _localization.GetString("User.InvalidToken") });
        }
    }

    /// <summary>
    /// Revoke refresh token (logout)
    /// </summary>
    /// <param name="request">Revoke token request</param>
    /// <returns>Success message</returns>
    /// <response code="200">Token revoked successfully</response>
    /// <response code="401">Invalid token</response>
    [HttpPost("logout")]
    public async Task<ActionResult> Logout([FromBody] RevokeTokenRequest request)
    {
        try
        {
            // Try to get refresh token from cookie if not provided in body
            var refreshToken = request.RefreshToken ?? Request.Cookies["refreshToken"];
            
            if (string.IsNullOrEmpty(refreshToken))
            {
                return BadRequest(new { error = _localization.GetString("User.RefreshTokenRequired") });
            }

            var command = new RevokeRefreshTokenCommand(
                request.Email,
                refreshToken,
                "Logout requested by user"
            );

            await Mediator.Send(command);
            
            // Remove refresh token cookie
            Response.Cookies.Delete("refreshToken");
            
            return Ok(new { message = _localization.GetString("User.LoggedOutSuccessfully") });
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(new { error = _localization.GetString("User.InvalidToken") });
        }
    }

    private void SetRefreshTokenCookie(string refreshToken, DateTime expiresAt)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expiresAt,
            Secure = true, // Enable in production with HTTPS
            SameSite = SameSiteMode.Strict,
            IsEssential = true
        };
        
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
}

/// <summary>
/// Login request model
/// </summary>
public record LoginRequest(string Email, string Password);

/// <summary>
/// Refresh token request model
/// </summary>
public record RefreshTokenRequest(string Email, string? RefreshToken = null);

/// <summary>
/// Revoke token request model
/// </summary>
public record RevokeTokenRequest(string Email, string? RefreshToken = null);

