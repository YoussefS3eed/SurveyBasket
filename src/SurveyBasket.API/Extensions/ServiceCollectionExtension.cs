using SurveyBasket.API.Middleware;

namespace SurveyBasket.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddAPI(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            )
        );

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddScoped<RequestTimeLoggingMiddleware>();

        //builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
        //    .AddEntityFrameworkStores<ApplicationDbContext>();

        return services;
    }
}