using SurveyBasket.Application.Features.Answers.Dtos;
using SurveyBasket.Application.Features.Authentication.Commands.Register;
using SurveyBasket.Application.Features.Questions.Dtos;
using SurveyBasket.Application.Features.Results.Dtos;
using SurveyBasket.Application.Features.Votes.Dtos;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Common.Mappings;

public class MappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Question, QuestionResponse>()
            .Map(dest => dest.Answers, src => src.Answers.Adapt<IEnumerable<AnswerResponse>>());

        config.NewConfig<Question, AvailableQuestionResponse>()
            .Map(dest => dest.Answers, src => src.Answers.Where(a => a.IsActive).Adapt<IEnumerable<AnswerResponse>>());

        // Vote → VoteResponse
        config.NewConfig<Vote, VoteResponse>()
            .Map(dest => dest.VoterName, src => $"{src.User.FirstName} {src.User.LastName}")
            .Map(dest => dest.VoteDate, src => src.SubmittedOn)
            .Map(dest => dest.SelectedAnswers, src => src.VoteAnswers);

        // VoteAnswer → QuestionAnswerResponse
        config.NewConfig<VoteAnswer, QuestionAnswerResponse>()
            .Map(dest => dest.Question, src => src.Question.Content)
            .Map(dest => dest.Answer, src => src.Answer.Content);

        config.NewConfig<RegisterCommand, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Username);

    }
}