using SurveyBasket.Application.Answers.Dtos;
using SurveyBasket.Application.Authentication.Commands.Register;
using SurveyBasket.Application.Polls.Commands.CreatePoll;
using SurveyBasket.Application.Votes.Dtos;
using SurveyBasket.Contracts.Results;
using SurveyBasket.Domain.Entities;

namespace SurveyBasket.Application.Mapping;

public class MappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {

        config.NewConfig<CreatePollRequest, CreatePollCommand>()
            .MapWith(src => new CreatePollCommand(
                src.Title,
                src.Summary,
                src.IsPublished,
                src.StartsAt,
                src.EndsAt
            ));

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