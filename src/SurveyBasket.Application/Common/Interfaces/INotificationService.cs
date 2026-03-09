namespace SurveyBasket.Application.Common.Interfaces;

public interface INotificationService
{
    Task SendNewPollsNotificationAsync(int? pollId = null, CancellationToken cancellationToken = default);
}
