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
        config.NewConfig<Question, QuestionResponseDto>()
            .Map(dest => dest.Answers, src => src.Answers.Adapt<IEnumerable<AnswerResponseDto>>());

        config.NewConfig<Question, AvailableQuestionResponseDto>()
            .Map(dest => dest.Answers, src => src.Answers.Where(a => a.IsActive).Adapt<IEnumerable<AnswerResponseDto>>());

        // Vote → VoteResponseDto
        config.NewConfig<Vote, VoteResponseDto>()
            .Map(dest => dest.VoterName, src => $"{src.User.FirstName} {src.User.LastName}")
            .Map(dest => dest.VoteDate, src => src.SubmittedOn)
            .Map(dest => dest.SelectedAnswers, src => src.VoteAnswers);

        // VoteAnswer → QuestionAnswerResponseDto
        config.NewConfig<VoteAnswer, QuestionAnswerResponseDto>()
            .Map(dest => dest.Question, src => src.Question.Content)
            .Map(dest => dest.Answer, src => src.Answer.Content);

        config.NewConfig<RegisterCommand, ApplicationUser>()
            .Map(dest => dest.UserName, src => src.Username);

    }
}