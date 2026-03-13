using SurveyBasket.Domain.Common.Dtos;
using SurveyBasket.Domain.Common.Models;

namespace SurveyBasket.Application.Features.Users.Queries.GetProfile;

public sealed record GetProfileQuery() : IRequest<Result<UserProfileDto>>;
