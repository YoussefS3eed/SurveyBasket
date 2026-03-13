namespace SurveyBasket.API.Contracts.Users;

public record UpdateProfileCommandUpdateProfileRequest(
    string FirstName,
    string LastName
);
