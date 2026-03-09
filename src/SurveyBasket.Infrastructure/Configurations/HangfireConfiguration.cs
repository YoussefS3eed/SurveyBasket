using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurveyBasket.Application.Common.Interfaces;
using SurveyBasket.Infrastructure.Services.BackgroundJobs;

namespace SurveyBasket.Infrastructure.Configurations;

public static class HangfireConfiguration
{
    internal static IServiceCollection AddBackgroundJobs(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(
                configuration.GetConnectionString("HangfireConnection")));

        services.AddHangfireServer();

        services.AddScoped<IBackgroundJobService, HangfireBackgroundJobService>();

        return services;
    }

    public static IApplicationBuilder InitializeRecurringJobs(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var jobService = scope.ServiceProvider.GetRequiredService<IBackgroundJobService>();
        var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

        // Schedule daily poll notifications
        jobService.AddOrUpdateRecurring<INotificationService>(
            "daily-poll-notifications",
            service => service.SendNewPollsNotification(null, default),
            Cron.Daily());

        return app;
    }
}
