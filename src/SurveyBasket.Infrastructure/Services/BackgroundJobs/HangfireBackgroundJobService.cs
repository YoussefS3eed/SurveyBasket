using Hangfire;
using SurveyBasket.Application.Common.Interfaces;
using System.Linq.Expressions;

namespace SurveyBasket.Infrastructure.Services.BackgroundJobs;

internal class HangfireBackgroundJobService : IBackgroundJobService
{
    public string Enqueue<T>(Expression<Action<T>> methodCall)
        => BackgroundJob.Enqueue(methodCall);

    public string Schedule<T>(Expression<Action<T>> methodCall, TimeSpan delay)
        => BackgroundJob.Schedule(methodCall, delay);

    public string Schedule<T>(Expression<Action<T>> methodCall, DateTimeOffset scheduleAt)
        => BackgroundJob.Schedule(methodCall, scheduleAt);

    public void AddOrUpdateRecurring<T>(string jobId, Expression<Action<T>> methodCall, string cronExpression)
        => RecurringJob.AddOrUpdate(jobId, methodCall, cronExpression);

    public void RemoveRecurringJob(string jobId)
        => RecurringJob.RemoveIfExists(jobId);
}
