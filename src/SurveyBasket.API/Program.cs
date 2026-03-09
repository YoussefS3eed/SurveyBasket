using Hangfire;
using Hangfire.Dashboard;
using Serilog;
using SurveyBasket.API.Middleware;
using SurveyBasket.Application.Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddDependencies(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration)
);

var app = builder.Build();

app.UseExceptionHandler();

// Configure pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard("/jobs", new DashboardOptions
{
    Authorization = [new LocalRequestsOnlyAuthorizationFilter()],
    DashboardTitle = "Survey Basket Dashboard"
});

app.MapControllers();

RecurringJob.AddOrUpdate<INotificationService>(
    "SendNewPollsNotification",
    svc => svc.SendNewPollsNotificationAsync(null, default),
    Cron.Daily);



//app.MapIdentityApi<ApplicationUser>();


app.Run();