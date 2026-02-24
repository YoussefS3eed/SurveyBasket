//using SurveyBasket.Domain.Entities;

//namespace SurveyBasket.Domain.Interfaces;  // or Application.Interfaces

//public interface IQuestionRepository
//{
//    Task<bool> PollExistsAsync(int pollId, CancellationToken cancellationToken = default);
//    Task<IEnumerable<QuestionResponse>> GetAllByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
//    Task<QuestionResponse?> GetByIdAsync(int pollId, int id, CancellationToken cancellationToken = default);
//    Task<Question?> GetQuestionWithAnswersAsync(int pollId, int id, CancellationToken cancellationToken = default);
//    Task<bool> QuestionContentExistsAsync(int pollId, string content, CancellationToken cancellationToken = default);
//    Task<bool> QuestionContentExistsExceptAsync(int pollId, string content, int excludeId, CancellationToken cancellationToken = default);
//    Task AddAsync(Question question, CancellationToken cancellationToken = default);
//    Task UpdateAsync(Question question, CancellationToken cancellationToken = default);
//}

using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Domain.Interfaces;

public interface IQuestionRepository
{
    Task<IEnumerable<Question>> GetByPollIdAsync(int pollId, CancellationToken cancellationToken = default);
    Task<Question?> GetByIdAsync(int pollId, int id, bool includeAnswers = true, CancellationToken cancellationToken = default);
    Task CreateAsync(Question question, CancellationToken cancellationToken = default);
    Task UpdateAsync(Question question, CancellationToken cancellationToken = default);
    Task<bool> ExistsByContentExceptIdAsync(int pollId, string content, int? excludeId = null, CancellationToken cancellationToken = default);
}