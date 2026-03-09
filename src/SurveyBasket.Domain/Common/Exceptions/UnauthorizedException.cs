namespace SurveyBasket.Domain.Common.Exceptions;

public class UnauthorizedException(string message = "Invalid credentials") : Exception(message)
{
}
