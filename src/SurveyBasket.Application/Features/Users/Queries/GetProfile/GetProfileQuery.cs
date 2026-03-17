using SurveyBasket.Domain.Common.Dtos;

namespace SurveyBasket.Application.Features.Users.Queries.GetProfile;

public sealed record GetProfileQuery() : IRequest<Result<UserProfileDto>>;
