using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces.Repositories;

namespace SurveyBasket.API.Middleware;

internal sealed class SecurityStampValidationMiddleware(
    RequestDelegate next,
    ILogger<SecurityStampValidationMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context, IUserRepository userRepository, IJwtService jwtService)
    {
        // Skip if no authorization header
        var authHeader = context.Request.Headers.Authorization.FirstOrDefault();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            await next(context);
            return;
        }

        // Skip endpoints with [AllowAnonymous] attribute
        var endpoint = context.GetEndpoint();
        if (endpoint?.Metadata.GetMetadata<IAllowAnonymous>() != null)
        {
            await next(context);
            return;
        }

        // Extract token
        var token = authHeader["Bearer ".Length..].Trim();

        // Get user ID from claims (already validated by Authentication middleware)
        var userId = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId))
        {
            await next(context);
            return;
        }

        // Get current security stamp from database
        var user = await userRepository.GetByIdAsync(userId, context.RequestAborted);
        if (user is null)
        {
            await next(context);
            return;
        }

        // Validate security stamp
        var validationResult = jwtService.ValidateTokenWithSecurityStamp(token, user.SecurityStamp);

        if (validationResult == null)
        {
            // Token is invalid/expired
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Invalid token",
                message = "Token is invalid or expired.",
                code = "INVALID_TOKEN"
            }, context.RequestAborted);
            return;
        }

        // Special handling for verify-email endpoint
        // Allow invalid security stamp (user may have changed email)
        // The verification code will provide the actual authorization
        var path = context.Request.Path.Value?.ToLowerInvariant();

        if (!validationResult.Value.IsStampValid)
        {
            // Security stamp mismatch - token has been invalidated
            logger.LogInformation("Invalid security stamp detected for user {UserId}. Token has been invalidated.", userId);
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsJsonAsync(new
            {
                error = "Invalid token",
                message = "Your session has been invalidated. Please login again.",
                code = "INVALID_SECURITY_STAMP"
            }, context.RequestAborted);
            return;
        }

        await next(context);
    }
}
