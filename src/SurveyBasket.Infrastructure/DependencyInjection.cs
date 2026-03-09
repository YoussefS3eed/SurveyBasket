using Hangfire;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.Application.Common.Caching;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Domain.Interfaces;
using SurveyBasket.Infrastructure.Persistence;
using SurveyBasket.Infrastructure.Persistence.Repositories;
using SurveyBasket.Infrastructure.Services;
using SurveyBasket.Infrastructure.Services.Auth;
using SurveyBasket.Infrastructure.Services.Cache;
using SurveyBasket.Infrastructure.Services.Email;
using SurveyBasket.Infrastructure.Services.Notification;
using System.Text;

namespace SurveyBasket.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        // Identity
        services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;

            options.SignIn.RequireConfirmedEmail = true;

            options.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            options.User.RequireUniqueEmail = true;
        });

        // Auth / JWT 
        services.AddAuthConfig(configuration);

        // Unit of Work
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repositories
        services.AddScoped<IPollRepository, PollRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IQuestionRepository, QuestionRepository>();
        services.AddScoped<IVoteRepository, VoteRepository>();
        services.AddScoped<IResultRepository, ResultRepository>();
        services.AddScoped<INotificationRepository, NotificationRepository>();

        // Services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddScoped<IApplicationUrlService, ApplicationUrlService>();

        // Mail Settings
        services.Configure<EmailSettings>(configuration.GetSection(EmailSettings.SectionName));

        // Casching
        services.AddCaching(configuration);

        // Hangfire 
        services.AddBackgroundJobs(configuration);

        // Http Context
        services.AddHttpContextAccessor();

        return services;
    }
    private static IServiceCollection AddAuthConfig(this IServiceCollection services, IConfiguration configuration)
    {
        // JWT
        services.AddSingleton<IJwtService, JwtService>();

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience
            };
        });
        return services;
    }
    private static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
    {
        // Add HybridCache with in-memory only (no Redis)
        services.AddHybridCache(options =>
        {
            options.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                LocalCacheExpiration = TimeSpan.FromMinutes(10),
                Expiration = TimeSpan.FromMinutes(20)
            };

            //options.MaximumPayloadBytes = 1024 * 1024; // 1MB
            //options.MaximumKeyLength = 512;
        });

        // Register our cache abstraction
        services.AddScoped<ICacheService, HybridCacheService>();

        return services;
    }

    private static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(
                configuration.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer();

        return services;
    }
}