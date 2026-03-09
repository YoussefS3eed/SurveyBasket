namespace SurveyBasket.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNewPollsNotification(int? pollId = null, CancellationToken cancellationToken = default);
}
