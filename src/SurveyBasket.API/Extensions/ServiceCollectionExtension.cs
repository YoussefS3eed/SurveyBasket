using Asp.Versioning;
using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.API.Abstractions.Consts;
using SurveyBasket.Application;
using SurveyBasket.Infrastructure;
using System.Security.Claims;
using System.Threading.RateLimiting;

namespace SurveyBasket.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            )
        );

        //services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // // Application + Infrastructure layers
        services.AddApplication();
        services.AddInfrastructure(configuration);

        //services.AddScoped<RequestTimeLoggingMiddleware>();

        //builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>();

        // Add RateLimiting
        services.AddRateLimitingConfig();

        // Add Api Versioning
        services.AddApiVersioning(options =>
        {
            options.DefaultApiVersion = new ApiVersion(1);
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.ReportApiVersions = true;

            options.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        })
        .AddApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'V";
            options.SubstituteApiVersionInUrl = true;
        });

        return services;
    }

    private static IServiceCollection AddRateLimitingConfig(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            rateLimiterOptions.AddPolicy(RateLimiters.IpLimiter, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );

            rateLimiterOptions.AddPolicy(RateLimiters.UserLimiter, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    factory: _ => new FixedWindowRateLimiterOptions
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );

            rateLimiterOptions.AddConcurrencyLimiter(RateLimiters.Concurrency, options =>
            {
                options.PermitLimit = 1000;
                options.QueueLimit = 100;
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });

            rateLimiterOptions.OnRejected = async (context, token) =>
            {
                var logger = context.HttpContext.RequestServices
                    .GetRequiredService<ILoggerFactory>()
                    .CreateLogger("RateLimiting");

                var httpContext = context.HttpContext;

                var ip = httpContext.Connection.RemoteIpAddress?.ToString();

                var userId = httpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                            ?? "Anonymous";


                var hasRetryAfter = context.Lease.TryGetMetadata(
                    MetadataName.RetryAfter,
                    out var retryAfterValue);

                var seconds = hasRetryAfter
                    ? retryAfterValue.TotalSeconds
                    : 0;

                logger.LogWarning(
                    "Rate limit exceeded | IP: {IP} | UserId: {UserId} | RetryAfter: {Seconds}s",
                    ip,
                    userId,
                    seconds
                );

                await Task.CompletedTask;
            };

        });

        return services;
    }

}