using SurveyBasket.Application.Answers.Dtos;
using SurveyBasket.Application.Votes.Dtos;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Question, AvailableQuestionResponse>()
            .Map(dest => dest.Answers, src => src.Answers.Where(a => a.IsActive).Adapt<IEnumerable<AnswerResponse>>());
    }
}