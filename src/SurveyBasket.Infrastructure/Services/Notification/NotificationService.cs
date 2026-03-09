using SurveyBasket.Application.Common.Interfaces;

namespace SurveyBasket.Infrastructure.Services.Notification;

internal sealed class NotificationService(
    INotificationRepository notificationRepository,
    IEmailService emailService,
    IApplicationUrlService urlService) : INotificationService
{
    public async Task SendNewPollsNotification(int? pollId = null, CancellationToken ct = default)
    {
        var polls = await notificationRepository.GetPollsForNotificationAsync(pollId, ct);
        var users = await notificationRepository.GetUsersForNotificationAsync(ct);

        var origin = urlService.GetOrigin();

        foreach (var poll in polls)
        {
            foreach (var user in users)
            {
                var pollUrl = $"{origin}/polls/start/{poll.Id}";

                // ✅ pollEndDate (poll.EndsAt) was missing — now passed correctly
                await emailService.SendPollNotificationAsync(
                    toEmail: user.Email!,
                    displayName: user.FullName,
                    pollTitle: poll.Title,
                    pollUrl: pollUrl,
                    pollEndDate: poll.EndsAt,    // ← this was the missing argument
                    ct: ct);
            }
        }
    }
}