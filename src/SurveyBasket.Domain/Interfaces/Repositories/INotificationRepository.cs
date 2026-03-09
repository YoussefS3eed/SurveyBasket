namespace SurveyBasket.Domain.Interfaces.Repositories;

public interface INotificationRepository
{
    Task<IEnumerable<Poll>> GetPollsForNotificationAsync(int? pollId = null, CancellationToken ct = default);
    Task<IEnumerable<ApplicationUser>> GetUsersForNotificationAsync(CancellationToken ct = default);
}
