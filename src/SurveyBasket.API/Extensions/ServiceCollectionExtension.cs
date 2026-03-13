using SurveyBasket.Application;
using SurveyBasket.Infrastructure;

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

        return services;
    }
}