using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.Infrastructure.Persistence.Repositories;

internal sealed class NotificationRepository(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager) : INotificationRepository
{
    public async Task<IEnumerable<Poll>> GetPollsForNotificationAsync(
        int? pollId = null, CancellationToken ct = default)
    {
        // If a specific pollId is given, return that one poll (must be published).
        // Otherwise return all polls that are published and start today — used by the daily job.
        return pollId.HasValue
            ? await context.Polls
                .Where(p => p.Id == pollId && p.IsPublished)
                .AsNoTracking()
                .ToListAsync(ct)
            : await context.Polls
                .Where(p => p.IsPublished
                    && p.StartsAt == DateOnly.FromDateTime(DateTime.UtcNow))
                .AsNoTracking()
                .ToListAsync(ct);
    }

    public async Task<IEnumerable<ApplicationUser>> GetUsersForNotificationAsync(
        CancellationToken ct = default)
    {
        // TODO: Filter to subscribed/member users only when membership is implemented.
        // For now returns all confirmed users.
        return await userManager.Users
            .Where(u => u.EmailConfirmed)
            .AsNoTracking()
            .ToListAsync(ct);
    }
}
